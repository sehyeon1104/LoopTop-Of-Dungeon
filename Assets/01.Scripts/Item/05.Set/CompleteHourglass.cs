using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteHourglass : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Set;

    public override bool isPersitantItem => true;

    public override void Disabling()
    {
        GameManager.Instance.Player.SkillRelatedItemEffects.RemoveListener(CompleteHourglassAbility);
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.SkillRelatedItemEffects.RemoveListener(CompleteHourglassAbility);
        GameManager.Instance.Player.SkillRelatedItemEffects.AddListener(CompleteHourglassAbility);
    }

    public override void Init()
    {

    }

    public override void Use()
    {
        GameManager.Instance.Player.playerBase.SkillCoolDown += 30;
        LastingEffect();
    }

    public void CompleteHourglassAbility(int num)
    {
        if (num == 2 || num == 3) return;
        if (Random.Range(0, 10) < 3)
        {
            UIManager.Instance.SkillCoolRedution(num);
            Managers.Pool.PoolManaging("Assets/10.Effects/player/@Item/CompleteHourglassFX.prefab", GameManager.Instance.Player.transform);
        }
    }
}
