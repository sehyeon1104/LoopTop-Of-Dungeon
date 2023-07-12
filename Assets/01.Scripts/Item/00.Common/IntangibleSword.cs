using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

// 무형의 검 (공격속도 10% 증가)
public class IntangibleSword : ItemBase
{
    public override ItemType itemType => ItemType.buff;

    public override ItemRating itemRating => ItemRating.Common;

    public override bool isPersitantItem => false;

    public override void Disabling()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("무형의 검 효과 발동");
        Debug.Log("공격속도 10% 증가");
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.1f;
    }
}
