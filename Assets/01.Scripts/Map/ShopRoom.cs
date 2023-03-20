using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoom : RoomBase
{
    [SerializeField]
    private GameObject itemPosObj;
    [SerializeField]
    private Transform[] itemSpawnPosArr;
    [SerializeField]
    private GameObject itemObj;

    private List<GameObject> itemList = new List<GameObject>();

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.Shop;
    }

    private void Start()
    {
        CreateItems();
        SetItem();
    }

    public void CreateItems()
    {
        itemSpawnPosArr = itemPosObj.GetComponentsInChildren<Transform>();
    }

    public void SetItem()
    {
        for(int i = 1; i < itemSpawnPosArr.Length; ++i)
        {
            itemObj.transform.position = itemSpawnPosArr[i].position;
        }

    }


    protected override void IsClear()
    {
        isClear = true;
    }

}
