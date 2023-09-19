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
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

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
    }

    public override void LastingEffect()
    {
        isEquip = true;
        co = ItemManager.Instance.StartCoroutine(Timer());//StartCoroutine(Timer());
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(ResetStack);
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.AddListener(ResetStack);
    }

    public void BrokenKingAbility()
    {
        GameManager.Instance.Player.playerBase.AttackSpeed += GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.05f;
    }

    public void ResetStack()
    {
        stack = 0;
        timer = 0f;
        GameManager.Instance.Player.playerBase.AttackSpeed -= GameManager.Instance.Player.playerBase.InitAttackSpeed * 0.05f * stack;
        if (co != null)
            ItemManager.Instance.StopCoroutine(co);
        co = ItemManager.Instance.StartCoroutine(Timer());
    }

    public IEnumerator Timer()
    {
        if (!isEquip)
            yield break;

        temp = 1;
        timer = 0f;

        while (timer < targetTime)
        {
            timer += Time.deltaTime;
            if(temp - Mathf.CeilToInt(timer) < 0)
            {
                temp++;
                stack++;
                BrokenKingAbility();
            }

            yield return waitForEndOfFrame;
        }
        co = null;
    }
}
