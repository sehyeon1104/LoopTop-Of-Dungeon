using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_G_Skull : EnemyDefault
{
    WaitForSeconds attackWait = new WaitForSeconds(2f);

    private void OnEnable()
    {
        hp = 15;
        damage = 1;
    }

    public override IEnumerator MoveToPlayer()
    {
        if (moveClip != null && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && anim.GetCurrentAnimatorStateInfo(0).IsName("Move")) anim.SetBool(_move, true);

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime);

        yield return null;

        actCoroutine = null;
    }

    public override IEnumerator AttackToPlayer()
    {
        if (Player.Instance.isPDead) yield break;
        if (attackClip != null) anim.SetBool(_move, true);

        Player.Instance.OnDamage(1, gameObject, 0);

        yield return attackWait;

        actCoroutine = null;

    }

}
