using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyPunch : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;


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

    public override void Disabling()
    {
        GameManager.Instance.Player.SkillRelatedItemEffects.RemoveListener(LuckyPunchAbility);
    }

    private float luckyPunchChance = 0f;
    public void LuckyPunchAbility(int num)
    {
        luckyPunchChance = Random.Range(0f, 100f);
        if (luckyPunchChance <= 7.77f)
            GameManager.Instance.Player.playerBase.FinalDamageMul += 1;
    }
}
