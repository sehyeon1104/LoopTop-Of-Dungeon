using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemObj : MonoBehaviour
{
    [SerializeField]
    private Image itemImage = null;
    [SerializeField]
    private TextMeshProUGUI itemNameTMP = null;
    [SerializeField]
    private TextMeshProUGUI itemDesTMP = null;
    [SerializeField]
    private TextMeshProUGUI priceTMP = null;
    [SerializeField]
    private Sprite[] itemSprites = null;
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
        itemImage.sprite = itemSprites[item.itemNumber];
        itemNameTMP.SetText(item.itemName);
        itemDesTMP.SetText(item.itemDescription);
        priceTMP.SetText(string.Format("{0}", item.price));
    }
}
