using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour, IPoolable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    // ���� ���� ������ ����Ʈ
    private List<Item> tempItemList = new List<Item>();
    private List<int> tempBrokenItemList = new List<int>();
    private Item item = null;
    Button interactionButton;

    private HashSet<int> itemSelectNum = new HashSet<int>();
    // ������ ���� ������
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
            Debug.LogError("������ ����. ���� ��ȹ/���� �� ��.");
            return;
        }

        if (chestRate == Define.ChestRating.Special)
        {
            SetBrokenItem();
            return;
        }

        // ���� ���� ������ ����Ʈ
        foreach(Item item in ItemManager.Instance.GetCurItemDic().Values)
        {
            tempItemList.Add(item);
        }

        for (int i = 0; i < tempItemList.Count; ++i)
        {
            // ���� ���� ������ ����Ʈ ����
            itemSelectNum.Add(tempItemList[i].itemNumber);
        }

        int rand = 0;

        Dictionary<string, Item> allItemDic = ItemManager.Instance.allItemDic;
        List<Item> allItemList = allItemDic.Values.ToList();

        item = ItemManager.Instance.allItemDic["MirrorOfSun"];

        while (item == null)
        {
            // ���־������� ������ ��� ������ rand
            rand = Random.Range(1, ItemManager.Instance.allItemDic.Count);

            // ���� ���� ������ �Ǵ� ������ �ִ� �������� ��� continue
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
        // ���� ���� ������ ����Ʈ
        foreach (Item item in ItemManager.Instance.GetCurItemDic().Values)
        {
            tempItemList.Add(item);
        }

        for (int i = 0; i < tempItemList.Count; ++i)
        {
            // ���� ���� ������ ����Ʈ ����
            itemSelectNum.Add(tempItemList[i].itemNumber);
        }

        if(StageManager.Instance.eventRoomCountDic[Define.EventRoomTypeFlag.BrokenItemRoom] == 0)
        {
            tempBrokenItemList = FindObjectOfType<BrokenItemRoom>().GetItemList();
        }

        int rand = 0;

        while (item == null)
        {
            // ���־������� ������ ��� ������ rand
            rand = Random.Range(1, ItemManager.Instance.brokenItemList.Count);

            // ���� ���� ������ �Ǵ� ������ �ִ� �������� ��� continue
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

    // ������ ȹ�� �Լ�
    public void TakeItem()
    {
        if (ItemManager.Instance.GetCurItemDic().ContainsKey(item.itemNameEng))
        {
            Debug.Log("�̹� �������ִ� ������");
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
