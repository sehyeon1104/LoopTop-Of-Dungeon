using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 거인의 장갑 ( 공격범위 10% 증가 )
public class GiantGlove : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("거인의 장갑 효과 발동");
        Debug.Log("공격범위 10% 증가");
        GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * 0.1f;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.AttackRange -= GameManager.Instance.Player.playerBase.InitAttackRange * 0.1f;
    }
}