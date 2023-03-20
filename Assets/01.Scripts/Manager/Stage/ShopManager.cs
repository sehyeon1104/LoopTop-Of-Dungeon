using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    private List<ItemObj> itemObjList = new List<ItemObj>();

    private HashSet<int> itemSelectNum = new HashSet<int>();

    private ShopRoom shopRoom = null;

    private void Awake()
    {
        shopRoom = FindObjectOfType<ShopRoom>();
    }

    private void Start()
    {
        ShuffleItemSelectNum();
        CreateObject();
    }

    public void ShuffleItemSelectNum()
    {
        if(itemObjSpawnPos.Length > itemList.Count)
        {
            Debug.LogWarning($"아이템 개수 부족. 최소 개수 : {itemObjSpawnPos.Length}");
        }

        while (itemSelectNum.Count != itemObjSpawnPos.Length)
        {
            itemSelectNum.Add(Random.Range(0, itemList.Count));
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
            Item shopItem = itemList[itemSelectNumitem];

            newObject = Instantiate(itemObjTemplate);
            newItemObjComponent = newObject.GetComponent<ItemObj>();
            newItemObjComponent.SetValue(shopItem);
            newObject.SetActive(true);
            itemObjList.Add(newItemObjComponent);
        }
        //}
    }

}
