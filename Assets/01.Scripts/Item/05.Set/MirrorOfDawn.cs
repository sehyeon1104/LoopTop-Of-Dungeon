using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorOfDawn : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Set;

    public override bool isPersitantItem => true;

    public override void Disabling()
    {

    }

    public override void LastingEffect()
    {

    }

    public override void Init()
    {
    }

    public override void Use()
    {
        Debug.Log("여명의 거울 준비 완료");
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.3f;
        GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * 0.3f;
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.3f;
        GameManager.Instance.Player.playerBase.SkillCoolDown += 20;
    }
}
