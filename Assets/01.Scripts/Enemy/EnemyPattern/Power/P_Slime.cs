using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Slime : EnemyDefault
{

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
