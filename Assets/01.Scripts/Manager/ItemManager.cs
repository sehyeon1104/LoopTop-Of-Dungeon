using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    public void InitItems()
    {
        for(int i = 1; i < ItemBase.Items.Length; ++i)
        {
            ItemBase.Items[i].Init();
        }
    }
}
