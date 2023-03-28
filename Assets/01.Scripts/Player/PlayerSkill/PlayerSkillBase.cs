using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class PlayerSkillBase : MonoBehaviour
{
    public  PlayerBase playerBase;
    public  List<Action<int>> playerSkills = new List<Action<int>>();

    public abstract void FirstSkill(int level);

    public abstract void SecondSkill(int level);
    public abstract void ThirdSkill(int level);

    public abstract void ForuthSkill(int level);

    public abstract void FifthSkill(int level);

    public abstract void UltimateSkill();

    public abstract void DashSkill();

    public void init()
    {
        playerBase = GameManager.Instance.Player.playerBase;
        playerSkills.Add(FirstSkill);
        playerSkills.Add(SecondSkill);
        playerSkills.Add(ThirdSkill);
        playerSkills.Add(ForuthSkill);
        playerSkills.Add(FifthSkill);
        print(playerSkills.Count);

    }
}
