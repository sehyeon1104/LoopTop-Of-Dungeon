using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEliteMob : GhostMobAI
{
    [SerializeField] GameObject trail;

    [SerializeField] AnimationClip[] attackClips;
    [SerializeField]  AnimationClip idleClip;
    AnimatorOverrideController overrideController;
    bool isFlip;
     int _count = Animator.StringToHash("Count");
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void FixedUpdate()
    {   
        base.FixedUpdate();
    }
    protected override void AnimInit()
    {
        overrideController = new AnimatorOverrideController();

        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (idleClip != null) overrideController["Idle"] = idleClip;
        if (moveClip != null) overrideController["Move"] = moveClip;

        for (int i = 0; i < attackClips.Length; i++)
        {
            overrideController[$"Attack{i + 1}"] = attackClips[i];
        }

        anim.runtimeAnimatorController = overrideController;
    }
    protected override IEnumerator AttackToEnemy()
    {
        int nowAttack = Random.Range(1, 4);

        anim.SetInteger(_count, nowAttack);

        anim.SetBool(_move, false);

        switch (nowAttack)
        {
            case 1:
                yield return StartCoroutine(Attack1());
                break;
            case 2:
                yield return StartCoroutine(Attack2());
                break;
            case 3:
                yield return StartCoroutine(Attack3());
                break;
        }

        actCoroutine = null;
    }
    IEnumerator Attack1()
    {
        trail.SetActive(true);
        anim.SetBool(_move, true);

        Vector2 dir = Vector2.zero;
        
        while (shortestdistance >= 2f)
        {
  
            sprite.flipX = (Mathf.Sign(flipVector.x) < 0);
            transform.Translate(flipVector.normalized * speed * 5f * Time.deltaTime);

            yield return null;
        }

        anim.SetBool(_move, false);
        sprite.flipX = (Mathf.Sign(dir.x) < 0);

        for (int i = 0; i < 2; i++)
        {

            overrideController[$"Attack1"] = attackClips[i];
            anim.SetTrigger(_attack);

            yield return new WaitForSeconds(0.4f);

            Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position + Vector3.right * Mathf.Sign(dir.x) * 2 + Vector3.up * 0.5f, new Vector2(2, 4), 0, 1<<enemyLayer);
            for (int j = 0; j < cols.Length; j++)
            {
                cols[j].GetComponent<IHittable>().OnDamage(5, 0);
            }
        }
        trail.SetActive(false);
        yield return new WaitForSeconds(2f);
    }
   IEnumerator Attack2()
    {
        
        if (shortestdistance <= 3.5f) yield break;

        for (int i = 0; i < 3; i++)
        {
            sprite.flipX = isFlip != (Mathf.Sign(flipVector.normalized.x) > 0);

            overrideController[$"Attack2"] = attackClips[(i + 1) % 2];
            anim.SetTrigger(_attack);

            yield return new WaitForSeconds(0.5f);

            float rot = Mathf.Atan2(flipVector.y, flipVector.x) * Mathf.Rad2Deg;
            Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Slash.prefab", transform.position, Quaternion.Euler(Vector3.forward * rot));

            yield return new WaitForSeconds(0.2f);
        }


        yield return new WaitForSeconds(3f);
    }
     IEnumerator Attack3()
    {
      
        if (shortestdistance > 2f) yield break;

        sprite.flipX =  (Mathf.Sign(flipVector.x) < 0);

        anim.SetTrigger(_attack);
        yield return new WaitForSeconds(1f);
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Wave.prefab", transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);
    }
}

