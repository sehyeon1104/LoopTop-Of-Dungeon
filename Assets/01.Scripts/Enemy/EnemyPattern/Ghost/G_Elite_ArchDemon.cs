using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Elite_ArchDemon : EnemyElite
{
    private GameObject trail;

    public override void Init()
    {
        base.Init();
        trail = GetComponentInChildren<MoveToTrailUV>().gameObject;
        trail?.SetActive(false);
    }

    protected override IEnumerator Attack1()
    {
        trail.SetActive(true);
        anim.SetBool(_move, true);

        Vector2 dir = Vector2.zero;

        while (distanceToPlayer >= 2f)
        {
            distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

            dir = (playerTransform.position - transform.position).normalized;
            sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);
            rigid.velocity = dir * speed * 5f;

            yield return null;
        }

        anim.SetBool(_move, false);
        rigid.velocity = Vector2.zero;

        dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);

        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.4f);

        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position + Vector3.right * Mathf.Sign(dir.x) * 2 + Vector3.up * 0.5f, new Vector2(2, 4), 0);
        for(int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag("Player"))
                GameManager.Instance.Player.OnDamage(2, 0);
        }

        trail.SetActive(false);
        yield return new WaitForSeconds(2f);
    }

    protected override IEnumerator Attack2()
    {
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer <= 3.5f) yield break;

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);

        for(int i = 0; i < 3; i++)
        {
            overrideController[$"Attack2"] = attackClips[i % 2];
            anim.SetTrigger(_attack);

            yield return new WaitForSeconds(0.5f);

            dir = (playerTransform.position - transform.position).normalized;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Slash.prefab", transform.position, Quaternion.Euler(Vector3.forward * rot));

            yield return new WaitForSeconds(0.2f);
        }


        yield return new WaitForSeconds(3f);
    }

    protected override IEnumerator Attack3()
    {
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer > 2f) yield break;

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);

        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(1f);
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Wave.prefab", transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);
    }
}
