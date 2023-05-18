using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    private List<Item> tempItemList = new List<Item>();
    private Item item = null;
    private bool isItemSetting = false;
    private bool isDuplication = false;
    Button interactionButton;

    private HashSet<int> itemSelectNum = new HashSet<int>();
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
        itemSelectNum.Clear();
        tempItemList.Clear();
        tempItemList = GameManager.Instance.GetItemList();

        if(tempItemList.Count == GameManager.Instance.allItemList.Count - 1)
        {
            return;
        }

        for (int i = 0; i < tempItemList.Count; ++i)
        {
            itemSelectNum.Add(tempItemList[i].itemNumber);
        }

        int rand = 0;

        List<Item> allItemList = new List<Item>();
        allItemList = GameManager.Instance.allItemList;

        while (item == null)
        {
            rand = Random.Range(1, GameManager.Instance.allItemList.Count);

            if (itemSelectNum.Contains(rand))
                continue;

            itemSelectNum.Add(rand);

            item = GameManager.Instance.allItemList[rand];
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

        if (GameManager.Instance.GetItemList().Contains(item))
        {
            Debug.Log("¿ÃπÃ ∞°¡ˆ∞Ì¿÷¥¬ æ∆¿Ã≈€");
            return;
        }

        InventoryUI.Instance.AddItemSlot(item);
        UIManager.Instance.RotateAttackButton();
        interactionButton.onClick.RemoveListener(TakeItem);
        Managers.Pool.Push(this.GetComponent<Poolable>());
        
    }
}
