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

    private bool isSold = false;
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

    private void Start()
    {
        soldOutPanel.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);
    }

    public void SetValue(Item item)
    {
        this.item = item;
        UpdateValues();
    }

    public void UpdateValues()
    {
        itemType = item.itemType;
        itemImage.sprite = ShopManager.Instance.itemSprites[item.itemNumber];
        itemNameTMP.SetText(item.itemName);
        itemDesTMP.SetText(item.itemDescription);
        priceTMP.SetText(string.Format("{0}", item.price));
    }


    public void PurchaseShopItem()
    {
        // TODO : 재화 부족할 시 구매 불가 적용

        if(GameManager.Instance.Player.playerBase.FragmentAmount < item.price || isSold)
        {
            Rito.Debug.Log("구매 불가");
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Player/UnablePurchase.wav");
            return;
        }

        isSold = true;

        GameManager.Instance.Player.playerBase.FragmentAmount -= (int)item.price;

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Player/Purchase.wav");
        itemImage.gameObject.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);  
        soldOutPanel.SetActive(true);
        ItemEffects.Items[item.itemNumber].Use();
        InventoryUI.Instance.AddItemSlot(item);
    }
}
