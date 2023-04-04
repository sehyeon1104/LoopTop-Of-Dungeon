using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class PlayerSkillBase : MonoBehaviour
{
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Rigidbody2D playerRigid;
    [HideInInspector] public SpriteRenderer playerSprite;
    public float dashVelocity = 0;
    public float dashDuration = 0;
    public PlayerBase playerBase;
    public List<Action<int>> playerSkills = new List<Action<int>>();
    public Action ultimateSkill;
    public Action dashSkill;
    public Action attack;
    public abstract void FirstSkill(int level);

    public abstract void SecondSkill(int level);
    public abstract void ThirdSkill(int level);

    public abstract void ForuthSkill(int level);

    public abstract void FifthSkill(int level);

    public abstract void Attack();
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
        attack = Attack;
        ultimateSkill = UltimateSkill;
        dashSkill = DashSkill;
        dashVelocity = 30f;
        dashDuration = 0.2f;
    }
    public void Cashing()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRigid = GetComponentInParent<Rigidbody2D>();
        init();
    }
}
