using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    private Item item = null;
    private List<Item> tempItemList= new List<Item>();

    private bool isDuplication = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Init();
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
            for(int i = 0; i < tempItemList.Count; ++i)
            {
                if(temp == tempItemList[i])
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

    private void TakeItem()
    {
        // TODO : »πµÊ Ω√ ¿Ã∆Â∆Æ √‚∑¬
        
        InventoryUI.Instance.AddItemSlot(item);
        Destroy(gameObject);
    }
}
