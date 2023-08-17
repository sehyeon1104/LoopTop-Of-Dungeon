using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussianRoulette : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;

    private int stack = 0;

    public override void Disabling()
    {

    }

    public override void Init()
    {
        stack = 0;
    }

    public override void Use()
    {

    }

    public override void LastingEffect()
    {

    }

    public void RussianRouletteAbility()
    {
        stack++;
        if(stack >= 6)
        {
            stack = 0;
            // TODO : 공격력 10% ~ 200%의 총알 발사
        }
    }
}
