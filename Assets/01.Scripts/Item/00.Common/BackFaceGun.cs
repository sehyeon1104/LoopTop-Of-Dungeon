using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڸ� ���� ��(�⺻���� �� 5% Ȯ���� ���� �ο�)
public class BackFaceGun : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;
    public override bool isPersitantItem => true;

    public override void Disabling()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {

    }

    public override void LastingEffect()
    {

    }

    public void BackFaceGunAbility()
    {
        // TODO : �⺻���� �� �����̻�:���� �ο�

    }
}
