using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    public void InitItems()
    {
        for(int i = 1; i < ItemEffects.Items.Length; ++i)
        {
            ItemEffects.Items[i].Init();
        }
    }
}
