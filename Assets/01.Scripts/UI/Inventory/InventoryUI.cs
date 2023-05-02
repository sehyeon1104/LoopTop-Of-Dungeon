using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryPanel;
    bool activeInventory = false;

    public InventorySlot[] slots;
    [SerializeField]
    private Transform slotHolder;

    private void Start()
    {
        slots = slotHolder.GetComponentsInChildren<InventorySlot>();
        Inventory.Instance.onSlotCountChange += SlotChange;
    }

    private void SlotChange(int cnt)
    {
        for(int i = 0; i < slots.Length; ++i)
        {
            //if(i < Inventory.Instance.InventorySlotCount)
        }
    }

}
