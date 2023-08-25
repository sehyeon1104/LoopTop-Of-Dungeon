using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static SetItemList;

public class ItemManager : MonoSingleton<ItemManager>
{
    [Tooltip("������ �߰�")]
    [SerializeField]
    private List<Item> allItemInfo = new List<Item>(); // �����

    [field:SerializeField]
    public Dictionary<string, Item> allItemDic { get; private set; } = new Dictionary<string, Item>();
    public Dictionary<string, Item> curItemDic { get; private set; } = new Dictionary<string, Item>();

    public List<Item> commonItemList { get; private set; } = new List<Item>();
    public List<Item> rareItemList { get; private set; } = new List<Item>();
    public List<Item> epicItemList { get; private set; } = new List<Item>();
    public List<Item> legendaryItemList { get; private set; } = new List<Item>();
    public List<Item> brokenItemList { get; private set; } = new List<Item>();
    public List<Item> setItemList { get; private set; } = new List<Item>();

    [field: SerializeField]
    public int brokenItemCount { get; private set; } = 0;

    // �ش� ��Ʈ ������, ��Ʈ�������� �Ǳ� ���� �ʿ��� ������ ����
    private Dictionary<SetItem, int> setItemDic = new Dictionary<SetItem, int>();

    private ItemAbility itemAbility = new ItemAbility();

    public UnityEvent RoomClearRelatedItemEffects { private set; get; }

    public void Init()
    {
        itemAbility.CreateItem();

        InitDic();
        InitItemLists();

        brokenItemCount = brokenItemList.Count;
    }

    public void InitDic()
    {
        SetAllItemDic(allItemInfo);

        setItemDic.Add(SetItem.CompleteHourglass, 2);
        setItemDic.Add(SetItem.MirrorOfDawn, 2);
        setItemDic.Add(SetItem.EqualExchange, 2);
        setItemDic.Add(SetItem.Overeager, 3);
    }

    public void InitItemLists()
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
                    break;   
                case Define.ItemRating.Special:
                    brokenItemList.Add(item);
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
    }

    public void RemoveCurItemDic(Item item)
    {
        curItemDic.Remove(item.itemNameEng);
    }


    public void InitItems()
    {
        for(int i = 1; i < ItemAbility.Items.Count; ++i)
        {
            ItemAbility.Items[i].Init();
        }
    }

    public bool CheckSetItem(Item item/*SetItem setItem*/)
    {
        // TODO : ���� �������� SetItem Ȱ��ȭ, ��Ʈ������ ��� ����

        return false;
    }
}
