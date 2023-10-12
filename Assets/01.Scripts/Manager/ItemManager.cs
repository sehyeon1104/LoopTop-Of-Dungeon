using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SetItemList;

public class ItemManager : MonoSingleton<ItemManager>
{
    [Tooltip("아이템 추가")]
    [SerializeField]
    private List<Item> allItemInfo = new List<Item>(); // 저장용

    // 아이템명(영어), 아이템
    [field:SerializeField]
    public Dictionary<string, Item> allItemDic { get; private set; } = new Dictionary<string, Item>();
    public Dictionary<int, string> allItemFromNumberDic { get; private set; } = new Dictionary<int, string>();
    public Dictionary<string, Item> curItemDic { get; private set; } = new Dictionary<string, Item>();

    public List<Item> commonItemList { get; private set; } = new List<Item>();
    public List<Item> rareItemList { get; private set; } = new List<Item>();
    public List<Item> epicItemList { get; private set; } = new List<Item>();
    public List<Item> legendaryItemList { get; private set; } = new List<Item>();
    public List<Item> brokenItemList { get; private set; } = new List<Item>();
    public List<Item> setItemList { get; private set; } = new List<Item>();
    public List<Item> etcItemList { get; private set; } = new List<Item>();

    [field: SerializeField]
    public int brokenItemCount { get; private set; } = 0;

    // 해당 세트 아이템, 세트아이템이 되기 위해 필요한 아이템들
    private Dictionary<int, List<int>> setItemDic = new Dictionary<int, List<int>>();
    private Dictionary<int, List<int>> SetItemPartsListDictionary = new Dictionary<int, List<int>>();
    private bool isHaveParts = false;
    public List<int> exceptionItemNumberList { get; private set; } = new List<int>();

    private ItemAbility itemAbility = new ItemAbility();

    public UnityEvent RoomClearRelatedItemEffects { private set; get; } = new UnityEvent();
    public UnityEvent<Vector3> FragmentDropRelatedItemEffects { private set; get; } = new UnityEvent<Vector3>();

    public void Init()
    {
        // 딕셔너리 초기화
        InitDic();
        InitList();

        // 모든 아이템 스크립트 생성
        itemAbility.CreateItem();
        // 아이템 타입 분류
        SortItemLists();

        brokenItemCount = brokenItemList.Count;
    }

    private void InitDic()
    {
        // AllItemDic에 모든 아이템 정보 추가
        SetAllItemDic(allItemInfo);

        SetItemPartsListDictionary.Add(601, CompleteHourglassParts);
        SetItemPartsListDictionary.Add(602, MirrorOfDawnParts);
        SetItemPartsListDictionary.Add(603, FlexodiaParts);
        SetItemPartsListDictionary.Add(604, OvereagerParts);
        // SetItemPartsListDictionary.Add(605, GamblersLegacyParts);

        setItemDic.Add(601, CompleteHourglassParts.ToList());
        setItemDic.Add(602, MirrorOfDawnParts.ToList());
        setItemDic.Add(603, FlexodiaParts.ToList());
        setItemDic.Add(604, OvereagerParts.ToList());
        // setItemDic.Add(605, GamblersLegacyParts.ToList());
    }

    private void InitList()
    {
        foreach (var setItemNum in setItemDic.Keys)
        {
            if (curItemDic.ContainsKey(allItemFromNumberDic[setItemNum]))
            {
                for (int i = 0; i < SetItemPartsListDictionary[setItemNum].Count; ++i)
                {
                    exceptionItemNumberList.Add(SetItemPartsListDictionary[setItemNum][i]);
                }
            }
        }
    }

    public void SortItemLists()
    {
        foreach(Item item in allItemDic.Values)
        {
            switch (item.itemRating)
            {
                case Define.ItemRating.Common:
                    commonItemList.Add(item);
                    break;
                case Define.ItemRating.Rare:
                    rareItemList.Add(item);
                    break;
                case Define.ItemRating.Epic:
                    epicItemList.Add(item);
                    break;
                case Define.ItemRating.Legendary:
                    legendaryItemList.Add(item);
                    break;
                case Define.ItemRating.ETC:
                    etcItemList.Add(item);
                    break;   
                case Define.ItemRating.Special:
                    brokenItemList.Add(item);
                    break;
                case Define.ItemRating.Set:
                    setItemList.Add(item);
                    break;
            }
        }
    }

    public void SetAllItemDic(List<Item> itemList)
    {
        foreach(Item item in itemList)
        {
            if(!allItemDic.ContainsKey(item.itemNameEng))
                allItemDic.Add(item.itemNameEng, item);
            if (!allItemFromNumberDic.ContainsKey(item.itemNumber))
                allItemFromNumberDic.Add(item.itemNumber, item.itemNameEng);
        }
    }

    public void SetCurItemDic(List<Item> itemList)
    {
        foreach (Item item in itemList)
        {
            curItemDic.Add(item.itemNameEng, item);
        }
    }

    public Dictionary<string, Item> GetCurItemDic()
    {
        return curItemDic;
    }

    public void AddCurItemDic(Item item)
    {
        curItemDic.Add(item.itemNameEng, item);
        GameManager.Instance.SaveItemData();
    }

    public void RemoveCurItemDic(Item item)
    {
        Debug.Log($"{item.itemName} 제거");
        if(curItemDic.ContainsKey(item.itemNameEng))
            curItemDic.Remove(item.itemNameEng);
    }

    /// <summary>
    /// 아이템 제거 코드. 제거하려는 아이템을 넣으면 됨
    /// </summary>
    /// <param name="item"></param>
    public void DisablingItem(Item item)
    {
        ItemAbility.Items[item.itemNumber].Disabling();
        RemoveCurItemDic(item);
        InventoryUI.Instance.LoadItemSlot();
    }

    public void DisablingItemWithoutLoad(Item item)
    {
        ItemAbility.Items[item.itemNumber].Disabling();
        RemoveCurItemDic(item);
    }

    public void InitItems()
    {
        foreach(var item in allItemDic.Values)
        {
            ItemAbility.Items[item.itemNumber].Init();
        }
    }

    public void CheckSetItem(Item item)
    {
        foreach(var itemParts in setItemDic.Values)
        {
            if (itemParts.Contains(item.itemNumber))
            {
                itemParts.Remove(item.itemNumber);
                isHaveParts = true;
            }
        }

        if (!isHaveParts)
            return;

        foreach (var setItemNum in setItemDic.Keys)
        {
            if (setItemDic[setItemNum].Count == 0 && !curItemDic.ContainsKey(allItemFromNumberDic[setItemNum]))
            {
                for (int i = 0; i < SetItemPartsListDictionary[setItemNum].Count; ++i)
                {
                    if (curItemDic.ContainsKey(allItemFromNumberDic[SetItemPartsListDictionary[setItemNum][i]]))
                    {
                        DisablingItemWithoutLoad(curItemDic[allItemFromNumberDic[SetItemPartsListDictionary[setItemNum][i]]]);
                        exceptionItemNumberList.Add(SetItemPartsListDictionary[setItemNum][i]);
                    }
                }

                InventoryUI.Instance.AddItemSlot(allItemDic[allItemFromNumberDic[setItemNum]]);
                InventoryUI.Instance.LoadItemSlot();
            }
        }
        isHaveParts = false;
    }
}