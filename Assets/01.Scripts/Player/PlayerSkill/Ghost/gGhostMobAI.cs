using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostMobAI : MonoBehaviour
{

    [SerializeField] protected float damage =3;
    [SerializeField] protected float speed = 3;
    [SerializeField] protected private float detectDistance = 5f;
    [SerializeField] protected private float minDistance = 1f;
    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip attackClip;

    protected Coroutine actCoroutine = null;
    protected Transform playerTrans = null;
    protected Animator anim;
    protected Collider2D enemy;
    protected Vector2 flipVector = Vector2.zero;
    protected float shortestdistance = 0;
    protected SpriteRenderer sprite;
    protected int enemyLayer;
    readonly protected int _attack = Animator.StringToHash("Attack");
    readonly protected int _move = Animator.StringToHash("Move");
    readonly protected int _idle = Animator.StringToHash("Idle");
    protected WaitForSeconds attackTime = new WaitForSeconds(0.5f);

    protected virtual void OnEnable()
    {
        if (actCoroutine != null) actCoroutine = null;
    }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerTrans = GameManager.Instance.Player.transform;
    }

    protected virtual void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        AnimInit();
    }


    protected virtual void FixedUpdate()
    {
        Act();
    }


    protected virtual void AnimInit()
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController();
            
        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (moveClip != null)
        {
            overrideController["Move"] = moveClip;
            overrideController["Idle"] = moveClip;
        }
        if (attackClip != null) overrideController["Attack"] = attackClip;

        anim.runtimeAnimatorController = overrideController;
    }

    public virtual void Act()
    {
        
        if (actCoroutine != null) return;

        switch (FindEnemies())
        {
            case 0:
                actCoroutine = StartCoroutine(Idle());
                break;
            case var a when a > minDistance:
                actCoroutine = StartCoroutine(MoveToEnemy(flipVector.normalized));
                break;
            case var a when a > 0 && a <= minDistance:
                actCoroutine = StartCoroutine(AttackToEnemy());
                break;
            default:
                break;
        }

    }
   protected virtual IEnumerator Idle()
    {
        if (Vector2.SqrMagnitude(transform.position - playerTrans.position) > 36)
            transform.position = playerTrans.position + new Vector3((transform.position - playerTrans.position).normalized.x, (transform.position - playerTrans.position).normalized.y, 0);
        Vector2 playerVec = (playerTrans.position - transform.position).normalized;
        if (Vector2.SqrMagnitude(transform.position - playerTrans.position) < 1)
        {
            actCoroutine = null;
            yield break;
        }
        sprite.flipX = playerVec.x >= 0 ? true : false;
        transform.Translate(playerVec * speed * Time.deltaTime);
        yield return null;
        actCoroutine = null;
    }
    public virtual float FindEnemies()
    {

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectDistance, 1 << enemyLayer);
        if (enemies.Length == 0)
        {
            shortestdistance = 0;
            return 0; 
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            float playerMagnitude = Vector2.SqrMagnitude(transform.position - enemies[i].transform.position);
            Vector2 playeyToEnemyVec = enemies[i].transform.position - transform.position;

            if ((shortestdistance > playerMagnitude && enemies[i].gameObject.activeSelf)||i==0)
            {
                enemy = enemies[i];
                shortestdistance = playerMagnitude;
                flipVector = playeyToEnemyVec;
            }
        }
        return Mathf.Sqrt(shortestdistance);
    }
    protected virtual IEnumerator MoveToEnemy(Vector2 dir)
    {
        anim.SetBool(_move, true);
        sprite.flipX = Mathf.Sign(dir.x) > 0 ? true : false;
        transform.Translate(dir * Time.deltaTime * speed);

        yield return null;
        actCoroutine = null;
    }


    protected virtual IEnumerator AttackToEnemy()
    {

        enemy.GetComponent<IHittable>().OnDamage(damage, 0);
        anim.SetBool(_move, false);
        if (attackClip != null) anim.SetTrigger(_attack);

        yield return attackTime;
        actCoroutine = null;

    }
}
