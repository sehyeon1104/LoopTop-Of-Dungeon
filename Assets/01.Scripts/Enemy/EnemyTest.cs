using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : EnemyDefault
{
    public override void MoveToPlayer()
    {
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime);
    }

    public override void AttackToPlayer()
    {
        Player.Instance.OnDamage(1, gameObject, 0);
    }
}
