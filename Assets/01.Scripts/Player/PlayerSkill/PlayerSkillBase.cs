using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class PlayerSkillBase : MonoBehaviour
{
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Rigidbody2D playerRigid;
    [HideInInspector] public SpriteRenderer playerSprite;
    [HideInInspector] protected Player player;
    [HideInInspector] public float dashTime;
    protected int enemyLayer;
    protected float dashVelocity = 0;
    protected float dashDuration = 0;
    protected float instanceClonePerVelocity = 0.5f;
    public PlayerBase playerBase;
    public Dictionary<int,Action<int>> playerSkills = new Dictionary<int,Action<int>>();
    public Action ultimateSkill;
    public Action dashSkill;
    public Action attack;
    protected List<Poolable> cloneList = new List<Poolable>();
    public Color dashCloneColor;
    public Dictionary<int, Action<int>> playerSkillUpdate = new Dictionary<int, Action<int>>();
    protected abstract void FirstSkill(int level);

    protected abstract void FirstSkillUpdate(int level);
    protected abstract void SecondSkill(int level);
    protected abstract void SecondSkillUpdate(int level);
    protected abstract void ThirdSkill(int level);
    protected abstract void ThirdSkillUpdate(int level);

    protected abstract void ForuthSkill(int level);
    protected abstract void ForuthSkillUpdate(int level);

    protected abstract void FifthSkill(int level);
    protected abstract void FifthSkillUpdate(int level);

    protected abstract void Attack();
    protected abstract void UltimateSkill();

    protected abstract void DashSkill();

    protected void init()
    {
        playerBase = GameManager.Instance.Player.playerBase;
        playerSkills.Add(1, FirstSkill);
        playerSkills.Add(2, SecondSkill);
        playerSkills.Add(3, ThirdSkill);
        playerSkills.Add(4, ForuthSkill);
        playerSkills.Add(5, FirstSkill);
        playerSkillUpdate.Add(1, FirstSkillUpdate);
        playerSkillUpdate.Add(2, SecondSkillUpdate);
        playerSkillUpdate.Add(3, ThirdSkillUpdate);
        playerSkillUpdate.Add(4, FifthSkillUpdate);
        playerSkillUpdate.Add(5,FifthSkillUpdate);
        attack = Attack;
        ultimateSkill = UltimateSkill;
        dashSkill = DashSkill;
        dashVelocity = 30f;
        dashDuration = 0.2f;
        dashTime = 0.1f;
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    protected void Cashing()
    {

        playerSprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRigid = GetComponentInParent<Rigidbody2D>();
        player = GameManager.Instance.Player;
        init();
    }
}
