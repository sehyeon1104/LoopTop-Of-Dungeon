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
        // TODO : 아이템이 파손됐음을 알리는 UI 출력
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.DeadRelatedItemEffects.RemoveListener(LifeInsuranceAbility);
        ItemManager.Instance.RemoveCurItemDic(ItemManager.Instance.curItemDic[this.GetType().Name]);
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.DeadRelatedItemEffects.RemoveListener(LifeInsuranceAbility);
        GameManager.Instance.Player.DeadRelatedItemEffects.AddListener(LifeInsuranceAbility);
    }

    public void LifeInsuranceAbility()
    {
        GameManager.Instance.Player.RevivePlayer();
        ItemManager.Instance.RemoveCurItemDic(ItemManager.Instance.curItemDic[this.GetType().Name]);
    }
}
