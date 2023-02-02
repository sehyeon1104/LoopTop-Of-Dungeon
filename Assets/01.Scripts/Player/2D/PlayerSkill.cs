using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoSingleton<PlayerSkill>
{

    public float skillCooltime { private set; get; } = 0f;

    public void Skill1()
    {
        skillCooltime = PlayerTransformation.Instance.playerTransformDataSO.skill1Delay;


    }

    public void Skill2()
    {
        skillCooltime = PlayerTransformation.Instance.playerTransformDataSO.skill2Delay;


    }

    public void UltimateSkill()
    {
        skillCooltime = PlayerTransformation.Instance.playerTransformDataSO.ultiSkillDelay;


    }

}
