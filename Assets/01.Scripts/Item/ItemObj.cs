using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemObj : MonoBehaviour
{
    [SerializeField]
    private Image itemInfoPanel = null;
    public Image ItemInfoPanel => itemInfoPanel;
    [SerializeField]
    private Image itemImage = null;
    [SerializeField]
    private TextMeshProUGUI itemNameTMP = null;
    [SerializeField]
    private TextMeshProUGUI itemDesTMP = null;
    [SerializeField]
    private TextMeshProUGUI priceTMP = null;
    [SerializeField]
    private Define.ItemType itemType;
    [SerializeField]
    private GameObject soldOutPanel = null;
    public int Num { get; set; }
    private Item item = null;

    public string itemName { get; private set; } = "";

    public bool isSold { get;  private set; } = false;
    private bool _isPurchaseAble = false;

    public bool IsPurchaseAble
    {
        get
        {
            return _isPurchaseAble;
        }
        set
        {
            _isPurchaseAble = value;
        }
    }

    private Vector3 pos;
    private Vector3 movePos;
    [SerializeField]
    private float delta = 0.05f;
    [SerializeField]
    private float speed = 1f;

    private WaitForEndOfFrame waitForEndOfFrame;

    private void Start()
    {
        soldOutPanel.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);
        pos = itemImage.transform.position;
        waitForEndOfFrame = new WaitForEndOfFrame();
        _isPurchaseAble = false;
        isSold = false;

        StartCoroutine(MoveUpDown());
    }

    public void SetValue(Item item)
    {
        this.item = item;
        UpdateValues();
    }

    public void UpdateValues()
    {
        itemType = item.itemType;
        itemImage.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
        itemName = item.itemName;
        itemNameTMP.text = $"<color={GameManager.Instance.itemRateColor[(int)item.itemRating]}>{itemName}</color>";
        itemDesTMP.SetText(item.itemDescription);
        priceTMP.SetText(string.Format("{0}", item.price));
    }

    private IEnumerator MoveUpDown()
    {
        while (true)
        {
            movePos = pos;
            movePos.y += delta * Mathf.Sin(Time.time * speed);
            itemImage.transform.position = movePos;

            yield return waitForEndOfFrame;
        }
    }

    public void PurchaseShopItem()
    {
        Debug.Log("isSold : " + isSold);
        if (GameManager.Instance.Player.playerBase.FragmentAmount < item.price || !_isPurchaseAble)
        {
            Rito.Debug.Log("구매 불가");
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Player/UnablePurchase.wav");
            return;
        }

        if (isSold)
        {
            Rito.Debug.Log("판매 완료된 상품");
            return;
        }

        isSold = true;

        GameManager.Instance.Player.playerBase.FragmentAmount -= (int)item.price;

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Player/Purchase.wav");
        itemImage.gameObject.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);
        soldOutPanel.SetActive(true);
        if (!ItemEffects.Items[item.itemNumber].isOneOff)
        {
            InventoryUI.Instance.AddItemSlot(item);
        }
        else
        {
            ItemEffects.Items[item.itemNumber].Use();
        }
    }
}
