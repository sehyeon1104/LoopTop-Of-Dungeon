using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO : Json파일로 저장 후 불러올 수 있도록
[Serializable]
public class ItemData
{
    public List<Item> itemsList = new List<Item>();
}
