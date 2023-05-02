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
        itemNameTmp.SetText(item.itemName.ToString());
        itemDesTmp.SetText(item.itemDescription.ToString());
    }


}
