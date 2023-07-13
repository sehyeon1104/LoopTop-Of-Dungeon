using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltyWater : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

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

    public void SaltyWaterAbility(GameObject damageObj)
    {
        // TODO : 데미지를 입은 오브젝트가 출혈 일 경우 추가데미지
    }
}
