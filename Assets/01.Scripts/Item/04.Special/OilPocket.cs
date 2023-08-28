using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPocket : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        OilPocketAbility();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.CritDamage += 20f;
        GameManager.Instance.Player.playerBase.SkillCoolDown += 20;
    }

    public void OilPocketAbility()
    {
        GameManager.Instance.Player.playerBase.CritDamage -= 20f;
        GameManager.Instance.Player.playerBase.SkillCoolDown -= 20;
    }
}
