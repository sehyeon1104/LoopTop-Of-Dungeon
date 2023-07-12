using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

// ������ �� (���ݼӵ� 10% ����)
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
        Debug.Log("������ �� ȿ�� �ߵ�");
        Debug.Log("���ݼӵ� 10% ����");
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.1f;
    }
}
