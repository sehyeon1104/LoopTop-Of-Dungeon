using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelieversClock : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => true;

    public override bool isStackItem => true;

    private int stack = 0;

    public override void Disabling()
    {
        ItemManager.Instance.RoomClearRelatedItemEffects.RemoveListener(BelieversClockAbility);
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * (0.005f * stack);
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * (0.005f * stack);
        GameManager.Instance.Player.playerBase.AttackSpeed -= GameManager.Instance.Player.playerBase.InitAttackSpeed * (0.005f * stack);
    }

    public override void Init()
    {

    }

    public override void Use()
    {
        ItemManager.Instance.RoomClearRelatedItemEffects.RemoveListener(BelieversClockAbility);
        ItemManager.Instance.RoomClearRelatedItemEffects.AddListener(BelieversClockAbility);
    }

    public override void LastingEffect()
    {
        ItemManager.Instance.RoomClearRelatedItemEffects.RemoveListener(BelieversClockAbility);
        ItemManager.Instance.RoomClearRelatedItemEffects.AddListener(BelieversClockAbility);
    }

    public void BelieversClockAbility()
    {
        stack++;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.005f;
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.005f;
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.005f;

        if (stack >= 12)
        {
            ItemManager.Instance.DisablingItem(ItemManager.Instance.allItemDict[typeof(BelieversClock).Name]);
            InventoryUI.Instance.AddItemSlot(ItemManager.Instance.allItemDict[typeof(LiarsPrayer).Name]);
        }
        ShowStack();
    }

    public override void ShowStack()
    {
        base.ShowStack();

        InventoryUI.Instance.uiInventorySlotDic[this.GetType().Name].UpdateStack(stack);
    }
}
