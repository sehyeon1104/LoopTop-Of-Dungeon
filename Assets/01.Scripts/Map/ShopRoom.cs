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
    private float playerSensingDis = 1.5f;
    private ItemObj[] itemobjArr;
    int itemobjCount = 0;
    private WaitForEndOfFrame waitForEndOfFrame;
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
        for (int i = 0; i < itemSpawnPosArr.Length - 1; ++i)
        {
            itemList[i].transform.position = itemSpawnPosArr[i + 1].position;
        }

    }

    public Transform[] GetItemSpawnPos()
    {
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
            StartCoroutine(ToggleItemInfoPanel());
        }
    }
    public IEnumerator ToggleItemInfoPanel()
    {
        while (true)
        {
            for (int i = 0; i < itemobjArr.Length; ++i)
            {
                if (Vector3.SqrMagnitude(GameManager.Instance.Player.transform.position - itemobjArr[i].transform.position) < playerSensingDis * playerSensingDis)
                {   
                    itemobjCount++;
                    if (!itemobjArr[i].ItemInfoPanel.gameObject.activeSelf)
                    {
                        itemobjArr[i].ItemInfoPanel.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (itemobjArr[i].ItemInfoPanel.gameObject.activeSelf)
                    {
                        itemobjArr[i].ItemInfoPanel.gameObject.SetActive(false);
                    }
                }
            }
            if (itemobjCount > 0)
                UIManager.Instance.RotateInteractionButton();
            else
                UIManager.Instance.RotateAttackButton();

            itemobjCount = 0;
            yield return waitForEndOfFrame;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var itemobj in itemobjArr)
            {
                itemobj.Num = 0;
                StopCoroutine(ToggleItemInfoPanel());
            }
        }
    }
}
