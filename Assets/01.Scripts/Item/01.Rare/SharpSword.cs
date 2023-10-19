﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Rito.Debug;

// 날카로운 검 (공격력 20% 증가, 적 처치 시 공격력 5% 증가 (최대 3 중첩, 지속시간 10초) )
public class SharpSword : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;
    public override bool isStackItem => true;

    private Coroutine co = null;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private float abilityDuration = 10f;
    private float delay = 0f;

    private int stack = 0;

    public override void Init()
    {
        stack = 0;
        delay = abilityDuration;
        co = null;
    }

    public override void Use()
    {
        Debug.Log("날카로운 검 효과 발동");
        Debug.Log("공격력 20% 증가, 적 처치 시 공격력 5% 증가 (최대 3 중첩, 지속시간 10초)");
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.2f;
        // StartCoroutine(CoolTime());
    }

    public override void LastingEffect()
    {
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.RemoveListener(SharpSwordAbility);
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.AddListener(SharpSwordAbility);
        // SharpSwordAbility();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.2f;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * (0.05f * stack);
    }

    public void SharpSwordAbility(Vector3 pos)
    {
        if (co != null)
        {
            ItemManager.Instance.StopCoroutine(co);
            co = null;
        }

        co = ItemManager.Instance.StartCoroutine(CoolTime());

        delay = 0f;
        if(stack >= 3)
            return;

        stack++;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.05f;
        ShowStack();
    }

    public void InitStack()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * (0.05f * stack);
        ShowStack();
        stack = 0;
    }

    private IEnumerator CoolTime()
    {
        while (true)
        {
            if (delay < abilityDuration)
            {
                delay += Time.deltaTime;
            }
            else
            {
                InitStack();
            }

            yield return waitForEndOfFrame;
        }
    }

    public override void ShowStack()
    {
        base.ShowStack();

        InventoryUI.Instance.uiInventorySlotDict[this.GetType().Name].UpdateStack(stack);
        InventoryUI.Instance.uiInventorySlotDict[this.GetType().Name].UpdateTimerPanel(abilityDuration);
    }
}