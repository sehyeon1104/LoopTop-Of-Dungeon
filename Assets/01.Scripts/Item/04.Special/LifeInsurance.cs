using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeInsurance : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

    public override bool isPersitantItem => true;

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.DeadRelatedItemEffects.RemoveListener(LifeInsuranceAbility);
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.DeadRelatedItemEffects.RemoveListener(LifeInsuranceAbility);
        GameManager.Instance.Player.DeadRelatedItemEffects.AddListener(LifeInsuranceAbility);
    }

    public void LifeInsuranceAbility()
    {
        // TODO : �������� �ļյ����� �˸��� UI ���
        GameManager.Instance.Player.RevivePlayer();
        ItemManager.Instance.DisablingItem(ItemManager.Instance.curItemDict[this.GetType().Name]);
    }
}
