using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ghostMobAI : MonoBehaviour
{
    [SerializeField] float damage =3;
    [SerializeField] float speed = 3;
    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip attackClip;

    public Coroutine actCoroutine = null;
    Transform playerTrans = null;
    Animator anim;
    Collider2D enemy;
    Vector2 flipVector = Vector2.zero;
    float shortestdistance = 0;
    protected SpriteRenderer sprite;
    int enemyLayer;
    readonly int _attack = Animator.StringToHash("Attack");
    readonly int _move = Animator.StringToHash("Move");
    readonly int _idle = Animator.StringToHash("Idle");
    Material hitMat;
    Material spriteLitMat;
    WaitForSeconds attackTime = new WaitForSeconds(0.5f);
    public Vector3 hitPoint => Vector3.zero;

    void OnEnable()
    {
        if (actCoroutine != null) actCoroutine = null;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerTrans = GameManager.Instance.Player.transform;
        spriteLitMat = Managers.Resource.Load<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
        hitMat = Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Mat/HitMat.mat");
    }

    void Start()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        AnimInit();
    }


    void FixedUpdate()
    {
        print((playerTrans.position - transform.position).normalized);
        Act();
    }


    public void AnimInit()
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

    public void Act()
    {
        if (Vector2.SqrMagnitude(transform.position - playerTrans.position) >36)
        {
              transform.position = playerTrans.position + new Vector3((transform.position - playerTrans.position).normalized.x,(transform.position - playerTrans.position).normalized.y , 0);
        }
        
        if (actCoroutine != null) return;

        switch (Mathf.Sqrt(FindEnemies()))
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
    IEnumerator Idle()
    {
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
    public float FindEnemies()
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
        return shortestdistance;
    }
    public virtual IEnumerator MoveToEnemy(Vector2 dir)
    {
        if (moveClip != null) anim.SetBool(_move, true);

        sprite.flipX = Mathf.Sign(dir.x) > 0 ? true : false;
        transform.Translate(dir * Time.deltaTime * speed);

        yield return null;
        actCoroutine = null;
    }


    public virtual IEnumerator AttackToEnemy()
    {

        enemy.GetComponent<IHittable>().OnDamage(damage, 0);
        anim.SetBool(_move, false);
        if (attackClip != null) anim.SetTrigger(_attack);

        yield return attackTime;
        actCoroutine = null;

    }
}
