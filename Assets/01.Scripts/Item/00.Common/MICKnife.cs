using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MICKnife : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => true;

    public override void Disabling()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("MIC나이프 효과 발동");
        Debug.Log("기본공격 시 5% 확률로 출혈 부여");
        // TODO : 플레이어 기본공격에 MICKnifeAbility효과 추가
    }

    public override void LastingEffect()
    {
        // TODO : 플레이어 기본공격에 MICKnifeAbility효과 추가
    }

    private int bleedingChance = 0;
    public void MICKnifeAbility()
    {
        bleedingChance = Random.Range(0, 100);
        if(bleedingChance < 5)
        {
            // TODO : 공격이 적중한 대상에게 상태이상:출혈 부여
        }
    }

}
