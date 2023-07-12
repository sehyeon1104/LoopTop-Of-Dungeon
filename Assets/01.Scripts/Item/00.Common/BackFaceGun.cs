using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤를 향한 총(기본공격 시 5% 확률로 공포 부여)
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
        // TODO : 기본공격 시 상태이상:공포 부여

    }
}
