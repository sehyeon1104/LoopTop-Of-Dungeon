using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoMachine : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => true;
    public override bool isStackItem => true;

    private static int recoveryAmount = 50;
    private int recentReceiveDamage = 0;
    private int temp = 0;

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(NanoMachineAbility);
        recoveryAmount = 50;
    }

    public override void LastingEffect()
    {
        ShowStack();
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(NanoMachineAbility);
        GameManager.Instance.Player.HPRelatedItemEffects.AddListener(NanoMachineAbility);
    }

    public void NanoMachineAbility()
    {
        recentReceiveDamage = GameManager.Instance.Player.playerBase.RecentReceiveDamage;
        if(recentReceiveDamage > 0)
        {
            temp = recoveryAmount;
            recoveryAmount = Mathf.Clamp(recoveryAmount - recentReceiveDamage, 0, 9999);

            GameManager.Instance.Player.playerBase.Hp += temp - recoveryAmount;

            ShowStack();

            if (recoveryAmount <= 0)
            {
                ItemManager.Instance.DisablingItem(ItemManager.Instance.allItemDict[this.GetType().Name]);
                // TODO : 아이템 파손 UI 띄우기
            }
        }
    }
    public override void ShowStack()
    {
        base.ShowStack();

        InventoryUI.Instance.uiInventorySlotDict[this.GetType().Name].UpdateStack(recoveryAmount);
    }
}
