using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    // ���� ���� ������ ����Ʈ
    private List<Item> tempItemList = new List<Item>();
    private Item item = null;
    //private bool isItemSetting = false;
    //private bool isDuplication = false;
    Button interactionButton;

    private HashSet<int> itemSelectNum = new HashSet<int>();
    // ������ ���� ������
    private List<ItemObj> itemObjList = null;
    private List<int> itemObjListNum = new List<int>();

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

    private void Init()
    {
        item = null;
        spriteRenderer.sprite = null;
        itemSelectNum.Clear();
        tempItemList.Clear();
        tempItemList = GameManager.Instance.GetItemList();
        itemObjList = FindObjectOfType<ShopRoom>().itemList;
    }

    public void SetItem(Define.ChestRating chestRate)
    {
        if (tempItemList.Count == GameManager.Instance.allItemList.Count - 1)
        {
            Debug.LogError("������ ����. ���� ��ȹ �� ��.");
            return;
        }


        for (int i = 0; i < tempItemList.Count; ++i)
        {
            // ���� ���� ������ ����Ʈ ����
            itemSelectNum.Add(tempItemList[i].itemNumber);
        }

        int rand = 0;

        List<Item> allItemList = new List<Item>();
        allItemList = GameManager.Instance.allItemList;

        while (item == null)
        {
            // ���־������� ������ ��� ������ rand
            rand = Random.Range(1, GameManager.Instance.allItemList.Count - GameManager.Instance.brokenItemCount);

            // ���� ���� ������ �Ǵ� ������ �ִ� �������� ��� continue
            if (itemSelectNum.Contains(rand) || itemObjListNum.Contains(rand))
                continue;

            itemSelectNum.Add(rand);

            item = GameManager.Instance.allItemList[rand];
        }

        spriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
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

    // ������ ȹ�� �Լ�
    public void TakeItem()
    {
        // TODO : ȹ�� �� ����Ʈ ���

        if (GameManager.Instance.GetItemList().Contains(item))
        {
            Debug.Log("�̹� �������ִ� ������");
            return;
        }

        InventoryUI.Instance.AddItemSlot(item);
        UIManager.Instance.RotateAttackButton();
        interactionButton.onClick.RemoveListener(TakeItem);
        Managers.Pool.Push(this.GetComponent<Poolable>());
    }
}
