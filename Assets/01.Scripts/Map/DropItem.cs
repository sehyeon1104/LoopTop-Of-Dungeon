using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour, IPoolable
{

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    // 현재 지닌 아이템 리스트
    private List<Item> tempItemList = new List<Item>();
    private Item item = null;
    //private bool isItemSetting = false;
    //private bool isDuplication = false;
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

    private void Awake()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        Init();
    }

    private void Start()
    {
        interactionButton = UIManager.Instance.GetInteractionButton();
        for (int i = 0; i < itemObjList.Count; ++i)
        {
            itemObjListNum.Add(itemObjList[i].Num);
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
        itemObjList = StageManager.Instance.shop.itemList;
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
        if (tempItemList.Count == GameManager.Instance.allItemList.Count - 1)
        {
            Debug.LogError("아이템 부족. 빨리 개발 더 해.");
            return;
        }

        // 현재 지닌 아이템 리스트
        tempItemList = GameManager.Instance.GetItemList();

        for (int i = 0; i < tempItemList.Count; ++i)
        {
            //Debug.Log("ItemName : " + tempItemList[i].itemName);
            //Debug.Log("ItemNumber : " + tempItemList[i].itemNumber);
            // 현재 지닌 아이템 리스트 복사
            itemSelectNum.Add(tempItemList[i].itemNumber);
        }

        int rand = 0;

        List<Item> allItemList = new List<Item>();
        allItemList = GameManager.Instance.allItemList;

        while (item == null)
        {
            // 저주아이템을 제외한 모든 아이템 rand
            rand = Random.Range(1, GameManager.Instance.allItemList.Count - GameManager.Instance.brokenItemCount);

            // 현재 지닌 아이템 또는 상점에 있는 아이템일 경우 continue
            if (itemSelectNum.Contains(rand) || itemObjListNum.Contains(rand))
                continue;

            Debug.Log("rand : " + rand);

            itemSelectNum.Add(rand);

            item = GameManager.Instance.allItemList[rand];
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
        }
    }
    private void OnTriggerExit2D(Collider2D collision)  
    {
        if(collision.CompareTag("Player"))
        {
           interactionButton.onClick.RemoveListener(TakeItem);
           UIManager.Instance.RotateAttackButton();
        }
    }

    // 아이템 획득 함수
    public void TakeItem()
    {
        if (GameManager.Instance.GetItemList().Contains(item))
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
