using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoMachine : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => true;

    private static int recoveryAmount = 100;
    private int recentReceiveDamage = 0;
    private int temp = 0;

    public override void Init()
    {
        recentReceiveDamage = GameManager.Instance.Player.playerBase.RecentReceiveDamage;
    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(NanoMachineAbility);
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(NanoMachineAbility);
        GameManager.Instance.Player.HPRelatedItemEffects.AddListener(NanoMachineAbility);
    }

    public void NanoMachineAbility()
    {
        if(recentReceiveDamage > 0)
        {
            temp = recoveryAmount;
            recoveryAmount = Mathf.Clamp(recoveryAmount - recentReceiveDamage, 0, 100);

            GameManager.Instance.Player.playerBase.Hp += temp - recoveryAmount;

            if (recoveryAmount < 0)
            {
                ItemManager.Instance.RemoveCurItemDic(ItemManager.Instance.allItemDic[this.GetType().Name]);
                // TODO : 아이템 파손 UI 띄우기
            }
        }
    }
}
