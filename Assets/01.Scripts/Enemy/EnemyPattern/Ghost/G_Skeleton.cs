using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Skeleton : EnemyDefault
{
    [SerializeField] private GameObject ghostPrefab;
    WaitForSeconds attackWait = new WaitForSeconds(2f);
    WaitForSeconds waitTime = new WaitForSeconds(0.5f);

    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();

        yield return waitTime;

        Collider2D col = Physics2D.OverlapCircle(transform.position, 1, 1 << 8);
        if(col != null)
            GameManager.Instance.Player.OnDamage(damage, 0);

        yield return attackWait;

        actCoroutine = null;

    }

    public override void OnDamage(float damage, float critChance, Poolable hitEffect = null)
    {
        base.OnDamage(damage ,critChance);
    }

    public override void EnemyDead()
    {
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        if(GameManager.Instance.sceneType == Define.Scene.Field)
        {
            //Poolable enemy = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Non_Load/G_Mob_5.prefab", transform.position + transform.right * Mathf.Sign(dir.x), Quaternion.identity);
            //EnemySpawnManager.Instance.curEnemies.Add(enemy);
        }
        base.EnemyDead();
    }

}
