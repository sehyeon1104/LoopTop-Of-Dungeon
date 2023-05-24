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
    private TextMeshProUGUI itemDesTmp = null;


    public void ShowItemInfo(Item item)
    {
        itemNameTmp.text = $"<color={GameManager.Instance.itemRateColor[(int)item.itemRating]}>{item.itemName}</color>";
        itemDesTmp.SetText(item.itemDescription.ToString());
    }


}
