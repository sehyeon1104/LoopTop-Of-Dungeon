using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Skill Class
public partial class Player
{

    public float skillCooltime { private set; get; } = 0f;

    public void Skill1()
    {
        skillCooltime = playerTransformDataSO.skill1Delay;


    }

    public void Skill2()
    {
        skillCooltime = playerTransformDataSO.skill2Delay;


    }

    public void UltimateSkill()
    {
        skillCooltime = playerTransformDataSO.ultiSkillDelay;


    }

}
