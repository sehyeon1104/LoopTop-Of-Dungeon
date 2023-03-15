using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Ghost : EnemyDefault
{
    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();

        GameManager.Instance.Player.OnDamage(damage, gameObject, 0);

        hp -= 1;
        EnemyDead();

        actCoroutine = null;

    }

    public override void EnemyDead()
    {
        base.EnemyDead();
    }
}
