using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_SkelMag : EnemyDefault
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject warning;
    WaitForSeconds attackWait = new WaitForSeconds(1f);
    float angle = 30f;

    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();

        Vector3 dir = playerTransform.position - transform.position;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        warning.SetActive(true);
        warning.transform.rotation = Quaternion.AngleAxis(rot + 180, Vector3.forward);
        yield return attackWait;

        for (int i = -1; i <= 1; i++)
        {
            Managers.Pool.PoolManaging("03.Prefabs/Test/Bullet", transform.position + Vector3.down * 0.35f, Quaternion.Euler(Vector3.forward * (angle * i + rot)));
        }

        yield return attackWait;
        warning.SetActive(false);

        actCoroutine = null;
    }

    public override void EnemyDead()
    {
        warning.SetActive(false);
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        //Poolable enemy = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Non_Load/G_Mob_5.prefab", transform.position + transform.right * Mathf.Sign(dir.x), Quaternion.identity);
        //EnemySpawnManager.Instance.curEnemies.Add(enemy);
        base.EnemyDead();
    }
}
