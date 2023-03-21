using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoom : RoomBase
{
    [SerializeField]
    private GameObject itemPosObj;
    [SerializeField]
    private Transform[] itemSpawnPosArr;

    private List<ItemObj> itemList = new List<ItemObj>();

    private ItemObj[] itemobjArr;

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.Shop;
    }

    private void Awake()
    {
        itemSpawnPosArr = itemPosObj.GetComponentsInChildren<Transform>();
    }

    public void SetItemObjList(List<ItemObj> lists)
    {
        itemList.Clear();
        itemList = lists;

        itemobjArr = lists.ToArray();
        SetItemPos();
    }

    public void SetItemPos()
    {
        for(int i = 0; i < itemSpawnPosArr.Length - 1; ++i)
        {
            itemList[i].transform.position = itemSpawnPosArr[i + 1].position;
        }

    }

    public Transform[] GetItemSpawnPos()
    {
        foreach(var itemSpawnPos in itemSpawnPosArr)
        {
             Debug.Log("return pos");
        }
        return itemSpawnPosArr;
    }



    protected override void IsClear()
    {
        isClear = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!ShopManager.Instance.isItemSetting)
            {
                ShopManager.Instance.SetItem();
            }

            foreach (var itemobj in itemobjArr)
            {
                StartCoroutine(itemobj.ToggleItemInfoPanel());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(var itemobj in itemobjArr)
            {
                StopCoroutine(itemobj.ToggleItemInfoPanel());
            }
        }
    }
}
