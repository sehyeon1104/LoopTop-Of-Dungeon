using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [Tooltip("아이템 추가")]
    public List<Item> allItemList { get; private set; } = new List<Item>();

    [field: SerializeField]
    public List<Item> cummonItemList { get; private set; } = new List<Item>();
    [field: SerializeField]
    public List<Item> rareItemList { get; private set; } = new List<Item>();
    [field: SerializeField]
    public List<Item> epicItemList { get; private set; } = new List<Item>();
    [field: SerializeField]
    public List<Item> legendaryItemList { get; private set; } = new List<Item>();
    [field: SerializeField]
    public List<Item> brokenItemList { get; private set; } = new List<Item>();
    [field: SerializeField]
    public List<Item> setItemList { get; private set; } = new List<Item>();

    [field: SerializeField]
    public int brokenItemCount { get; private set; } = 0;

    public void InitItems()
    {
        for(int i = 1; i < ItemAbility.Items.Length; ++i)
        {
            ItemAbility.Items[i].Init();
        }
    }
}
