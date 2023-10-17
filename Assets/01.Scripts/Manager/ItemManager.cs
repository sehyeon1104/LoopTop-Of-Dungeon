using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SetItemList;

public class ItemManager : MonoSingleton<ItemManager>
{
    [Tooltip("������ �߰�")]
    [SerializeField]
    private List<Item> allItemInfo = new List<Item>(); // �����

    // �����۸�(����), ������
    [field:SerializeField]
    public Dictionary<string, Item> allItemDict { get; private set; } = new Dictionary<string, Item>();
    public Dictionary<int, string> allItemFromNumberDict { get; private set; } = new Dictionary<int, string>();
    public Dictionary<string, Item> curItemDict { get; private set; } = new Dictionary<string, Item>();

    public List<Item> commonItemList { get; private set; } = new List<Item>();
    public List<Item> rareItemList { get; private set; } = new List<Item>();
    public List<Item> epicItemList { get; private set; } = new List<Item>();
    public List<Item> legendaryItemList { get; private set; } = new List<Item>();
    public List<Item> brokenItemList { get; private set; } = new List<Item>();
    public List<Item> setItemList { get; private set; } = new List<Item>();
    public List<Item> etcItemList { get; private set; } = new List<Item>();

    [field: SerializeField]
    public int brokenItemCount { get; private set; } = 0;

    // �ش� ��Ʈ ������, ��Ʈ�������� �Ǳ� ���� �ʿ��� �����۵�
    private Dictionary<int, List<int>> setItemDict = new Dictionary<int, List<int>>();
    private Dictionary<int, List<int>> SetItemPartsListDictionary = new Dictionary<int, List<int>>();
    private bool isHaveParts = false;
    public List<int> exceptionItemNumberList { get; private set; } = new List<int>();

    private ItemAbility itemAbility = new ItemAbility();

    public UnityEvent RoomClearRelatedItemEffects { private set; get; } = new UnityEvent();
    public UnityEvent<Vector3> FragmentDropRelatedItemEffects { private set; get; } = new UnityEvent<Vector3>();

    public void Init()
    {
        // ��ųʸ� �ʱ�ȭ
        InitDic();
        InitList();

        // ��� ������ ��ũ��Ʈ ����
        itemAbility.CreateItem();
        // ������ Ÿ�� �з�
        SortItemLists();

        brokenItemCount = brokenItemList.Count;
    }

    private void InitDic()
    {
        // AllItemDic�� ��� ������ ���� �߰�
        SetAllItemDic(allItemInfo);

        SetItemPartsListDictionary.Add(601, CompleteHourglassParts);
        SetItemPartsListDictionary.Add(602, MirrorOfDawnParts);
        SetItemPartsListDictionary.Add(603, FlexodiaParts);
        SetItemPartsListDictionary.Add(604, OvereagerParts);
        // SetItemPartsListDictionary.Add(605, GamblersLegacyParts);

        setItemDict.Add(601, CompleteHourglassParts.ToList());
        setItemDict.Add(602, MirrorOfDawnParts.ToList());
        setItemDict.Add(603, FlexodiaParts.ToList());
        setItemDict.Add(604, OvereagerParts.ToList());
        // setItemDic.Add(605, GamblersLegacyParts.ToList());
    }

    private void InitList()
    {
        foreach (var setItemNum in setItemDict.Keys)
        {
            if (curItemDict.ContainsKey(allItemFromNumberDict[setItemNum]))
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
        foreach(Item item in allItemDict.Values)
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
            if(!allItemDict.ContainsKey(item.itemNameEng))
                allItemDict.Add(item.itemNameEng, item);
            if (!allItemFromNumberDict.ContainsKey(item.itemNumber))
                allItemFromNumberDict.Add(item.itemNumber, item.itemNameEng);
        }
    }

    public void SetCurItemDic(List<Item> itemList)
    {
        foreach (Item item in itemList)
        {
            curItemDict.Add(item.itemNameEng, item);
        }
    }

    public Dictionary<string, Item> GetCurItemDic()
    {
        return curItemDict;
    }

    public void AddCurItemDict(Item item)
    {
        curItemDict.Add(item.itemNameEng, item);
        GameManager.Instance.SaveItemData();
    }

    public void RemoveCurItemDict(Item item)
    {
        Debug.Log($"{item.itemName} ����");
        if(curItemDict.ContainsKey(item.itemNameEng))
            curItemDict.Remove(item.itemNameEng);
    }

    /// <summary>
    /// ������ ���� �ڵ�. �����Ϸ��� �������� ������ ��
    /// </summary>
    /// <param name="item"></param>
    public void DisablingItem(Item item)
    {
        ItemAbility.Items[item.itemNumber].Disabling();
        RemoveCurItemDict(item);
        InventoryUI.Instance.LoadItemSlot();
    }

    public void DisablingItemWithoutLoad(Item item)
    {
        ItemAbility.Items[item.itemNumber].Disabling();
        RemoveCurItemDict(item);
    }

    public void InitItems()
    {
        foreach(var item in allItemDict.Values)
        {
            ItemAbility.Items[item.itemNumber].Init();
        }
    }

    public void CheckSetItem(Item item)
    {
        foreach(var itemParts in setItemDict.Values)
        {
            if (itemParts.Contains(item.itemNumber))
            {
                itemParts.Remove(item.itemNumber);
                isHaveParts = true;
            }
        }

        if (!isHaveParts)
            return;

        foreach (var setItemNum in setItemDict.Keys)
        {
            if (setItemDict[setItemNum].Count == 0 && !curItemDict.ContainsKey(allItemFromNumberDict[setItemNum]))
            {
                for (int i = 0; i < SetItemPartsListDictionary[setItemNum].Count; ++i)
                {
                    if (curItemDict.ContainsKey(allItemFromNumberDict[SetItemPartsListDictionary[setItemNum][i]]))
                    {
                        DisablingItemWithoutLoad(curItemDict[allItemFromNumberDict[SetItemPartsListDictionary[setItemNum][i]]]);
                        exceptionItemNumberList.Add(SetItemPartsListDictionary[setItemNum][i]);
                    }
                }

                InventoryUI.Instance.AddItemSlot(allItemDict[allItemFromNumberDict[setItemNum]]);
                InventoryUI.Instance.LoadItemSlot();
            }
        }
        isHaveParts = false;
    }
}