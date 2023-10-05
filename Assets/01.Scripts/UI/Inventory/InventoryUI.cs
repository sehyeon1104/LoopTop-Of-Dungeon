using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoSingleton<InventoryUI>
{
    [SerializeField]
    private GameObject inventoryPanel;

    public List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField]
    private Transform slotHolder;

    [SerializeField]
    private GameObject itemObjTemplate = null;
    [SerializeField]
    private GameObject uiItemObjTemplate = null;

    public Dictionary<string, UIInventorySlot> uiInventorySlotDic = new Dictionary<string, UIInventorySlot>();

    private List<Poolable> itemSlotObjList = new List<Poolable>();

    private void Awake()
    {
        if(itemObjTemplate == null)
        {
            Debug.Log("itemObjTemplate is null!");
            itemObjTemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/ItemSlot.prefab");
        }
        uiItemObjTemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/UIItemSlot.prefab");
        inventoryPanel = transform.Find("Background").gameObject;
        Init();
    }

    private void Init()
    {
        // Debug.Log("아이템 로딩");
        slots.Clear();
        if(slotHolder.childCount > 0)
        {
            for (int i = 0; i < slotHolder.childCount; ++i)
            {
                {
                    Destroy(slotHolder.GetChild(0).gameObject);
                }
            }
        }
    }

    public void ToggleInventoryUI()
    {
        if (UIManager.Instance.isSetting)
            return;

        //inventoryPanel.SetActive(!inventoryPanel.gameObject.activeSelf);
        if (!inventoryPanel.gameObject.activeSelf)
        {
            UIManager.Instance.PushPanel(inventoryPanel);
            MouseManager.Lock(false);
            MouseManager.Show(true);
        }
        else
        {
            UIManager.Instance.PopPanel();
            MouseManager.Lock(true);
            MouseManager.Show(false);
        }
        Inventory.Instance.ClearText();
    }

    // 아이템 획득시 슬롯에 추가
    public void AddItemSlot(Item item)
    {
        ItemManager.Instance.AddCurItemDic(item);
        // GameManager.Instance.AddItemData(item);

        //if (ItemManager.Instance.CheckSetItem(item))
        //{
        //    return;
        //}

        Poolable newObject = null;
        InventorySlot newItemObjComponent = null;

        Item inventoryItem = item;

        newObject = Managers.Pool.Pop(itemObjTemplate);
        itemSlotObjList.Add(newObject);
        newObject.transform.GetChild(0).GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
        newItemObjComponent = newObject.GetComponent<InventorySlot>();
        newItemObjComponent.SetValue(inventoryItem);
        newObject.transform.SetParent(slotHolder);
        newObject.gameObject.SetActive(true);
        slots.Add(newItemObjComponent);
        ItemAbility.Items[item.itemNumber].Use();

        // 스택형 아이템 표기
        //GameObject uiObj = null;
        //UIInventorySlot uiObjComponent = null;
        //Item uiObjItem = item;
        //uiObj = Instantiate(uiItemObjTemplate);
        //uiObjComponent = uiObj.GetComponent<UIInventorySlot>();
        //uiObjComponent.SetValue(uiObjItem);
        //uiObj.SetActive(true);
        //uiInventorySlotDic.Add(uiObjItem.itemName, uiObjComponent);

        // UIManager.Instance.AddItemListUI(item);
        StartCoroutine(UIManager.Instance.ShowObtainItemInfo(item));
    }

    // 현재 획득한 아이템 목록 가져옴
    public void LoadItemSlot()
    {
        Dictionary<string, Item> itemDic = ItemManager.Instance.GetCurItemDic();
        //List<Item> itemList = GameManager.Instance.GetItemList();

        if(itemSlotObjList.Count > 0)
        {
            foreach(var item in itemSlotObjList)
            {
                Managers.Pool.Push(item);
            }
        }

        Poolable newObject = null;
        InventorySlot newItemObjComponent = null;

        foreach(var items in itemDic.Values)
        {
            Debug.Log(items.itemName);
            if (items.itemNumber == 0)
                continue;

            Item inventoryItem = items;

            newObject = Managers.Pool.Pop(itemObjTemplate);
            itemSlotObjList.Add(newObject);

            newObject.transform.GetChild(0).GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{inventoryItem.itemRating}/{inventoryItem.itemNameEng}.png");
            newItemObjComponent = newObject.GetComponent<InventorySlot>();
            newItemObjComponent.SetValue(inventoryItem);
            newObject.transform.SetParent(slotHolder);
            newObject.gameObject.SetActive(true);
            slots.Add(newItemObjComponent);
            if (ItemAbility.Items[inventoryItem.itemNumber].isPersitantItem)
            {
                ItemAbility.Items[inventoryItem.itemNumber].LastingEffect();
            }

            // UIManager.Instance.AddItemListUI(items);
        }
    }

    /// <summary>
    /// 아이템 제거 코드. 제거하려는 아이템을 넣으면 됨
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItemSlot(Item item)
    {
        ItemAbility.Items[item.itemNumber].Disabling();
        ItemManager.Instance.RemoveCurItemDic(item);
        LoadItemSlot();
    }

}
