using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public Define.ItemType itemType;
    public Define.ItemRating itemRating;
    public int itemNumber;
    public string itemName;
    public string itemNameEng;
    public string itemDescription;
    public long price;
}
