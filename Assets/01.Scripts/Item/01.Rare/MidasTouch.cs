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


    public override void Init()
    {
        isEquip = false;
    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void LastingEffect()
    {
        isEquip = true;
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.RemoveListener(MidasTouchAbility);
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.AddListener(MidasTouchAbility);
    }

    public override void Disabling()
    {
        isEquip = false;
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.RemoveListener(MidasTouchAbility);
    }

    public void MidasTouchAbility(Vector3 pos)
    {
        if (isEquip)
        {
            midasTouchChance = Random.Range(0, 100);
            if(midasTouchChance < 10)
            {
                FragmentCollectManager.Instance.OneTimeIncrease += 1;
            }
        }
    }
}
