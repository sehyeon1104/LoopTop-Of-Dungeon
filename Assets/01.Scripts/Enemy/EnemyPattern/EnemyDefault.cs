using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class EnemyDefault : MonoBehaviour, IHittable
{
    [SerializeField] private EnemySO enemySO;

    protected float hp = 1;
    protected float damage = 1;
    protected float speed = 1;
    protected float attackSpeed = 1f;

    protected Transform playerTransform;
    protected Rigidbody2D rigid;
    protected float distanceToPlayer;

    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float minDistance = 1f;

    [SerializeField] protected AnimationClip idleClip;
    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip attackClip;

    protected AnimatorOverrideController overrideController;

    public Coroutine actCoroutine = null;
    public Animator anim;

    protected SpriteRenderer sprite;

    protected int _attack = Animator.StringToHash("Attack");
    protected int _move = Animator.StringToHash("Move");

    private Material hitMat;
    private Material spriteLitMat;

    private Coroutine getHitCoroutine;

    private float changeTime = 0.07f;
    private WaitForEndOfFrame wait;
    private bool isPlayGetHitEffect = false;
    
    public Vector3 hitPoint => Vector3.zero;

    protected bool isMove { private set; get; } = false;
    protected bool isDead { private set; get; } = false;
    protected bool isFlip { private set; get; } = false;

    void OnEnable()
    {
        Init();
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteLitMat = Managers.Resource.Load<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
        hitMat = new Material(Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Mat/HitMat.mat"));
    }
    void Start()
    {
        playerTransform = GameManager.Instance.Player.transform;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        isFlip = sprite.flipX;
        AnimInit();
    }


    void Update()
    {
        Act();
    }

    public virtual void Init()
    {
        SetStatus();
        if (actCoroutine != null) actCoroutine = null;
        isDead = false;
        isPlayGetHitEffect = false;
        if (sprite != null)
        {
            sprite.material = spriteLitMat;
        }
    }

    public void SetStatus()
    {
        if (enemySO == null) return;

        hp = enemySO.hp;
        damage = enemySO.damage;
        speed = enemySO.speed;
    }

    public virtual void AnimInit()
    {
        overrideController = new AnimatorOverrideController();

        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (idleClip != null) overrideController["Idle"] = idleClip;
        if (moveClip != null) overrideController["Move"] = moveClip;
        if (attackClip != null) overrideController["Attack"] = attackClip;

        anim.runtimeAnimatorController = overrideController;
    }
    public void Act()
    {
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        
        switch (distanceToPlayer)
        {
            case var a when a <= detectDistance && a > minDistance:
                isMove = true;
                if(actCoroutine == null) actCoroutine = StartCoroutine(MoveToPlayer());
                break;
            case var a when a <= minDistance:
                isMove = false;
                if (actCoroutine == null) actCoroutine = StartCoroutine(AttackToPlayer());
                break;
            default:
                if(rigid != null) rigid.velocity = Vector3.zero;
                break;
        }
    }

    public virtual IEnumerator MoveToPlayer()
    {
        if (rigid == null) yield break;
        if (moveClip != null) anim.SetBool(_move, true);

        while (isMove)
        {
            Vector2 dir = (playerTransform.position - transform.position).normalized;
            sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);
            rigid.velocity = dir * speed;

            yield return null;
        }

        yield return null;
        actCoroutine = null;
    }

    public virtual IEnumerator AttackToPlayer()
    {
        if (GameManager.Instance.Player.playerBase.IsPDead) yield break;

        anim.SetBool(_move, false);
        if(rigid != null)
        rigid.velocity = Vector3.zero;
        if (attackClip != null) anim.SetTrigger(_attack);
    }

    public virtual void OnDamage(float damage, float critChance)
    {
        if (isDead)
        {
            return;
        }

        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
            StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject, true));
        }
        else
        {
            StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject));
        }

        hp -= damage;

        GameManager.Instance.PlayHitEffect(transform);

        if (hp <= 0)
            EnemyDead();
        else
        {
            if (isPlayGetHitEffect)
            {
                return;
            }
            else
            {
                StartCoroutine(GetHit());
            }
            //if(getHitCoroutine != null)
            //{
            //    StopCoroutine(getHitCoroutine);
            //}
            //getHitCoroutine = StartCoroutine(GetHit());
        }
    }
    private IEnumerator GetHit()
    {
        isPlayGetHitEffect = true;
        float timer = 0;
        hitMat.SetTexture("_Texture2D",sprite.sprite.texture); 
        sprite.material = hitMat;
        while(changeTime > timer)
        {   
            timer+= Time.deltaTime;
            hitMat.SetTexture("_Texture2D",sprite.sprite.texture);
            yield return wait;
        }
        sprite.material = spriteLitMat;
        isPlayGetHitEffect = false;
    }

    public virtual void EnemyDead()
    {
        if (!isDead)
        {
            isDead = true;

            if(GameManager.Instance.sceneType == Define.Scene.StageScene)
            {
                EnemySpawnManager.Instance.RemoveEnemyInList(gameObject.GetComponent<Poolable>());
            }
            else
            {
                Managers.Pool.Push(gameObject.GetComponent<Poolable>());
            }
            FragmentCollectManager.Instance.AddFragment(gameObject);

            gameObject.SetActive(false);
        }
    }
}
