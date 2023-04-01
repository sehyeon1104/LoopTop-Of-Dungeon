using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ghostMobAI : MonoBehaviour
{ 
    [SerializeField] float damage = 1;
    [SerializeField] float speed = 1;
    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float minDistance = 1f;

    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip attackClip;
    
    public Coroutine actCoroutine = null;
    Transform playerTrans = null;
    Animator anim;
    Vector2 flipVector = Vector2.zero;
    float shortestdistance = 100;
    protected SpriteRenderer sprite;

    readonly int _attack = Animator.StringToHash("Attack");
    readonly int _move   = Animator.StringToHash("Move");
    readonly int _idle   = Animator.StringToHash("Idle");
    Material hitMat;
    Material spriteLitMat;

    float changeTime = 0.07f;
    public Vector3 hitPoint => Vector3.zero;

    void OnEnable()
    {
        if (actCoroutine != null) actCoroutine = null;
    }
    private void Awake()
    {
        playerTrans = GameManager.Instance.Player.transform;
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

        if (actCoroutine != null) return;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectDistance);
        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].CompareTag("Enemy") || enemies[i].CompareTag("Boss")) {
                float playerMagnitude = Vector2.SqrMagnitude(transform.position - enemies[i].transform.position);
                Vector2 playeyToEnemyVec = transform.position - enemies[i].transform.position;
                print(playerMagnitude);
                if (shortestdistance > playerMagnitude)
                {
                    shortestdistance = playerMagnitude;
                    flipVector = playeyToEnemyVec;
                }
            }
        }
        print(shortestdistance);
        switch (Mathf.Sqrt(shortestdistance))
        {
            case var a when a <= detectDistance && a > minDistance:
                actCoroutine = StartCoroutine(MoveToEnemy(flipVector.normalized));
                break;
            case var a when a <= minDistance:
                actCoroutine = StartCoroutine(AttackToEnemy());
                break;
            default:
                break;
        }

    }
    public void Idle()
    {
        
    }
    public virtual IEnumerator MoveToEnemy(Vector2 dir)
    {

        if (moveClip != null) anim.SetBool(_move, true);
        sprite.flipX = Mathf.Sign(dir.x) > 0 ? true : false;
        transform.Translate((Vector2)playerTrans.position +(dir * Time.deltaTime * speed));

        yield return null;

        actCoroutine = null;
    }


    public virtual IEnumerator AttackToEnemy()
    {
        if (GameManager.Instance.Player.playerBase.IsPDead) yield break;

        anim.SetBool(_move, false);
        if (attackClip != null) anim.SetTrigger(_attack);

    }


    public virtual void EnemyDead()
    {
        //if (transform.parent != null)
        gameObject.SetActive(false);
    }
}
