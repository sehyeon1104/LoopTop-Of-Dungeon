using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelieversClock : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => true;

    private int stack = 0;

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * (0.005f * stack);
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * (0.005f * stack);
        GameManager.Instance.Player.playerBase.AttackSpeed -= GameManager.Instance.Player.playerBase.InitAttackSpeed * (0.005f * stack);
    }

    public override void Init()
    {

    }

    public override void Use()
    {

    }

    public override void LastingEffect()
    {
        //TODO : 방 클리어 시 <- 조건추가
        stack++;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.005f;
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.005f;
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.005f;

        if(stack >= 12)
        {
            Disabling();
            new LiarsPrayer();
        }
    }
}
