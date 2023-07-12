using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyPiggy : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => false;

    public override void Disabling()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("작은 저금통 효과 발동");
        Debug.Log("재화 획득량 5% 증가");
        GameManager.Instance.Player.playerBase.FragmentAddAcq += GameManager.Instance.Player.playerBase.InitFragmentAddAcq * 0.05f;
    }
}
