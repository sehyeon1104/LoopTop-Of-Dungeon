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
        isEquip = true;

        co = ItemManager.Instance.StartCoroutine(Timer());
    }

    public override void Disabling()
    {
        isEquip = false;
    }

    public override void LastingEffect()
    {
        isEquip = true;
        co = ItemManager.Instance.StartCoroutine(Timer());//StartCoroutine(Timer());
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
    }

    public IEnumerator Timer()
    {
        while (isEquip)
        {
            if (timer >= targetTime)
                continue;

            timer += Time.deltaTime;
            if(temp - Mathf.CeilToInt(timer) > 0)
            {
                temp = Mathf.CeilToInt(timer);
                stack++;
                BrokenKingAbility();
            }

            yield return waitForEndOfFrame;
        }
    }
}
