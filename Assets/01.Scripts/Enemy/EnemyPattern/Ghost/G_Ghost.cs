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

        GameManager.Instance.Player.OnDamage(damage, 0);

        EnemyDead();

        actCoroutine = null;

    }

    public override void EnemyDead()
    {
        EnemySpawnManager.Instance.RemoveEnemyInList(gameObject.GetComponent<Poolable>());

        gameObject.SetActive(false);
    }
}
