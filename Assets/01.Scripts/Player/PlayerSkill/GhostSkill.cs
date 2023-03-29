using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSkill : PlayerSkillBase
{
    float janpanDuration = 5f;
    PlayerSkillData skillData;

    private void Awake()
    {
        init();
    }

    public override void FirstSkill(int level)
    {
        StartCoroutine(JanpangSkill(level));
    }
    public override void SecondSkill(int level)
    {

    }
    public override void ThirdSkill(int level)
    {

    }
    public override void ForuthSkill(int level)
    {

    }
    public override void FifthSkill(int level)
    {

    }
    public override void UltimateSkill()
    {

    }

    public override void DashSkill()
    {

    }

    #region 스킬 구현
    IEnumerator JanpangSkill(int level)
    {
        Collider2D[] attachObjs = null;
        float timer = 0;
        float timerA = 0;
        GameObject smoke = Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform.parent);

        while (timer < janpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, 1.7f);
                for (int i = 0; i < attachObjs.Length; i++)
                {
                    if (attachObjs[i].CompareTag("Enemy")||attachObjs[i].CompareTag("Boss"))
                    {
                        attachObjs[i].GetComponent<IHittable>().OnDamage(1, attachObjs[i].gameObject, playerBase.CritChance);
                    }
                }
                timerA = 0;
            }
            yield return null;
        }
        Managers.Pool.Push(smoke.GetComponent<Poolable>());

    }



    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
