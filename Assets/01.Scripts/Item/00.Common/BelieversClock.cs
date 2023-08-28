using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelieversClock : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => true;

    private int stack = 0;

    public override void Disabling()
    {
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
            InventoryUI.Instance.RemoveItemSlot(ItemManager.Instance.allItemDic[typeof(BelieversClock).Name]);
            InventoryUI.Instance.AddItemSlot(ItemManager.Instance.allItemDic[typeof(LiarsPrayer).Name]);
        }
    }
}
