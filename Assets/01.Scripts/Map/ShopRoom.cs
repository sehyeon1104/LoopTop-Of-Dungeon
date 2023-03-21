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

    private List<ItemObj> itemList = new List<ItemObj>();

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.Shop;
    }

    public void SetItemObjList(List<ItemObj> lists)
    {
        itemList.Clear();
        itemList = lists;

        SetItem();
    }

    public void SetItem()
    {
        itemSpawnPosArr = itemPosObj.GetComponentsInChildren<Transform>();

        for(int i = 0; i < itemSpawnPosArr.Length; ++i)
        {
            itemList[i].transform.position = itemSpawnPosArr[i + 1].position;
        }

    }



    protected override void IsClear()
    {
        isClear = true;
    }

}
