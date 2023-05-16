using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private float interactionDis = 2f;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    private Item item = null;
    private List<Item> tempItemList = new List<Item>();
    private bool isCheck = false;
    private bool isDuplication = false;
    Button interactionButton;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Init();
    }
    private void Start()
    {
        interactionButton = UIManager.Instance.GetInteractionButton();
    }
    private void Update()
    {
        CheckDistance();
    }
    private void Init()
    {
        item = null;
        spriteRenderer.sprite = null;
        tempItemList.Clear();
        tempItemList = GameManager.Instance.GetItemList();

        while (item == null)
        {
            isDuplication = false;
            Item temp = GameManager.Instance.allItemList[Random.Range(1, GameManager.Instance.allItemList.Count)];
            for (int i = 0; i < tempItemList.Count; ++i)
            {
                if (temp == tempItemList[i] /*&& item.itemType != Define.ItemType.heal && item.itemType != Define.ItemType.broken*/)
                {
                    isDuplication = true;
                    break;
                }
            }
            if (!isDuplication)
            {
                item = temp;
            }
        }

        spriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
    }

    private void CheckDistance()
    {
        if (Vector2.SqrMagnitude(GameManager.Instance.Player.transform.position - transform.position) > interactionDis * interactionDis)
        {
            if (!isCheck)
            {
                interactionButton.onClick.AddListener(TakeItem);
                isCheck = true;
            }
        }
        else
        {
            interactionButton.onClick.RemoveListener(TakeItem);
            isCheck = false;
        }

    }

    // æ∆¿Ã≈€ »πµÊ «‘ºˆ
    public void TakeItem()
    {
        // TODO : »πµÊ Ω√ ¿Ã∆Â∆Æ √‚∑¬

        InventoryUI.Instance.AddItemSlot(item);
        Managers.Pool.Push(this.GetComponent<Poolable>());
    }
}
