using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenHourglass : ItemBase
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

    public void BrokenHourglassAbility()
    {
        if(Random.Range(0, 100) < 10)
        {
            // TODO : 현재 사용한 스킬 쿨타임 1초 감소
        }
    }
}
