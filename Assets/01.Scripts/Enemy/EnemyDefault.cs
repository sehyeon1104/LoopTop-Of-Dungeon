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
    public AnimatorOverrideController animClips;

    protected int _attack = Animator.StringToHash("Attack");
    protected int _move = Animator.StringToHash("Move");

    public Vector3 hitPoint => Vector3.zero;

    void Start()
    {
        playerTransform = Player.Instance.transform;
        anim = GetComponent<Animator>();
        animClips = GetComponent<AnimatorOverrideController>();

        if (moveClip != null) animClips["Move"] = moveClip;
        if (attackClip != null) animClips["Attack"] = attackClip;
    }


    void Update()
    {
        Act();
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

    public void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
            StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject, true));
        }

        hp -= (int)damage;
        StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject));
        if (hp <= 0)
        {
            EnemyDead();
        }
    }

    public void EnemyDead()
    {
        if (transform.parent != null)
            EnemySpawnManager.Instance.RemoveEnemyInList(gameObject);

        gameObject.SetActive(false);
    }
}
