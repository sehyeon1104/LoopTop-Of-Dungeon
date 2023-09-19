using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SmallSlime : EnemyDefault
{
    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();
        float dir = Mathf.Sign(playerTransform.position.x - transform.position.x);
        yield return new WaitForSeconds(0.5f);

        transform.DOMoveX(transform.position.x + dir * 3f, 0.2f);

        Collider2D col = Physics2D.OverlapBox(transform.position + Vector3.right * dir * 1.5f, new Vector2(3f, 0.5f), 0, 1 << 8);

        yield return new WaitForSeconds(0.1f);

        if (col != null)
            GameManager.Instance.Player.OnDamage(5, 0);

        yield return new WaitForSeconds(0.5f);
        actCoroutine = null;
    }
}
