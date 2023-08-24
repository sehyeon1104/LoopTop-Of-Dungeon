using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeInsurance : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

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
        GameManager.Instance.Player.RevivePlayer();
        ItemManager.Instance.RemoveCurItemDic(ItemManager.Instance.curItemDic[this.GetType().Name]);
        // TODO : 아이템이 파손됐음을 알리는 UI 출력
    }
}
