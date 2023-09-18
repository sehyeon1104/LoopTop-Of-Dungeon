using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Slime : EnemyDefault
{
    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();
        float dir = Mathf.Sign(playerTransform.position.x - transform.position.x);
        yield return new WaitForSeconds(0.5f);

        transform.DOMoveX(transform.position.x + dir * 5f, 0.2f);

        Collider2D col = Physics2D.OverlapBox(transform.position + Vector3.right * dir * 2.5f, new Vector2(5f, 1f), 0 , 1 << 8);

        yield return new WaitForSeconds(0.1f);

        if (col != null)
            GameManager.Instance.Player.OnDamage(10, 0);
        
        yield return new WaitForSeconds(1f);
        actCoroutine = null;
    }
    public override void EnemyDead()
    {
        for (int i = -1; i <= 1; i += 2)
        {
            Poolable enemy = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Power/Non_Load/P_Mob_02_S.prefab", transform.position + transform.right * i, Quaternion.identity);
            EnemySpawnManager.Instance.curEnemies.Add(enemy);
        }

        base.EnemyDead();
    }
}
