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

        while (distanceToPlayer >= 1f)
        {
            distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

            Vector2 dir = (playerTransform.position - transform.position).normalized;
            sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);
            rigid.velocity = dir * speed * 5f;

            yield return null;
        }

        anim.SetBool(_move, false);
        trail.SetActive(false);
        rigid.velocity = Vector2.zero;
        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(2.5f);
    }

    protected override IEnumerator Attack2()
    {
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer <= 3.5f) yield break;

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);

        anim.SetTrigger(_attack);
        yield return new WaitForSeconds(3f);
    }

    protected override IEnumerator Attack3()
    {
        distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer > 2f) yield break;

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);

        anim.SetTrigger(_attack);
        yield return null;
    }
}
