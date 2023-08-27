using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.Default;

    public override Define.ItemRating itemRating => Define.ItemRating.Default;

    public override bool isPersitantItem => false;

    public override void Disabling()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {

    }
}
