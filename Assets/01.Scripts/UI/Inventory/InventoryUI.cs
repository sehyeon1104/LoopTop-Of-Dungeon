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
        Init();
    }

    private void Init()
    {
        Debug.Log("æ∆¿Ã≈€ ∑Œµ˘");
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
        inventoryPanel.SetActive(!inventoryPanel.gameObject.activeSelf);
        if (inventoryPanel.gameObject.activeSelf)
        {
            MouseManager.Lock(false);
            MouseManager.Show(true);
        }
        else
        {
            MouseManager.Lock(true);
            MouseManager.Show(false);
        }
    }

    // æ∆¿Ã≈€ »πµÊΩ√ ΩΩ∑‘ø° √ﬂ∞°
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
        ItemAbility.Items[item.itemNumber].Use();

        UIManager.Instance.AddItemListUI(item);
        StartCoroutine(UIManager.Instance.ShowObtainItemInfo(item));
    }

    // «ˆ¿Á »πµÊ«— æ∆¿Ã≈€ ∏Ò∑œ ∞°¡Æø»
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

            newObject = Instantiate(itemObjTemplate);

            newObject.transform.GetChild(0).GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{inventoryItem.itemRating}/{inventoryItem.itemNameEng}.png");
            newItemObjComponent = newObject.GetComponent<InventorySlot>();
            newItemObjComponent.SetValue(inventoryItem);
            newObject.transform.SetParent(slotHolder);
            newObject.SetActive(true);
            slots.Add(newItemObjComponent);
            if (ItemAbility.Items[inventoryItem.itemNumber].isPersitantItem)
            {
                ItemAbility.Items[inventoryItem.itemNumber].LastingEffect();
            }

            UIManager.Instance.AddItemListUI(items);
        }
    }

    public void RemoveItemSlot(Item item)
    {

    }

}
