using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorOfSun : ItemBase
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
        MirrorOfSunAbility();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.2f;
        GameManager.Instance.Player.playerBase.AttackRange -= GameManager.Instance.Player.playerBase.InitAttackRange * 0.2f;
    }
    
    public override void SetItemCheck()
    {   
        ItemManager.Instance.CheckSetItem(ItemManager.Instance.allItemDic[this.GetType().Name]);
    }

    public void MirrorOfSunAbility()
    {
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.2f;
        GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * 0.2f;
    }
}
