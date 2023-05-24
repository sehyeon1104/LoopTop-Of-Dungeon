using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPassive : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float damage = 10f;

    private Vector2 flipVector = Vector2.zero;
    private Coroutine actCoroutine = null;

    Collider2D enemy = null;

    private void LateUpdate()
    {
        FindEnemies();

        if(actCoroutine == null && enemy != null)
        {
            actCoroutine = StartCoroutine(MoveToEnemy());
        }
    }

    private void FindEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 15, 1 << 9);
        if (enemies.Length == 0) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector2 playeyToEnemyVec = enemies[i].transform.position - transform.position;

            if ((enemies[i].gameObject.activeSelf) || i == 0)
            {
                enemy = enemies[i];
                Debug.Log(enemy);
                flipVector = playeyToEnemyVec;
                Debug.Log(flipVector);
            }
        }
    }

    private IEnumerator MoveToEnemy()
    {
        while (true)
        {
            transform.Translate(flipVector.normalized * Time.deltaTime * speed);
            yield return null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 2, 1 << 9);
            if (enemies.Length == 0) return;

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i]?.GetComponent<IHittable>()?.OnDamage(damage, 0);
            }

            actCoroutine = null;
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Bullet.wav");
            Managers.Pool.PoolManaging("10.Effects/ghost/Boom", transform.position, Quaternion.Euler(Vector2.zero));
            Managers.Pool.Push(GetComponent<Poolable>());
        }
    }

}
