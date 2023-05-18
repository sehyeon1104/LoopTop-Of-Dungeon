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
        //ShuffleItemSelectNum();
    }
    

    public void ShuffleItemSelectNum()
    {
        if(itemObjSpawnPos.Length - 1 > GameManager.Instance.allItemList.Count)
        {
            Debug.LogWarning($"아이템 개수 부족. 최소 개수 : {itemObjSpawnPos.Length - 1}");
            return;
        }

        int index = 0;
        int randNum = 0;

        List<Item> allItemList = new List<Item>();
        allItemList = GameManager.Instance.allItemList;

        while (itemSelectNum.Count != itemObjSpawnPos.Length - 1)
        {
            randNum = Random.Range(1, allItemList.Count);

            if (allItemList.Contains(allItemList[randNum])) 
                continue;

            itemSelectNum.Add(randNum);
            index++;

            if(index > 100)
            {
                Debug.Log("break while loop");
                break;
            }
        }

        CreateObject();
    }
    
    public void CreateObject()
    {
        GameObject newObject = null;
        ItemObj newItemObjComponent = null;

        List<Item> curItemList = new List<Item>(); 
        curItemList = GameManager.Instance.GetItemList();

        itemSelectNum.Clear();
        for(int i = 0; i < curItemList.Count; ++i)
        {
            itemSelectNum.Add(curItemList[i].itemNumber);
        }

        int index = 0;
        int loopCount = 0;
        int rand = 0;

        List<Item> allItemList = new List<Item>();
        allItemList = GameManager.Instance.allItemList;

        while (index < 4)
        {
            rand = Random.Range(1, allItemList.Count);

            if (itemSelectNum.Contains(rand))
                continue;

            itemSelectNum.Add(rand);

            // 아이템 오브젝트 생성
            Item shopItem = GameManager.Instance.allItemList[rand];

            newObject = Instantiate(itemObjTemplate);
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
                    Item defaultItem = GameManager.Instance.allItemList[0];

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
