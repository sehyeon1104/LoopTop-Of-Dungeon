using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overeager : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Set;

    public override bool isPersitantItem => true;

    public override void Disabling()
    {

    }

    public override void LastingEffect()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {

    }
}
