using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingHourglass : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.broken;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => true;

    public override void Init()
    {

    }

    public override void Use()
    {
        // 스킬쿨타임 50% ~ 150 랜덤 적용
    }
}
