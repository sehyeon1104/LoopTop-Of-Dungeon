using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenHourglass : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => true;

    public override bool isSetElement => true;

    public override void Disabling()
    {
        GameManager.Instance.Player.SkillRelatedItemEffects.RemoveListener(BrokenHourglassAbility);
    }

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.SkillRelatedItemEffects.RemoveListener(BrokenHourglassAbility);
        GameManager.Instance.Player.SkillRelatedItemEffects.AddListener(BrokenHourglassAbility);
    }

    public override void SetItemCheck()
    {
        ItemManager.Instance.CheckSetItem(ItemManager.Instance.allItemDic[this.GetType().Name]);
    }

    public void BrokenHourglassAbility(int num)
    {
        if(Random.Range(0, 100) < 10)
        {
                UIManager.Instance.SkillCoolCalculation(1, num);
        }
    }
}
