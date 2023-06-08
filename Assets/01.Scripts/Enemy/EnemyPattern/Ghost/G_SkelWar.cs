using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_SkelWar : EnemyDefault
{
    [SerializeField] private GameObject ghostPrefab;
    WaitForSeconds attackWait = new WaitForSeconds(3f);


    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();

        yield return new WaitForSeconds(0.7f);

        if(distanceToPlayer <= 1.5f)
            GameManager.Instance.Player.OnDamage(damage, 0);

        yield return attackWait;

        actCoroutine = null;

    }

    public override void OnDamage(float damage, float critChance)
    {
        base.OnDamage(damage, critChance);
    }

    public override void EnemyDead()
    {
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        //Poolable enemy = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Non_Load/G_Mob_5.prefab", transform.position + transform.right * Mathf.Sign(dir.x), Quaternion.identity);
        //EnemySpawnManager.Instance.curEnemies.Add(enemy);
        base.EnemyDead();
    }
}
