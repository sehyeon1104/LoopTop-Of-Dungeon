using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussianRoulette : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;
    public override bool isStackItem => true;

    private int stack = 0;

    public override void Disabling()
    {
        GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(RussianRouletteAbility);
    }

    public override void Init()
    {
        stack = 0;
    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(RussianRouletteAbility);
        GameManager.Instance.Player.AttackRelatedItemEffects.AddListener(RussianRouletteAbility);
    }

    public void RussianRouletteAbility()
    {
        stack++;
        if(stack >= 6)
        {
            stack = 0;
            GameObject bullet = Managers.Resource.Instantiate("Assets/03.Prefabs/Item/RussianRouletteBullet.prefab");
            bullet.transform.position = GameManager.Instance.Player.transform.position;
            bullet.transform.right = PlayerVisual.Instance.IsFlipX() ? GameManager.Instance.Player.transform.right : -GameManager.Instance.Player.transform.right;
        }
        //InventoryUI.Instance.uiInventorySlotDic[this.GetType().Name].UpdateStack(stack);
    }
}
