using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltedWaxWing : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => true;

    private static int stack = 0;

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.05f * stack;
        GameManager.Instance.Player.DashRelatedItemEffects.RemoveListener(MeltedWaxWingAbility);
    }

    public override void LastingEffect()
    {
        if(stack > 0)
        {
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.05f * stack;
        }

        GameManager.Instance.Player.DashRelatedItemEffects.RemoveListener(MeltedWaxWingAbility);
        GameManager.Instance.Player.DashRelatedItemEffects.AddListener(MeltedWaxWingAbility);
    }

    public void MeltedWaxWingAbility()
    {
        stack++;
        if(stack >= 16)
        {
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.05f * (stack - 1);
            stack = 0;
            GameManager.Instance.Player.OnDamage(10, 0);
        }

        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.05f * stack;
    }
}
