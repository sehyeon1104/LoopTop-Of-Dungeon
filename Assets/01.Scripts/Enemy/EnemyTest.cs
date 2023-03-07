using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : EnemyDefault
{
    private void OnEnable()
    {
        hp = 5;
        damage = 1;
    }

    public override void MoveToPlayer()
    {
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime);
    }

    public override void AttackToPlayer()
    {
        if (Player.Instance.isPDead)
            return;

        Player.Instance.OnDamage(1, gameObject, 0);     
    }

    public override void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
            StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject, true));
        }
        
        hp -= (int)damage;
        StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject));
        if(hp <= 0)
        {
            EnemyDead();
        }
    }

    public override void EnemyDead()
    {
        if (transform.parent != null)
            EnemySpawnManager.Instance.RemoveEnemyInList(gameObject);

        gameObject.SetActive(false);
    }
}
