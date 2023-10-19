using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelieversClock : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => true;

    public override bool isStackItem => true;

    private static int stack = 0;

    public override void Disabling()
    {
        ItemManager.Instance.RoomClearRelatedItemEffects.RemoveListener(BelieversClockAbility);
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * (0.005f * stack);
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * (0.005f * stack);
        GameManager.Instance.Player.playerBase.AttackSpeed -= GameManager.Instance.Player.playerBase.InitAttackSpeed * (0.005f * stack);
        stack = 0;
    }

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void LastingEffect()
    {
        ItemManager.Instance.RoomClearRelatedItemEffects.RemoveListener(BelieversClockAbility);
        ItemManager.Instance.RoomClearRelatedItemEffects.AddListener(BelieversClockAbility);
        UpdateStackAndTimerPanel();
    }

    public void BelieversClockAbility()
    {
        Debug.Log("BelieversClockAbility");
        stack++;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.005f;
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.005f;
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.005f;

        UpdateStackAndTimerPanel();

        if (stack >= 12)
        {
            ItemManager.Instance.DisablingItem(ItemManager.Instance.allItemDict[typeof(BelieversClock).Name]);
            InventoryUI.Instance.AddItemSlot(ItemManager.Instance.allItemDict[typeof(LiarsPrayer).Name]);
        }
    }

    public override void UpdateStackAndTimerPanel()
    {
        base.UpdateStackAndTimerPanel();

        InventoryUI.Instance.uiInventorySlotDict[this.GetType().Name].UpdateStack(stack);
    }
}
