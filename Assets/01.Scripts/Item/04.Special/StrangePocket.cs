using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangePocket : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => true;

    public override void Init()
    {

    }

    public override void Use()
    {
        GameManager.Instance.Player.playerBase.FragmentAddAcq += GameManager.Instance.Player.playerBase.InitFragmentAddAcq * 0.2f;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(StrangePocketAbility);
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(StrangePocketAbility);
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.AddListener(StrangePocketAbility);
    }

    public void StrangePocketAbility()                                
    {
        FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, Random.Range(0, 50));
    }
}
