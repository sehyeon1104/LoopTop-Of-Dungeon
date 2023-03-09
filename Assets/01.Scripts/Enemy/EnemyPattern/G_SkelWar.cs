using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_SkelWar : EnemyDefault
{
    WaitForSeconds attackWait = new WaitForSeconds(3f);

    public override IEnumerator MoveToPlayer()
    {
        if (moveClip != null) anim.SetBool(_move, true);

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = Mathf.Sign(dir.x) > 0 ? true : false ;
        transform.Translate(dir * Time.deltaTime * speed);

        yield return null;

        actCoroutine = null;
    }

    public override IEnumerator AttackToPlayer()
    {
        if (Player.Instance.isPDead) yield break;

        anim.SetBool(_move, false);
        if (attackClip != null) anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.7f);

        if(distanceToPlayer <= 1.5f)
            Player.Instance.OnDamage(damage, gameObject, 0);

        yield return attackWait;

        actCoroutine = null;

    }

    public override void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
        base.OnDamage(damage, damageDealer, critChance);
    }

    public override void EnemyDead()
    {
        base.EnemyDead();
    }
}
