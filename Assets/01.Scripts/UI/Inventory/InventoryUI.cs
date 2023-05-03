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

    private GameObject itemObjTemplate;

    private void Awake()
    {
        itemObjTemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/ItemSlot.prefab");
        inventoryPanel = transform.Find("Background").gameObject;
    }

    private void Start()
    {
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
        inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
    }

    // æ∆¿Ã≈€ »πµÊΩ√ ΩΩ∑‘ø° √ﬂ∞°
    public void AddItemSlot(Item item)
    {
        GameManager.Instance.SetItemData(item);

        GameObject newObject = null;
        InventorySlot newItemObjComponent = null;

        Item inventoryItem = item;

        newObject = Instantiate(itemObjTemplate);
        newItemObjComponent = newObject.GetComponent<InventorySlot>();
        newItemObjComponent.SetValue(inventoryItem);
        newObject.transform.SetParent(slotHolder);
        newObject.SetActive(true);
        slots.Add(newItemObjComponent);
        ItemEffects.Items[item.itemNumber].Use();
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
            newItemObjComponent = newObject.GetComponent<InventorySlot>();
            newItemObjComponent.SetValue(items);
            newObject.transform.SetParent(slotHolder);
            newObject.SetActive(true);
            slots.Add(newItemObjComponent);
            if (ItemEffects.Items[items.itemNumber].isPersitantItem)
            {
                ItemEffects.Items[items.itemNumber].Use();
            }
        }
    }

    public void HPRelatedItemEffects()
    {
        // ±§¿¸ªÁ¿« ∞À
        ItemEffects.Items[10].Use();
    }

}
