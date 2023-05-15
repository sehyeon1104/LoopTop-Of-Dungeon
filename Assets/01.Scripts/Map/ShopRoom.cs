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
    private List<ItemObj> itemList = new List<ItemObj>();
    private float playerSensingDis = 1.5f;
    private ItemObj[] itemobjArr;
    int itemobjCount = 0;
    private WaitForEndOfFrame waitForEndOfFrame;

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.Shop;
    }

    [SerializeField]
    private GameObject shopNpc;

    private void Awake()
    {
        InteractionBtn = UIManager.Instance.playerUI.transform.Find("RightDown/Btns/Interaction_Btn").GetComponent<Button>();

        itemSpawnPosArr = itemPosObj.GetComponentsInChildren<Transform>();
        shopNpc = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/Da.panda(ShopNpc).prefab");
        minimapIconSpriteRenderer = transform.parent.Find("MinimapIcon").GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InteractionBtn.onClick.AddListener(ShopManager.Instance.InteractiveToItem);

        if (!ShopManager.Instance.isItemSetting)
        {
            ShopManager.Instance.SetItem();
        }
    }
    public void SpawnNPC()
    {
        Instantiate(shopNpc, transform.position + Vector3.up * 3, Quaternion.identity);
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



    protected override void IsClear()
    {
        isClear = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        StartCoroutine(ToggleItemInfoPanel());
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

                    if (!itemobjArr[i].ItemInfoPanel.gameObject.activeSelf)
                    {
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

            if (itemobjCount > 0)
                UIManager.Instance.RotateInteractionButton();
            else
                UIManager.Instance.RotateAttackButton();


            itemobjCount = 0;
            yield return waitForEndOfFrame;

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var itemobj in itemobjArr)
            {
                itemobj.Num = 0;
                StopCoroutine(ToggleItemInfoPanel());
            }
        }
    }

    public ItemObj GetPurchaseableItem()
    {
        for (int i = 0; i < itemobjArr.Length; ++i)
        {
            if (itemobjArr[i].IsPurchaseAble)
            {
                return itemobjArr[i];
            }
        }

        return null;
    }
}
