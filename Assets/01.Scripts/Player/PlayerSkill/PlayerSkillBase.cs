using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkillBase : MonoBehaviour
{
    protected float detectiveDistance;
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
    GameObject playerVisual;
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
    protected float attackRange = 1.5f;
    private WaitForFixedUpdate waitforFixedUpdate = new WaitForFixedUpdate();
    public Dictionary<int, Action<int>> playerSkillUpdate = new Dictionary<int, Action<int>>();
    float attackTimer = 0;
    virtual protected void Update()
    {
        if (GameManager.Instance.platForm == Define.PlatForm.PC)
            return;

        if (Physics2D.OverlapCircle(transform.position, attackRange, 1 << enemyLayer))
        {
            Attack();
        }
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
        if ( !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !playerMovement.IsMove)
            return;

            playerAnim.SetTrigger("Attack");
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, playerBase.AttackRange, 1 << enemyLayer);
        
        if (enemies == null) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            PlayerVisual.Instance.VelocityChange(enemies[i].transform.position.x - transform.position.x);
            CinemachineCameraShaking.Instance.CameraShake();
            enemies[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, GameManager.Instance.Player.playerBase.CritChance);
        }
        GameManager.Instance.Player.AttackRelatedItemEffects?.Invoke();
    }
    protected abstract void UltimateSkill();

    void DashSkill()
    {
        StartCoroutine(Dash());
    }

    protected virtual IEnumerator Dash()
    {
        GameManager.Instance.Player.DashRelatedItemEffects.Invoke();

        timer = 0;
        alphaValue = 0;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        distance = 0;
        changePosition = transform.position;
        playerRigid.velocity = playerMovement.Direction * dashVelocity;
        currentPlayerSprite = playerSprite;
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
            if(player.IsInvincibility && timer >= dashTime * 0.25f)
                player.IsInvincibility = false;

            yield return waitforFixedUpdate;
        }
        playerMovement.IsMove = true;
        //foreach (var c in cloneList)
        //{
        //    Managers.Pool.Push(c);
        //}
        cloneList.Clear();
    }
    protected void init()
    {
        playerBase = GameManager.Instance.Player.playerBase;
        playerSkills.Add(1, FirstSkill);
        playerSkills.Add(2, SecondSkill);
        playerSkills.Add(3, ThirdSkill);
        playerSkills.Add(4, ForuthSkill);
        playerSkills.Add(5, FifthSkill);
        playerSkillUpdate.Add(1, FirstSkillUpdate);
        playerSkillUpdate.Add(2, SecondSkillUpdate);
        playerSkillUpdate.Add(3, ThirdSkillUpdate);
        playerSkillUpdate.Add(4, ForuthSkillUpdate);
        playerSkillUpdate.Add(5, FifthSkillUpdate);
        attack = Attack;
        ultimateSkill = UltimateSkill;
        dashSkill = DashSkill;
        dashVelocity = 20f;
        detectiveDistance = 10;
        dashDuration = 0.2f;
        dashTime = 0.15f;
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    protected void Cashing()
    {
        playerVisual = transform.Find("PlayerVisual").gameObject;
        dashObj = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Player/Ghost/DashClone.prefab");
        dashSprite = dashObj.GetComponent<SpriteRenderer>();
        playerSprite = playerVisual.GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRigid = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.Player;
        playerAnim = playerVisual.GetComponent<Animator>();
        init();
    }
}
