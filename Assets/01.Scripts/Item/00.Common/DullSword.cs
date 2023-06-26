using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무뎌진 검 (공격력 5% 증가)
public class DullSword : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("무뎌진 검 효과 발동");
        Debug.Log("공격력 5% 증가");
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.05f;
    }
}