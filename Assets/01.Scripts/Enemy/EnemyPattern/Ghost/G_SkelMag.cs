using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_SkelMag : EnemyDefault
{
    [SerializeField] private GameObject ghostPrefab;
    WaitForSeconds attackWait = new WaitForSeconds(1.5f);
    float angle = 30f;

    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();

        yield return attackWait;

        Vector3 dir = playerTransform.position - transform.position;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        for (int i = -1; i <= 1; i++)
        {
            Managers.Pool.PoolManaging("03.Prefabs/Test/Bullet", transform.position, Quaternion.Euler(Vector3.forward * (angle * i + rot * 0.5f)));
        }

        yield return attackWait;

        actCoroutine = null;
    }

    public override void EnemyDead()
    {
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        GameObject enemy = Managers.Pool.PoolManaging("03.Prefabs/Enemy/Ghost/G_Mob_01", transform.position + transform.right * Mathf.Sign(dir.x), Quaternion.identity);
        enemy.transform.SetParent(null);
        EnemySpawnManager.Instance.curEnemies.Add(enemy);
        base.EnemyDead();
    }
}
