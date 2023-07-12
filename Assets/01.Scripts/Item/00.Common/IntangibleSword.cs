using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class IntangibleSword : ItemBase
{
    public override ItemType itemType => ItemType.buff;

    public override ItemRating itemRating => ItemRating.Common

    public override bool isPersitantItem => false;

    public override void Disabling()
    {
        throw new System.NotImplementedException();
    }

    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}
