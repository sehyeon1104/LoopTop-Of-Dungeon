using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Skeleton : EnemyDefault
{
    WaitForSeconds attackWait = new WaitForSeconds(2f);

    private void OnEnable()
    {
        hp = 15;
        damage = 1;
    }

    public override IEnumerator MoveToPlayer()
    {
        if (moveClip != null) anim.SetBool(_move, true);

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime);

        yield return null;

        actCoroutine = null;
    }

    public override IEnumerator AttackToPlayer()
    {
        if (Player.Instance.isPDead) yield break;

        anim.SetBool(_move, false);
        if (attackClip != null) anim.SetTrigger(_attack);

        Player.Instance.OnDamage(1, gameObject, 0);

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
