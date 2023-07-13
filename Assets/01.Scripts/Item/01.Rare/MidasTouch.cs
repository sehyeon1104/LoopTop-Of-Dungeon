using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidasTouch : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;

    private bool isEquip = false;

    private int midasTouchChance = 0;

    public override void Disabling()
    {
        isEquip = false;
    }

    public override void Init()
    {
        isEquip = false;
    }

    public override void Use()
    {
        isEquip = true;
    }

    public override void LastingEffect()
    {

    }

    public void MidasTouchAbility()
    {
        if (isEquip)
        {
            midasTouchChance = Random.Range(0, 100);
            if(midasTouchChance < 10)
            {
                // TODO : ÀçÈ­ 2¹è È¹µæ ±¸Çö
            }
        }
    }
}
