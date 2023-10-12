using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoSingleton<InventoryUI>
{
    [SerializeField]
    private GameObject inventoryPanel;

    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<GameObject> uiItemSlots = new List<GameObject>();
    [SerializeField]
    private Transform slotHolder;
    [SerializeField]
    private Transform uiItemSlotHolder;

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
        slots.Clear();

        if (slotHolder.childCount <= 0) return;

        foreach(Transform trm in slotHolder.transform)
        {
            Destroy(trm);
        }
    }

    public void ToggleInventoryUI()
    {
        if (UIManager.Instance.isSetting)
            return;

        if (inventoryPanel.activeSelf)
        {
            UIManager.Instance.PopPanel();
            MouseManager.Lock(true);
            MouseManager.Show(false);
        }
        else
        {
            UIManager.Instance.PushPanel(inventoryPanel);
            MouseManager.Lock(false);
            MouseManager.Show(true);
        }

        Inventory.Instance.ClearText();
    }

    // 아이템 획득시 슬롯에 추가
    public void AddItemSlot(Item item)
    {
        ItemManager.Instance.AddCurItemDic(item);
        Item inventoryItem = item;

        Poolable newObject = Managers.Pool.Pop(itemObjTemplate);
        itemSlotObjList.Add(newObject);

        newObject.transform.SetParent(slotHolder);
        newObject.gameObject.SetActive(true);

        newObject.transform.GetChild(0).GetComponent<Image>().sprite = 
            Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");

        InventorySlot newItemObjComponent = newObject.GetComponent<InventorySlot>();
        newItemObjComponent.SetValue(inventoryItem);

        slots.Add(newItemObjComponent);

        ItemAbility.Items[item.itemNumber].Use();

        // 스택형 아이템 표기
        if (ItemAbility.Items[item.itemNumber].isStackItem)
        {
            Item uiObjItem = item;
            GameObject uiObj = Managers.Resource.Instantiate("Assets/03.Prefabs/UI/UIItemSlot.prefab");
            UIInventorySlot uiObjComponent = uiObj.GetComponent<UIInventorySlot>();
            uiObjComponent.SetValue(uiObjItem);
            uiObj.SetActive(true);
            uiInventorySlotDic.Add(uiObjItem.itemNameEng, uiObjComponent);
            uiObj.transform.SetParent(uiItemSlotHolder);
        }

        if (ItemAbility.Items[inventoryItem.itemNumber].isSetElement)
            ItemManager.Instance.CheckSetItem(item);

        StartCoroutine(UIManager.Instance.ShowObtainItemInfo(item));
    }

    // 현재 획득한 아이템 목록 가져옴
    public void LoadItemSlot()
    {
        Dictionary<string, Item> itemDic = ItemManager.Instance.GetCurItemDic();

        if(itemSlotObjList.Count > 0)
        {
            foreach(var item in itemSlotObjList)
            {
                Managers.Pool.Push(item);
            }
        }

        foreach (var items in itemDic.Values)
        {
            if (items.itemNumber == 0)
                continue;

            Item inventoryItem = items;

            Poolable newObject = Managers.Pool.Pop(itemObjTemplate);
            itemSlotObjList.Add(newObject);

            newObject.transform.GetChild(0).GetComponent<Image>().sprite = 
                Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{inventoryItem.itemRating}/{inventoryItem.itemNameEng}.png");

            InventorySlot newItemObjComponent = newObject.GetComponent<InventorySlot>();
            newItemObjComponent.SetValue(inventoryItem);
            newObject.transform.SetParent(slotHolder);
            newObject.gameObject.SetActive(true);
            slots.Add(newItemObjComponent);

            // 스택형 아이템 표기
            if (ItemAbility.Items[items.itemNumber].isStackItem)
            {
                Item uiObjItem = items;
                GameObject uiObj = Managers.Resource.Instantiate("Assets/03.Prefabs/UI/UIItemSlot.prefab");
                UIInventorySlot uiObjComponent = uiObj.GetComponent<UIInventorySlot>();
                uiObjComponent.SetValue(uiObjItem);
                uiObj.SetActive(true);
                uiInventorySlotDic.Add(uiObjItem.itemNameEng, uiObjComponent);
                uiObj.transform.SetParent(uiItemSlotHolder);
            }

            if (ItemAbility.Items[inventoryItem.itemNumber].isPersitantItem)
                ItemAbility.Items[inventoryItem.itemNumber].LastingEffect();
            if (ItemAbility.Items[inventoryItem.itemNumber].isSetElement)
                ItemManager.Instance.CheckSetItem(items);
        }
    }
}
