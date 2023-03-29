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
    [Tooltip("아이템 추가")]
    [SerializeField]
    private List<Item> itemList = new List<Item>();

    [field: SerializeField]
    public Sprite[] itemSprites { private set; get; } = null;

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
            }
        }
        isItemSetting = true;
        itemObjSpawnPos = shopRoom.GetItemSpawnPos();

        ShuffleItemSelectNum();
        CreateObject();
    }
    

    public void ShuffleItemSelectNum()
    {
        if(itemObjSpawnPos.Length - 1 > itemList.Count)
        {
            Debug.LogWarning($"아이템 개수 부족. 최소 개수 : {itemObjSpawnPos.Length - 1}");
        }

        int index = 0;
        int randNum = 0;

        while (itemSelectNum.Count != itemObjSpawnPos.Length - 1)
        {
            randNum = Random.Range(1, itemList.Count);

            itemSelectNum.Add(randNum);
            index++;

            if(index > 100)
            {
                Debug.Log("break while loop");
                break;
            }
        }

        foreach(var itemselectnumtime in itemSelectNum)
        {
            Debug.Log("num : " + itemselectnumtime);
        }
    }
    
    public void CreateObject()
    {
        GameObject newObject = null;
        ItemObj newItemObjComponent = null;

        //for(int i = 0; i < itemObjSpawnPos.Length; ++i)
        //{
        foreach(var itemSelectNumitem in itemSelectNum)
        {
            Debug.Log(itemSelectNumitem);
            Item shopItem = itemList[itemSelectNumitem];

            newObject = Instantiate(itemObjTemplate);
            newItemObjComponent = newObject.GetComponent<ItemObj>();
            newItemObjComponent.SetValue(shopItem);
            newObject.SetActive(true);
            itemObjList.Add(newItemObjComponent);
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
