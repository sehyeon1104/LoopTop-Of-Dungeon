using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public delegate void OnSlotCountChange(int cnt);
    public OnSlotCountChange onSlotCountChange;

    private int inventorySlotCount;
    public int InventorySlotCount
    {
        get => inventorySlotCount;
        set
        {
            inventorySlotCount = value;
            onSlotCountChange(inventorySlotCount);
        }
    }


}
