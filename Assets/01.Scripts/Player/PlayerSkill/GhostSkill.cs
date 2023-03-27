using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSkill : PlayerSkillBase
{
    private PlayerSkillData skillData;
    private void OnEnable()
    {
        skillData = GameManager.Instance.Player.playerBase.PlayerTransformDataSO;
    }
    public override void FirstSkill()
    {
        StartCoroutine(JanpangSkill());
    }
    public override void SecondSkill()
    {

    }
    public override void ThirdSkill()
    {

    }
    public override void FifthSkill()
    {

    }


    public override void FourthSkill()
    {

    }



    public override void UltimateSkill()
    {

    }
    #region 스킬 구현
    IEnumerator JanpangSkill()
    {
        Collider2D[] attachOb = null;
        float timer =0;
        float timerA =0;
        while (timer > 1)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if(timerA>0.1f)
            {

            }
            Physics2D.OverlapCircleAll(transform.position, 1.5f);
            yield return null;
        }

    }
    #endregion 
}
