using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopRoom : RoomBase
{
    [SerializeField]
    private GameObject itemPosObj;
    [SerializeField]
    private Transform[] itemSpawnPosArr;
    private Button InteractionBtn;
    [field:SerializeField]
    public List<ItemObj> itemList { get; private set; } = new List<ItemObj>();
    private float playerSensingDis = 1.5f;
    public ItemObj[] itemobjArr { get; private set; }
    int itemobjCount = 0;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public bool isInteractive = false;

    IEnumerator toggleItemInfoPanel;
    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.EventRoom;
    }

    [SerializeField]
    private GameObject shopNpc;
    [SerializeField]
    private GameObject shopNpcIcon = null;

    protected override void Awake()
    {
        InteractionBtn = UIManager.Instance.GetInteractionButton();

        itemSpawnPosArr = itemPosObj.GetComponentsInChildren<Transform>();
        shopNpc = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/Da.panda(ShopNpc).prefab");
        shopNpcIcon = transform.Find("ShopIcon").gameObject;
        shopNpcIcon.SetActive(false);
        minimapIconSpriteRenderer = transform.parent.Find("MinimapIcon").GetComponent<SpriteRenderer>();
        minimapIconSpriteRenderer.gameObject.SetActive(false);
        //curLocatedMapIcon = transform.parent.Find("CurLocatedIcon").gameObject;
    }   
    private void Start()
    {
        toggleItemInfoPanel = ToggleItemInfoPanel();
        if (!ShopManager.Instance.isItemSetting)
        {
            ShopManager.Instance.SetItem();
        }

    }
    public void SpawnNPC()
    {
        Instantiate(shopNpc, transform.position + Vector3.up * 3, Quaternion.identity, transform);
    }

    public void SetItemObjList(List<ItemObj> lists)
    {
        SpawnNPC();
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

    protected override void ShowIcon()
    {
        Debug.Log("ShowIcon");
        shopNpcIcon.SetActive(true);
    }

    protected override void IsClear()
    {
        isClear = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        isClear = true;
        InteractionBtn.onClick.AddListener(ShopManager.Instance.InteractiveToItem);

        StartCoroutine(toggleItemInfoPanel);
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

                    if (!itemobjArr[i].ItemInfoPanel.gameObject.activeSelf 
                        && itemobjArr[i].itemName != "Default"
                        && !itemobjArr[i].isSold)
                    {
                        isInteractive = false;
                        itemobjArr[i].ItemInfoPanel.gameObject.SetActive(true);
                        itemobjArr[i].IsPurchaseAble = true;
                    }
                }
                else
                {
                    if (itemobjArr[i].ItemInfoPanel.gameObject.activeSelf)
                    {
                        itemobjArr[i].ItemInfoPanel.gameObject.SetActive(false);
                        itemobjArr[i].IsPurchaseAble = false;
                    }
                }
            }

            if (itemobjCount > 0 || isInteractive)
                UIManager.Instance.RotateInteractionButton();
            else
                UIManager.Instance.RotateAttackButton();

            itemobjCount = 0;

            yield return waitForEndOfFrame;

        }

    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(toggleItemInfoPanel);
            InteractionBtn.onClick.RemoveListener(ShopManager.Instance.InteractiveToItem);
            base.OnTriggerExit2D(collision);
        }
    }

    public ItemObj GetPurchaseableItem()
    {
        for (int i = 0; i < itemobjArr.Length; ++i)
        {
            if (itemobjArr[i].IsPurchaseAble && !itemobjArr[i].isSold)
            {
                return itemobjArr[i];
            }
        }

        return null;
    }
}
