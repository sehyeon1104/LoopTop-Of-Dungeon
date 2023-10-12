using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenKing : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => true;

    private float timer = 0f;
    private float targetTime = 10f;

    private bool isEquip = false;

    private Coroutine co = null;
    private WaitForSeconds waitForSec = new WaitForSeconds(1f);

    public override bool isStackItem => true;
    private static int stack = 0;
    int temp = 1;

    public override void Init()
    {
        co = null;
    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void Disabling()
    {
        isEquip = false;
        ResetStack();
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(ResetStack);
    }

    public override void LastingEffect()
    {
        isEquip = true;
        ResetStack();
        if (co != null)
            ItemManager.Instance.StopCoroutine(co);
        co = ItemManager.Instance.StartCoroutine(Timer());
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(ResetStack);
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.AddListener(ResetStack);
    }

    public void BrokenKingAbility()
    {
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.05f;
        ShowStack();
    }

    public void ResetStack()
    {
        GameManager.Instance.Player.playerBase.AttackSpeed -= GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.05f * stack;
        stack = 0;
        timer = 0f;
        if (co != null)
            ItemManager.Instance.StopCoroutine(co);
        co = ItemManager.Instance.StartCoroutine(Timer());
        ShowStack();
    }

    public IEnumerator Timer()
    {
        if (!isEquip)
            yield break;

        temp = 1;

        while (stack < targetTime)
        {
            temp++;
            stack++;
            BrokenKingAbility();
            yield return waitForSec;
        }
        co = null;
    }

    public override void ShowStack()
    {
        base.ShowStack();

        InventoryUI.Instance.uiInventorySlotDic[this.GetType().Name].UpdateStack(stack);
    }
}
