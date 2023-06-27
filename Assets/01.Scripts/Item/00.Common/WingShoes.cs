using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����޸� �Ź� ( �̵��ӵ� 10% ���� )
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
        Debug.Log("�����޸� �Ź� ȿ�� �ߵ�");
        Rito.Debug.Log("�̵��ӵ� 10% ����");
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.1f;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.1f;
    }
}
