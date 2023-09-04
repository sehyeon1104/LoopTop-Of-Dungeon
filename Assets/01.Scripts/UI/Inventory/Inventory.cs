using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Inventory : MonoSingleton<Inventory>
{
    [SerializeField]
    private TextMeshProUGUI itemNameTmp = null;
    [SerializeField]
    private TextMeshProUGUI itemEffDesTmp = null;
    [SerializeField]
    private TextMeshProUGUI itemDesTmp = null;

    public void ClearText()
    {
        itemNameTmp.text = string.Empty;
        itemEffDesTmp.text = string.Empty;
        itemDesTmp.text = string.Empty;
    }

    public void ShowItemInfo(Item item)
    {
        itemNameTmp.text = $"<color={GameManager.Instance.itemRateColor[(int)item.itemRating]}>{item.itemName}</color>";
        itemEffDesTmp.SetText(item.itemEffectDescription.ToString());
        itemDesTmp.SetText(item.itemDescription.ToString());
    }


}
