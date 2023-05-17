using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    private Item item = null;
    private List<Item> tempItemList = new List<Item>();
    private bool isItemSetting = false;
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

    private void Init()
    {
        item = null;
        spriteRenderer.sprite = null;
        tempItemList.Clear();
        tempItemList = GameManager.Instance.GetItemList();

        if(tempItemList.Count == GameManager.Instance.allItemList.Count - 1)
        {
            return;
        }

        for(int i = 0; i < tempItemList.Count; ++i)
        {
            Debug.Log($"{tempItemList[i].itemName} º“¿Ø");
        }


        // TODO : ¡ﬂ∫πæ∆¿Ã≈€ µÂ∂¯ æ»µ«∞‘≤˚ ºˆ¡§
        while (!isItemSetting)
        {
            isDuplication = false;
            Item temp = GameManager.Instance.allItemList[Random.Range(1, GameManager.Instance.allItemList.Count)];

            isDuplication = tempItemList.Contains(temp);
            //Debug.Log($"{temp.itemName} : {isDuplication}");

            if (!isDuplication)
            {
                item = temp;
                isItemSetting = true;
                break;
            }
        }

        spriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
                interactionButton.onClick.AddListener(TakeItem);
                UIManager.Instance.RotateInteractionButton();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)  
    {
        if(collision.CompareTag("Player"))
        {
           interactionButton.onClick.RemoveListener(TakeItem);
           UIManager.Instance.RotateAttackButton();
        }
        
    }
    // æ∆¿Ã≈€ »πµÊ «‘ºˆ
    public void TakeItem()
    {
        // TODO : »πµÊ Ω√ ¿Ã∆Â∆Æ √‚∑¬

        InventoryUI.Instance.AddItemSlot(item);
        UIManager.Instance.RotateAttackButton();
        interactionButton.onClick.RemoveListener(TakeItem);
        Managers.Pool.Push(this.GetComponent<Poolable>());
        
    }
}
