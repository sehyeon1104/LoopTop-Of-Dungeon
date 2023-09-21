using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltedWaxWing : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => true;
    public override bool isStackItem => true;

    private static int stack = 0;

    private Coroutine co = null;
    private float targetTime = 5f;
    private float timer = 0f;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.05f * stack;
        GameManager.Instance.Player.DashRelatedItemEffects.RemoveListener(MeltedWaxWingAbility);
    }

    public override void LastingEffect()
    {
        if(stack > 0)
        {
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.05f * stack;
        }

        GameManager.Instance.Player.DashRelatedItemEffects.RemoveListener(MeltedWaxWingAbility);
        GameManager.Instance.Player.DashRelatedItemEffects.AddListener(MeltedWaxWingAbility);
    }

    private void ResetStack()
    {
        stack = 0;
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.05f * stack;
    }

    public void MeltedWaxWingAbility()
    {
        stack++;
        if(stack >= 16)
        {
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.05f * (stack - 1);
            stack = 0;
            GameManager.Instance.Player.OnDamage(10, 0);
            ItemManager.Instance.StopCoroutine(co);
        }
        else
        {
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.05f;
            if (co != null)
                ItemManager.Instance.StopCoroutine(co);
            co = ItemManager.Instance.StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        timer = 0;
        while(timer < targetTime)
        {
            timer += Time.deltaTime;
            yield return waitForEndOfFrame;
        }

        ResetStack();
    }
}
