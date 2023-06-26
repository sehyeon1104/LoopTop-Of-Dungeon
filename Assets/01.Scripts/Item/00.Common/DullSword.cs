using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �� (���ݷ� 5% ����)
public class DullSword : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("������ �� ȿ�� �ߵ�");
        Debug.Log("���ݷ� 5% ����");
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.05f;
    }
}