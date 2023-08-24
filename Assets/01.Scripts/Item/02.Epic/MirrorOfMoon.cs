using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorOfMoon : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => false;

    public override bool isSetElement => true;

    public override void Init()
    {

    }

    public override void Use()
    {
        MirrorOfMoonAbility();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.AttackSpeed -= GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.2f;
        GameManager.Instance.Player.playerBase.coolDown -= 20;
    }

    public override void SetItemCheck()
    {
        ItemManager.Instance.CheckSetItem(ItemManager.Instance.allItemDic[this.GetType().Name]);
    }

    public void MirrorOfMoonAbility()
    {
        // TODO : 스킬 쿨타임 감소
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.2f;
        GameManager.Instance.Player.playerBase.coolDown += 20;

    }
}
