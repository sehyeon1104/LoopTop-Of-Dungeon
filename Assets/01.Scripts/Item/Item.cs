using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public Define.ItemType itemType;        // 아이템 타입
    public Define.ItemRating itemRating;    // 아이템 등급
    public int itemNumber;                  // 아이템 번호
    public string itemName;                 // 아이템명
    public string itemNameEng;              // 아이템 영문명
    public string itemEffectDescription;    // 아이템 효과 설명
    public string itemDescription;          // 아이템 설명
    public long price;                      // 아이템 가격
}
