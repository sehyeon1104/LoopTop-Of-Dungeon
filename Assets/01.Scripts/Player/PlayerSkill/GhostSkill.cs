using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSkill : PlayerSkillBase
{
    GameObject smoke;
    PlayerSkillData skillData;

    private void Awake()
    {
        init();

        smoke = Managers.Resource.Load<GameObject>("Assets/10.Effects/ghost/Smoke.prefab");
    }
    private void Start()
    {
        print(playerBase.PlayerTransformTypeFlag);
        
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
        Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform.parent);
        while (timer > 1)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                for(int i=0; i<attachObjs.Length; i++)
                {
                    attachObjs[i].GetComponent<IHittable>().OnDamage(5, attachObjs[i].gameObject, playerBase.CritChance);
                }
                timerA = 0;
            }
            yield return null;
        }

    }



    #endregion
}
