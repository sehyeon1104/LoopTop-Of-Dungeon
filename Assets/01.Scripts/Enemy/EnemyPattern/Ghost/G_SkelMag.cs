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

        Vector3 dir = playerTransform.position - transform.position;
        yield return attackWait;

        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        for (int i = -1; i <= 1; i++)
        {
            Managers.Pool.PoolManaging("03.Prefabs/Test/Bullet", transform.position, Quaternion.Euler(Vector3.forward * (angle * i + rot)));
        }

        yield return attackWait;

        actCoroutine = null;
    }

    public override void EnemyDead()
    {
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        Poolable enemy = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Non_Load/G_Mob_5.prefab", transform.position + transform.right * Mathf.Sign(dir.x), Quaternion.identity);
        EnemySpawnManager.Instance.curEnemies.Add(enemy);
        base.EnemyDead();
    }
}
