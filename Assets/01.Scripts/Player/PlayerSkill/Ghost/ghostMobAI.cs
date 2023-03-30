using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ghostMobAI : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;

    protected float hp = 1;
    protected float damage = 1;
    protected float speed = 1;

    protected Transform playerTransform;
    protected float distanceToPlayer;

    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float minDistance = 1f;

    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip attackClip;

    public Coroutine actCoroutine = null;
    public Animator anim;
    Vector2 flipVector = Vector2.zero;
    float shortestdistance = 0;
    protected SpriteRenderer sprite;

    protected int _attack = Animator.StringToHash("Attack");
    protected int _move = Animator.StringToHash("Move");

    Material hitMat;
    Material spriteLitMat;

    float changeTime = 0.07f;
    WaitForEndOfFrame wait;
    public Vector3 hitPoint => Vector3.zero;

    void OnEnable()
    {
        SetStatus();
        if (actCoroutine != null) actCoroutine = null;
    }
    private void Awake()
    {
        spriteLitMat = Managers.Resource.Load<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
        hitMat = Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Mat/HitMat.mat");
    }
    void Start()
    {
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

        if (actCoroutine != null) return;

        Collider2D[] inEnemy = Physics2D.OverlapCircleAll(transform.position, detectDistance);
        for(int i=0; i<inEnemy.Length; i++)
        {
            if (inEnemy[i].CompareTag("Boss")|| inEnemy[i].CompareTag("Enemy"))
            {
                Vector2 flipFloatX = transform.position - inEnemy[i].transform.position;
                float distanceEnemy = Vector2.SqrMagnitude(transform.position - inEnemy[i].transform.position);
                if (distanceEnemy < shortestdistance)
                {
                    flipVector = flipFloatX;
                    shortestdistance = distanceEnemy;
                }
            }
        }

        switch (Mathf.Sqrt(shortestdistance))
        {
            case var a when a <= detectDistance && a > minDistance:
                actCoroutine = StartCoroutine(MoveToPlayer(flipVector));
                break;
            case var a when a <= minDistance:
                actCoroutine = StartCoroutine(AttackToPlayer());
                break;
            default:
                break;
        }
    }

    public virtual IEnumerator MoveToPlayer(Vector2 dir)
    {
        if (moveClip != null) anim.SetBool(_move, true);
        sprite.flipX = Mathf.Sign(dir.x) > 0 ? true : false;
        transform.Translate(dir * Time.deltaTime * speed);

        yield return null;

        actCoroutine = null;
    }

    public virtual IEnumerator AttackToPlayer()
    {
        if (GameManager.Instance.Player.playerBase.IsPDead) yield break;

        anim.SetBool(_move, false);
        if (attackClip != null) anim.SetTrigger(_attack);

    }

    public virtual void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
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
        while (changeTime > timer)
        {
            timer += Time.deltaTime;
            hitMat.SetTexture("_Texture2D", sprite.sprite.texture);
            yield return null;
        }
        sprite.material = spriteLitMat;
    }
    public virtual void EnemyDead()
    {
        //if (transform.parent != null)
        EnemySpawnManager.Instance.RemoveEnemyInList(gameObject.GetComponent<Poolable>());
        FragmentCollectManager.Instance.AddFragment(gameObject);

        gameObject.SetActive(false);
    }
}
