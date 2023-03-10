using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDefault : MonoBehaviour, IHittable
{
    protected int hp;
    protected float damage;

    protected Transform playerTransform;
    protected float distanceToPlayer;

    [SerializeField] private float detectDistance = 5f;
    [SerializeField] private float minDistance = 1f;

    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip attackClip;

    public Coroutine actCoroutine = null;
    public Animator anim;

    protected int _attack = Animator.StringToHash("Attack");
    protected int _move = Animator.StringToHash("Move");

    public Vector3 hitPoint => Vector3.zero;

    void Start()
    {
        playerTransform = Player.Instance.transform;
        anim = GetComponent<Animator>();
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

        if (moveClip != null) overrideController["Move"] = moveClip;
        if (attackClip != null) overrideController["Attack"] = attackClip;

        anim.runtimeAnimatorController = overrideController;
    }
    public void Act()
    {
        if (actCoroutine != null) return;
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        switch (distanceToPlayer)
        {
            case var a when a <= detectDistance && a >= minDistance:
                actCoroutine = StartCoroutine(MoveToPlayer());
                break;
            case var a when a < minDistance:
                actCoroutine = StartCoroutine(AttackToPlayer());
                break;
            default:
                break;
        }
    }

    public virtual IEnumerator MoveToPlayer(){ yield break; }
    public virtual IEnumerator AttackToPlayer(){ yield break; }

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

        hp -= (int)damage;
        if (hp <= 0)
        {
            EnemyDead();
        }
    }

    public virtual void EnemyDead()
    {
        //if (transform.parent != null)
        EnemySpawnManager.Instance.RemoveEnemyInList(gameObject);

        gameObject.SetActive(false);
        gameObject.transform.SetParent(null);
    }
}
