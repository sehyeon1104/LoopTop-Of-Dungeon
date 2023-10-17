using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = Rito.Debug;

public class ShopManager : MonoSingleton<ShopManager>
{
    [Tooltip("상점 아이템이 배치될 위치")]
    [SerializeField]
    private Transform[] itemObjSpawnPos;

    [Tooltip("복제할 오리지널 오브젝트")]
    [SerializeField]
    private GameObject itemObjTemplate = null;

    private List<ItemObj> itemObjList = new List<ItemObj>();

    private HashSet<int> itemSelectNum = new HashSet<int>();

    private ShopRoom shopRoom = null;

    public bool isItemSetting { private set; get; } = false;

    private void Awake()
    {
        shopRoom = FindObjectOfType<ShopRoom>();
        itemObjTemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Test/ItemObj.prefab");
    }

    public void SetItem()
    {
        if(shopRoom == null)
        {
            shopRoom = FindObjectOfType<ShopRoom>();
            if(shopRoom == null)
            {
                Debug.LogWarning("shopRoom is Null!");
                return;
            }
        }
        isItemSetting = true;
        itemObjSpawnPos = shopRoom.GetItemSpawnPos();

        CreateObject();
    }
    
    public void CreateObject()
    {
        GameObject newObject = null;
        ItemObj newItemObjComponent = null;

        Dictionary<string, Item> curItemDic = ItemManager.Instance.GetCurItemDic();

        List<Item> allItemList = ItemManager.Instance.allItemDict.Values.ToList();

        itemSelectNum.Clear();
        foreach(Item item in curItemDic.Values)
        {
            itemSelectNum.Add(item.itemNumber);
        }

        int index = 0;
        int loopCount = 0;
        int rand = 0;

        Dictionary<string, Item> allItemDic = ItemManager.Instance.allItemDict;

        while (index < 4)
        {
            rand = Random.Range(1, ItemManager.Instance.allItemDict.Count);

            if (itemSelectNum.Contains(allItemList[rand].itemNumber) 
                || allItemList[rand].itemRating == Define.ItemRating.Special 
                || allItemList[rand].itemRating == Define.ItemRating.Set)
                continue;

            itemSelectNum.Add(allItemList[rand].itemNumber);

            Item shopItem = null;
            // 아이템 오브젝트 생성


            foreach(Item item in ItemManager.Instance.allItemDict.Values)
            {
                if (item.itemNumber == allItemList[rand].itemNumber)
                {
                    shopItem = item;
                    break;
                }
            }

            newObject = Instantiate(itemObjTemplate, shopRoom.transform);
            newItemObjComponent = newObject.GetComponent<ItemObj>();
            newItemObjComponent.SetValue(shopItem);
            newObject.SetActive(true);
            itemObjList.Add(newItemObjComponent);

            index++;
            loopCount++;

            if (loopCount > 100)
            {
                Debug.Log("break while loop");
                for(int i = index; i < 4; ++i)
                {
                    Item defaultItem = ItemManager.Instance.allItemDict["Default"];

                    newObject = Instantiate(itemObjTemplate);
                    newItemObjComponent = newObject.GetComponent<ItemObj>();
                    newItemObjComponent.SetValue(defaultItem);
                    newObject.SetActive(true);
                    itemObjList.Add(newItemObjComponent);
                }
                break;
            }
        }

        shopRoom.SetItemObjList(itemObjList);
        //}
    }

    // 아이템 구매 함수
    public void InteractiveToItem()
    {
        if(shopRoom.GetPurchaseableItem() == null)
        {
            return;
        }

        shopRoom.GetPurchaseableItem().PurchaseShopItem();
    }

}
