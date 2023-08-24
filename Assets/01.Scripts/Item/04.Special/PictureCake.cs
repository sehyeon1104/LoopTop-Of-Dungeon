using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureCake : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        PictureCakeAbility(); 
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.CritChance += 20;
        GameManager.Instance.Player.playerBase.CritDamage -= 50;
    }

    public void PictureCakeAbility()
    {
        GameManager.Instance.Player.playerBase.CritChance -= 20;
        GameManager.Instance.Player.playerBase.CritDamage += 50;
    }

}
