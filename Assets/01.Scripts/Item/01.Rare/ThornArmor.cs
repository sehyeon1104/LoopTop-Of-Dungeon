using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornArmor : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;

    private int thornArmorChance = 0;

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

    public void ThornArmorAbility(GameObject damageObj)
    {
        thornArmorChance = Random.Range(0, 100);
        if(thornArmorChance < 30)
        {
            damageObj.GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Attack);
        }
    }
}
