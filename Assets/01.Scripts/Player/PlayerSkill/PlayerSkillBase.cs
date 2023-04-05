using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class PlayerSkillBase : MonoBehaviour
{
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Rigidbody2D playerRigid;
    [HideInInspector] public SpriteRenderer playerSprite;
    protected int enemyLayer;
    protected float dashVelocity = 0;
    protected float dashDuration = 0;
    public PlayerBase playerBase;
    public List<Action<int>> playerSkills = new List<Action<int>>();
    public Action ultimateSkill;
    public Action dashSkill;
    public Action attack;
    protected abstract void FirstSkill(int level);

    protected abstract void SecondSkill(int level);
    protected abstract void ThirdSkill(int level);

    protected abstract void ForuthSkill(int level);

    protected abstract void FifthSkill(int level);

    protected abstract void Attack();
    protected abstract void UltimateSkill();

    protected abstract void DashSkill();

    protected void init()
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
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    protected void Cashing()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRigid = GetComponentInParent<Rigidbody2D>();
        init();
    }
}
