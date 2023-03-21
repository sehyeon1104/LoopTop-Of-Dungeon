using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemObj : MonoBehaviour
{
    [SerializeField]
    private Image itemInfoPanel = null;
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


    [SerializeField]
    private float playerSensingDis = 1.5f;

    private Item item = null;

    private WaitForEndOfFrame waitForEndOfFrame;

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

        itemImage.gameObject.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);
        soldOutPanel.SetActive(true);
        ItemEffects.ShopItems[item.itemNumber].Use();
    }

    public IEnumerator ToggleItemInfoPanel()
    {
        while (true)
        {
            if (Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) < playerSensingDis && !itemInfoPanel.gameObject.activeSelf)
            {
                itemInfoPanel.gameObject.SetActive(true);
            }
            else
            {
                itemInfoPanel.gameObject.SetActive(false);
            }

            yield return waitForEndOfFrame;
        }

    }
}
