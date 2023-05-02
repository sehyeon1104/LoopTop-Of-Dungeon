using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillBase : MonoBehaviour
{

    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public Rigidbody2D playerRigid;
    [HideInInspector] public SpriteRenderer playerSprite;
    SpriteRenderer currentPlayerSprite;
    [HideInInspector] protected Player player;
    [HideInInspector] public float dashTime;
    Vector3 changePosition;
    float timer = 0;
    float alphaValue = 0;
    protected GameObject dashObj;
    protected SpriteRenderer dashSprite;
    protected int enemyLayer;
    protected float dashVelocity = 0;
    protected float dashDuration = 0;
    float distance = 0;
    protected float instanceClonePerVelocity = 0.3f;
    public PlayerBase playerBase;
    public Dictionary<int, Action<int>> playerSkills = new Dictionary<int, Action<int>>();
    public Action ultimateSkill;
    public Action dashSkill;
    public Action attack;
    protected List<Poolable> cloneList = new List<Poolable>();
    protected Color dashCloneColor;
    protected Animator playerAnim;
    protected float attackRange = 1;
    private WaitForFixedUpdate waitforFixedUpdate = new WaitForFixedUpdate();
    public Dictionary<int, Action<int>> playerSkillUpdate = new Dictionary<int, Action<int>>();
    virtual protected void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, attackRange, 1 << enemyLayer))
            Attack();
    }
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

    protected virtual void Attack()
    {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || !playerMovement.IsMove)
            return;

            playerAnim.SetTrigger("Attack");
            Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange, 1 << enemyLayer);
            for (int i = 0; i < enemys.Length; i++)
            {
                PlayerVisual.Instance.VelocityChange(enemys[i].transform.position.x - transform.position.x);
                CinemachineCameraShaking.Instance.CameraShake();
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, GameManager.Instance.Player.playerBase.CritChance);
            }
    }
    protected abstract void UltimateSkill();

    void DashSkill()
    {
        StartCoroutine(Dash());
    }

    protected virtual IEnumerator Dash()
    {
        timer = 0;
        alphaValue = 0;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        distance = 0;
        changePosition = transform.position;
        playerRigid.velocity = playerMovement.Direction * dashVelocity;
        currentPlayerSprite = GetComponent<SpriteRenderer>();
        dashCloneColor = dashSprite.color;
        dashCloneColor.a = 0;
        while (timer < dashTime)
        {
            timer += Time.fixedDeltaTime;
            alphaValue = timer / dashTime;
            distance = Vector2.SqrMagnitude(transform.position - changePosition);
            if (distance > instanceClonePerVelocity * instanceClonePerVelocity)
            {
                changePosition = transform.position;
                Poolable dashPool = Managers.Pool.Pop(dashObj, transform.position);
                cloneList.Add(dashPool);
                SpriteRenderer dashPoolSprite = dashPool.GetComponent<SpriteRenderer>();
                dashPoolSprite.sprite = currentPlayerSprite.sprite;
                dashPoolSprite.flipX = currentPlayerSprite.flipX;
                dashCloneColor.a = alphaValue;
                dashPoolSprite.color = dashCloneColor;

            }
            yield return waitforFixedUpdate;
        }
        playerMovement.IsMove = true;
        player.IsInvincibility = false;
        foreach (var c in cloneList)
        {
            Managers.Pool.Push(c);
        }
        cloneList.Clear();
    }
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
        playerSkillUpdate.Add(4, ForuthSkillUpdate);
        playerSkillUpdate.Add(5, FifthSkillUpdate);
        attack = Attack;
        ultimateSkill = UltimateSkill;
        dashSkill = DashSkill;
        dashVelocity = 20f;
        dashDuration = 0.2f;
        dashTime = 0.15f;
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    protected void Cashing()
    {
        dashObj = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Player/Ghost/DashClone.prefab");
        dashSprite = dashObj.GetComponent<SpriteRenderer>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRigid = GetComponentInParent<Rigidbody2D>();
        player = GameManager.Instance.Player;
        playerAnim = GetComponent<Animator>();
        init();
    }
}
