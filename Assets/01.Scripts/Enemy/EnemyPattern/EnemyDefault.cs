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

    protected Transform playerTransform;
    protected Rigidbody2D rigid;
    protected float distanceToPlayer;

    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float minDistance = 1f;

    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip attackClip;

    public Coroutine actCoroutine = null;
    public Animator anim;

    protected SpriteRenderer sprite;

    protected int _attack = Animator.StringToHash("Attack");
    protected int _move = Animator.StringToHash("Move");

    Material hitMat;
    Material spriteLitMat;

    float changeTime = 0.07f;
    WaitForEndOfFrame wait;
    public Vector3 hitPoint => Vector3.zero;

    private bool isDead = false;

    void OnEnable()
    {
        SetStatus();
        if (actCoroutine != null) actCoroutine = null;
        isDead = false;
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteLitMat = Managers.Resource.Load<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
        hitMat = Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Mat/HitMat.mat");
    }
    void Start()
    {
        playerTransform = GameManager.Instance.Player.transform;
        anim = GetComponent<Animator>();

        sprite = GetComponent<SpriteRenderer>();
        AnimInit();
    }


    void Update()
    {
        Act();
    }

    public void SetStatus()
    {
        if (enemySO == null) return;

        hp = enemySO.hp;
        damage = enemySO.damage;
        speed = enemySO.speed;
    }

    public void AnimInit()
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController();

        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (moveClip != null) overrideController["Move"] = moveClip;
        if (attackClip != null) overrideController["Attack"] = attackClip;

        anim.runtimeAnimatorController = overrideController;
    }
    public void Act()
    {
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        
        if (actCoroutine != null) return;
        
        switch (distanceToPlayer)
        {
            case var a when a <= detectDistance && a > minDistance:
                actCoroutine = StartCoroutine(MoveToPlayer());
                break;
            case var a when a <= minDistance:
                actCoroutine = StartCoroutine(AttackToPlayer());
                break;
            default:
                break;
        }
    }

    public virtual IEnumerator MoveToPlayer()
    {
        if (rigid == null) yield break;
        if (moveClip != null) anim.SetBool(_move, true);


        Vector2 dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = Mathf.Sign(dir.x) > 0;
        rigid.velocity = dir * speed;


        yield return null;
        actCoroutine = null;
    }

    public virtual IEnumerator AttackToPlayer()
    {
        if (GameManager.Instance.Player.playerBase.IsPDead) yield break;

        anim.SetBool(_move, false);
        if(rigid != null)
        rigid.velocity = Vector2.zero;
        if (attackClip != null) anim.SetTrigger(_attack);

    }

    public virtual void OnDamage(float damage, GameObject damageDealer, float critChance)
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
            StartCoroutine(GetHit());


    }
    IEnumerator GetHit()
    {
        float timer = 0;
        sprite.material = hitMat;
        while(changeTime > timer)
        {
            timer+= Time.deltaTime;
            hitMat.SetTexture("_Texture2D",sprite.sprite.texture); 
             yield return null;
        }
        sprite.material = spriteLitMat;
    }
    public virtual void EnemyDead()
    {
        //if (transform.parent != null)
        isDead = true;
        EnemySpawnManager.Instance.RemoveEnemyInList(gameObject.GetComponent<Poolable>());
        FragmentCollectManager.Instance.AddFragment(gameObject);

        gameObject.SetActive(false);
    }
}
