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

    private void Awake()
    {
        if(itemObjTemplate == null)
        {
            Debug.Log("itemObjTemplate is null!");
            itemObjTemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/ItemSlot.prefab");
        }
        inventoryPanel = transform.Find("Background").gameObject;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Debug.Log("아이템 로딩");
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
        inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
    }

    // 아이템 획득시 슬롯에 추가
    public void AddItemSlot(Item item)
    {
        GameManager.Instance.AddItemData(item);

        GameObject newObject = null;
        InventorySlot newItemObjComponent = null;

        Item inventoryItem = item;

        newObject = Instantiate(itemObjTemplate);
        newObject.transform.GetChild(0).GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
        newItemObjComponent = newObject.GetComponent<InventorySlot>();
        newItemObjComponent.SetValue(inventoryItem);
        newObject.transform.SetParent(slotHolder);
        newObject.SetActive(true);
        slots.Add(newItemObjComponent);
        ItemEffects.Items[item.itemNumber].Use();

        UIManager.Instance.AddItemListUI(item);
    }

    // 현재 획득한 아이템 목록 가져옴
    public void LoadItemSlot()
    {
        List<Item> itemList = GameManager.Instance.GetItemList();

        GameObject newObject = null;
        InventorySlot newItemObjComponent = null;

        foreach(var items in itemList)
        {
            if (items.itemNumber == 0)
                continue;

            Item inventoryItem = items;

            newObject = Instantiate(itemObjTemplate);   // 여기서 오류남. 아마 템플릿이 로딩이 안된듯?

            newObject.transform.GetChild(0).GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{inventoryItem.itemRating}/{inventoryItem.itemNameEng}.png");
            newItemObjComponent = newObject.GetComponent<InventorySlot>();
            newItemObjComponent.SetValue(inventoryItem);
            newObject.transform.SetParent(slotHolder);
            newObject.SetActive(true);
            slots.Add(newItemObjComponent);
            if (ItemEffects.Items[inventoryItem.itemNumber].isPersitantItem)
            {
                ItemEffects.Items[inventoryItem.itemNumber].Use();
            }

            UIManager.Instance.AddItemListUI(items);
        }
    }

}
