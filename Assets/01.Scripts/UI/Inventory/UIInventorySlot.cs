using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image itemInfoPanel = null;
    public Image ItemInfoPanel => itemInfoPanel;
    [SerializeField]
    private Image itemImage = null;
    [SerializeField]
    private Define.ItemType itemType;
    [SerializeField]
    private TextMeshProUGUI stackTMP = null;

    private Item item = null;

    public void SetValue(Item item)
    {
        this.item = item;
        UpdateValues();
    }

    public void UpdateValues()
    {
        itemType = item.itemType;
        itemImage.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
    }

    public void UpdateStack(int stack)
    {
        stackTMP.SetText(stack.ToString());
    }
}
