using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour, IPoolable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    // 현재 지닌 아이템 리스트
    private List<Item> tempItemList = new List<Item>();
    private List<int> tempBrokenItemList = new List<int>();
    private Item item = null;
    Button interactionButton;

    private HashSet<int> itemSelectNum = new HashSet<int>();
    // 상점에 나온 아이템
    private List<ItemObj> itemObjList = null;
    private List<int> itemObjListNum = new List<int>();

    private Vector3 movePos;
    [SerializeField]
    private float delta = 0.05f;
    [SerializeField]
    private float speed = 1f;

    private Poolable poolable = null;

    private WaitForEndOfFrame waitForEndOfFrame;

    [SerializeField]
    private GameObject canvas = null;

    private void Awake()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        Init();
    }

    private void Start()
    {
        interactionButton = UIManager.Instance.GetInteractionButton();
        if (itemObjList != null)
        {
            for (int i = 0; i < itemObjList.Count; ++i)
            {
                itemObjListNum.Add(itemObjList[i].Num);
            }
        }

        SetItem(Define.ChestRating.Common);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        item = null;
        spriteRenderer.sprite = null;
        itemSelectNum.Clear();
        tempItemList.Clear();
        if(GameManager.Instance.sceneType == Define.Scene.Field)
        {
            itemObjList = StageManager.Instance.shop.itemList;
        }
        poolable = GetComponent<Poolable>();
    }

    public void PoolInit()
    {
        item = null;
        spriteRenderer.sprite = null;
        itemSelectNum.Clear();
        tempItemList.Clear();
        itemObjList = StageManager.Instance.shop.itemList;
    }

    public void SetItem(Define.ChestRating chestRate)
    {
        if (tempItemList.Count == ItemManager.Instance.allItemDic.Count - 1)
        {
            Debug.LogError("아이템 부족. 빨리 기획/개발 더 해.");
            return;
        }

        if (chestRate == Define.ChestRating.Special)
        {
            SetBrokenItem();
            return;
        }

        // 현재 지닌 아이템 리스트
        foreach(Item item in ItemManager.Instance.GetCurItemDic().Values)
        {
            tempItemList.Add(item);
        }

        for (int i = 0; i < tempItemList.Count; ++i)
        {
            // 현재 지닌 아이템 리스트 복사
            itemSelectNum.Add(tempItemList[i].itemNumber);
        }

        int rand = 0;

        Dictionary<string, Item> allItemDic = ItemManager.Instance.allItemDic;
        List<Item> allItemList = allItemDic.Values.ToList();

        item = ItemManager.Instance.allItemDic["MirrorOfSun"];

        while (item == null)
        {
            // 저주아이템을 제외한 모든 아이템 rand
            rand = Random.Range(1, ItemManager.Instance.allItemDic.Count);

            // 현재 지닌 아이템 또는 상점에 있는 아이템일 경우 continue
            if (itemSelectNum.Contains(allItemList[rand].itemNumber) 
                || itemObjListNum.Contains(rand) 
                || allItemList[rand].itemRating == Define.ItemRating.Special
                || allItemList[rand].itemRating == Define.ItemRating.Set)
            continue;

            itemSelectNum.Add(allItemList[rand].itemNumber);

            foreach(Item items in allItemDic.Values)
            {
                if (items.itemNumber == allItemList[rand].itemNumber)
                {
                    item = items;
                    break;
                }
            }
        }

        spriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
    }

    public void SetBrokenItem()
    {
        // 현재 지닌 아이템 리스트
        foreach (Item item in ItemManager.Instance.GetCurItemDic().Values)
        {
            tempItemList.Add(item);
        }

        for (int i = 0; i < tempItemList.Count; ++i)
        {
            // 현재 지닌 아이템 리스트 복사
            itemSelectNum.Add(tempItemList[i].itemNumber);
        }

        if(StageManager.Instance.eventRoomCountDic[Define.EventRoomTypeFlag.BrokenItemRoom] == 0)
        {
            tempBrokenItemList = FindObjectOfType<BrokenItemRoom>().GetItemList();
        }

        int rand = 0;

        while (item == null)
        {
            // 저주아이템을 제외한 모든 아이템 rand
            rand = Random.Range(1, ItemManager.Instance.brokenItemList.Count);

            // 현재 지닌 아이템 또는 상점에 있는 아이템일 경우 continue
            if (itemSelectNum.Contains(rand)
                || itemObjListNum.Contains(rand)
                || tempBrokenItemList.Contains(ItemManager.Instance.brokenItemList[rand].itemNumber) 
                || ItemManager.Instance.brokenItemList[rand].itemNumber == 510
                )
                continue;

            itemSelectNum.Add(rand);

            foreach (Item items in ItemManager.Instance.brokenItemList)
            {
                if (items.itemNumber == ItemManager.Instance.brokenItemList[rand].itemNumber)
                {
                    item = items;
                    break;
                }
            }
        }

        spriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
    }

    private IEnumerator MoveUpDown()
    {
        while (true)
        {
            movePos = transform.position;
            movePos.y += delta * Mathf.Sin(Time.time * speed);
            transform.position = movePos;

            yield return waitForEndOfFrame;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            interactionButton.onClick.RemoveListener(TakeItem);
            interactionButton.onClick.AddListener(TakeItem);
            UIManager.Instance.RotateInteractionButton();
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  
    {
        if(collision.CompareTag("Player"))
        {
           interactionButton.onClick.RemoveListener(TakeItem);
           UIManager.Instance.RotateAttackButton();
            canvas.SetActive(false);
        }
    }

    // 아이템 획득 함수
    public void TakeItem()
    {
        if (ItemManager.Instance.GetCurItemDic().ContainsKey(item.itemNameEng))
        {
            Debug.Log("이미 가지고있는 아이템");
            return;
        }

        if (!ItemAbility.Items[item.itemNumber].isOneOff)
        {
            InventoryUI.Instance.AddItemSlot(item);
        }
        else
        {
            ItemAbility.Items[item.itemNumber].Use();
        }

        UIManager.Instance.RotateAttackButton();
        interactionButton.onClick.RemoveListener(TakeItem);
        Managers.Pool.Push(poolable);
    }
}
