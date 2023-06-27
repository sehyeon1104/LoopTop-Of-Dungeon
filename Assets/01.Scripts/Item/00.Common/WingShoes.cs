using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 날개달린 신발 ( 이동속도 10% 증가 )
public class WingShoes : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("날개달린 신발 효과 발동");
        Rito.Debug.Log("이동속도 10% 증가");
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.1f;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.1f;
    }
}
