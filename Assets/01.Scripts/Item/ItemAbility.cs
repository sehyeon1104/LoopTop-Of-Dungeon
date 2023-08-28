using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ItemAbility : MonoBehaviour
{
    public class Default : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.Default;
        public override Define.ItemRating itemRating => Define.ItemRating.Default;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {

        }

        public override void Disabling()
        {

        }

    }

    public static Dictionary<int, ItemBase> Items = new Dictionary<int, ItemBase>();
    //public static ItemBase[] Items = new ItemBase[800];

    public void CreateItem()
    {
        Type itemType = null;

        foreach(var item in ItemManager.Instance.allItemDic.Values)
        {
            if (Items.ContainsKey(item.itemNumber))
                continue;

            // Reflection을 사용하여 문자열로 클래스 인스턴스 생성
            itemType = Type.GetType(item.itemNameEng);

            if (itemType != null && itemType.IsSubclassOf(typeof(ItemBase)))
            {
                Items.Add(item.itemNumber, Activator.CreateInstance(itemType) as ItemBase);
            }
            else
            {
                Debug.Log($"{item.itemNumber} : {item.itemName}");
                Debug.LogError("Invalid item type!");
            }
        }
    }
}