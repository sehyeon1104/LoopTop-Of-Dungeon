using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = Rito.Debug;

public class ShopManager : MonoSingleton<ShopManager>
{
    [Tooltip("���� �������� ��ġ�� ��ġ")]
    [SerializeField]
    private Transform[] itemObjSpawnPos;

    [Tooltip("������ �������� ������Ʈ")]
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

        ShuffleItemSelectNum();
    }
    

    public void ShuffleItemSelectNum()
    {
        if(itemObjSpawnPos.Length - 1 > GameManager.Instance.allItemList.Count)
        {
            Debug.LogWarning($"������ ���� ����. �ּ� ���� : {itemObjSpawnPos.Length - 1}");
            return;
        }

        int index = 0;
        int randNum = 0;

        while (itemSelectNum.Count != itemObjSpawnPos.Length - 1)
        {
            randNum = Random.Range(1, GameManager.Instance.allItemList.Count);

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

        //for(int i = 0; i < itemObjSpawnPos.Length; ++i)
        //{
        foreach(var itemSelectNumitem in itemSelectNum)
        {
            Item shopItem = GameManager.Instance.allItemList[itemSelectNumitem];

            newObject = Instantiate(itemObjTemplate);
            newItemObjComponent = newObject.GetComponent<ItemObj>();
            newItemObjComponent.SetValue(shopItem);
            newObject.SetActive(true);
            itemObjList.Add(newItemObjComponent);
        }

        shopRoom.SetItemObjList(itemObjList);
        //}
    }

    // ������ ���� �Լ�
    public void InteractiveToItem()
    {
        if(shopRoom.GetPurchaseableItem() == null)
        {
            return;
        }

        shopRoom.GetPurchaseableItem().PurchaseShopItem();
    }

}
