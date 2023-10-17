using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 코드 최적화 필요. 너무 길다
public class BrokenItemRoom : RoomBase
{
    [Tooltip("복제할 오리지널 오브젝트")]
    private GameObject itemObjTemplate = null;

    private Vector3[] itemObjSpawnPos = new Vector3[4];
    private List<ItemObj> itemObjList = new List<ItemObj>();
    private List<int> itemList = new List<int>();
    private HashSet<int> itemSelectNum = new HashSet<int>();

    private float interactiveDis = 1f;

    private bool isTakeItem = false;

    private GameObject minimapIcon = null;

    private Coroutine coroutine = null;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    protected override void Awake()
    {
        base.Awake();
        minimapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/BrokenItemRoomIcon.prefab", transform);
        minimapIcon.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 1.5f);
        minimapIcon.SetActive(false);
    }

    private void Start()
    {
        itemObjTemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Test/ItemObj.prefab");
        InitItemPos();
        CreateObject();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (coroutine != null)
            StopCoroutine(coroutine);

        if (isTakeItem)
            return;

        UIManager.Instance.GetInteractionButton().onClick.RemoveListener(InteractiveToItemObj);
        UIManager.Instance.GetInteractionButton().onClick.AddListener(InteractiveToItemObj);

        coroutine = StartCoroutine(ToggleItemInfoPanel());
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = null;

        UIManager.Instance.GetInteractionButton().onClick.RemoveListener(InteractiveToItemObj);
    }

    protected override void IsClear() { }

    protected override void ShowIcon()
    {
        base.ShowIcon();
        minimapIcon.SetActive(true);
    }

    public void InitItemPos()
    {
        for (int i = 0; i < 4; ++i)
        {
            itemObjSpawnPos[i] = new Vector3(transform.position.x -5 + (i * 3), transform.position.y - 2);
        }
    }

    public void CreateObject()
    {
        GameObject newObject = null;
        ItemObj newItemObjComponent = null;

        Dictionary<string, Item> curItemDic = new Dictionary<string, Item>();
        curItemDic = ItemManager.Instance.GetCurItemDic();

        itemSelectNum.Clear();
        foreach(Item item in curItemDic.Values)
        {
            itemSelectNum.Add(item.itemNumber);
        }

        int index = 0;
        int loopCount = 0;
        int rand = 0;

        while (index < 4)
        {
            loopCount++;

            if (loopCount > 100)
            {
                Debug.Log("break while loop");
                for (int i = index; i < 4; ++i)
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

            rand = Random.Range(0, ItemManager.Instance.brokenItemCount);

            if (itemSelectNum.Contains(ItemManager.Instance.brokenItemList[rand].itemNumber) || ItemManager.Instance.brokenItemList[rand].itemNumber == 510)
                continue;

            itemSelectNum.Add(ItemManager.Instance.brokenItemList[rand].itemNumber);

            // 아이템 오브젝트 생성
            Item brokenItem = null;
            foreach (Item item in ItemManager.Instance.brokenItemList)
            {
                if (item.itemNumber == ItemManager.Instance.brokenItemList[rand].itemNumber)
                {
                    brokenItem = item;
                    itemList.Add(item.itemNumber);
                    break;
                }
            }

            newObject = Instantiate(itemObjTemplate);
            newItemObjComponent = newObject.GetComponent<ItemObj>();
            newItemObjComponent.SetValue(brokenItem);
            newObject.SetActive(true);
            itemObjList.Add(newItemObjComponent);

            index++;
        }

        SetItemPos();
    }

    public void SetItemPos()
    {
        for (int i = 0; i < itemObjSpawnPos.Length; ++i)
            itemObjList[i].transform.position = itemObjSpawnPos[i];
    }

    public IEnumerator ToggleItemInfoPanel()
    {
        while (true)
        {
            if (isTakeItem)
                break;

            for (int i = 0; i < itemObjList.Count; ++i)
            {
                if (Vector2.Distance(GameManager.Instance.Player.transform.position, itemObjList[i].transform.position) < interactiveDis)
                {
                    if (!itemObjList[i].IsPurchaseAble)
                    {
                        itemObjList[i].ItemInfoPanel.gameObject.SetActive(true);
                        itemObjList[i].IsPurchaseAble = true;
                    }
                }
                else
                {
                    if (itemObjList[i].IsPurchaseAble)
                    {
                        itemObjList[i].ItemInfoPanel.gameObject.SetActive(false);
                        itemObjList[i].IsPurchaseAble = false;
                    }
                }
            }

            for (int i = 0; i < itemObjList.Count; ++i)
            {
                if(itemObjList[i].IsPurchaseAble)
                {
                    UIManager.Instance.RotateInteractionButton();
                    break;
                }
                else
                    UIManager.Instance.RotateAttackButton();
            }

            yield return waitForEndOfFrame;
        }

        UIManager.Instance.RotateAttackButton();

        for (int i = 0; i < itemObjList.Count; ++i)
        {
            itemObjList[i].IsPurchaseAble = false;
            itemObjList[i].ItemInfoPanel.gameObject.SetActive(false);
            itemObjList[i].transform.DOScale(Vector2.zero, 1.5f).SetEase(Ease.InElastic);
        }
        CinemachineCameraShaking.Instance.CameraShake(8, 0.1f);
    }

    public void InteractiveToItemObj()
    {
        for (int i = 0; i < itemObjList.Count; ++i)
        {
            if (itemObjList[i].IsPurchaseAble && !itemObjList[i].isSold)
            {
                itemObjList[i].TakeBrokenItem();
                isTakeItem = true;
            }
        }
    }

    public List<int> GetItemList()
    {
        return itemList;
    }
}
