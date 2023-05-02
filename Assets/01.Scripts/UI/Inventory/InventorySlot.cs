using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image itemInfoPanel = null;
    public Image ItemInfoPanel => itemInfoPanel;
    [SerializeField]
    private Image itemImage = null;
    [SerializeField]
    private Define.ItemType itemType;

    private Item item = null;

    public void SetValue(Item item)
    {
        this.item = item;
        UpdateValues();
    }

    public void UpdateValues()
    {
        itemType = item.itemType;
        //itemImage.sprite = ShopManager.Instance.itemSprites[item.itemNumber];
    }

    public void ClickSlot()
    {
        Inventory.Instance.ShowItemInfo(item);
    }
}
