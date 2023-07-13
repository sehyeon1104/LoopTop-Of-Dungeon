using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotoursAxe : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => true;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private bool isEquip = false;

    private float abilityDuration = 5f;
    private float delay = 5f;


    // 중첩수
    private int stack = 0;

    // 주변 적 수
    private int nearByEnemyCount = 0;
    // 적 체크 원 반지름
    private float radius = 3f;

    public override void Disabling()
    {
        isEquip = false;
    }

    public override void Init()
    {

    }

    public override void Use()
    {
        isEquip = true;
        delay = abilityDuration;
        StartCoroutine(CoolTime());
        StartCoroutine(MinotoursAxeAbility());
    }

    public IEnumerator MinotoursAxeAbility()
    {
        while (isEquip)
        {
            if (delay >= abilityDuration)
            {
                GameManager.Instance.Player.playerBase.AttackRange -= GameManager.Instance.Player.playerBase.InitAttackRange * 0.1f;
                GameManager.Instance.Player.playerBase.AttackRange -= GameManager.Instance.Player.playerBase.InitAttackRange * (0.04f * stack);

                stack = CheckNearByEnemyCount();
                stack = Mathf.Clamp(stack, 0, 4);
                delay = 0f;

                GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * 0.1f;
                GameManager.Instance.Player.playerBase.AttackRange += GameManager.Instance.Player.playerBase.InitAttackRange * (0.04f * stack);
            }
            yield return waitForEndOfFrame;
        }
    }

    public int CheckNearByEnemyCount()
    {
        Physics2D.OverlapCircle(GameManager.Instance.Player.transform.position, 3f);

        return nearByEnemyCount;
    }

    private IEnumerator CoolTime()
    {
        while (true)
        {
            if (delay < abilityDuration)
            {
                delay += Time.deltaTime;
            }

            yield return waitForEndOfFrame;
        }
    } 
}
