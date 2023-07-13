using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyPunch : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;

    public override void Disabling()
    {
        GameManager.Instance.Player.SkillRelatedItemEffects.RemoveListener(LuckyPunchAbility);
    }

    public override void Init()
    {

    }

    public override void Use()
    {

    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.SkillRelatedItemEffects.RemoveListener(LuckyPunchAbility);
        GameManager.Instance.Player.SkillRelatedItemEffects.AddListener(LuckyPunchAbility);
    }

    private int luckyPunchChance = 0;
    public void LuckyPunchAbility()
    {
        luckyPunchChance = Random.Range(0, 100);
        if (luckyPunchChance == 0)
            GameManager.Instance.Player.playerBase.FinalDamageMul += 1;
    }
}
