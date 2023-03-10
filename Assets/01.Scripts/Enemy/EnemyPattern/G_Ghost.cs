using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Ghost : EnemyDefault
{
    public override IEnumerator MoveToPlayer()
    {
        if (moveClip != null) anim.SetBool(_move, true);

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * speed);

        yield return null;

        actCoroutine = null;
    }

    public override IEnumerator AttackToPlayer()
    {
        if (Player.Instance.isPDead) yield break;

        anim.SetBool(_move, false);
        if (attackClip != null) anim.SetTrigger(_attack);

        Player.Instance.OnDamage(damage, gameObject, 0);

        hp -= 1;
        EnemyDead();

        actCoroutine = null;

    }

    public override void EnemyDead()
    {
        base.EnemyDead();
    }
}
