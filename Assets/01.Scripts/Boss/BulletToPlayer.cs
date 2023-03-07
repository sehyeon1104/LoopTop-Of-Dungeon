using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletToPlayer : MonoBehaviour
{
    private float speed = 0f;

    void OnEnable()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float timer = 0f;
        float dist = Vector2.Distance(transform.position, Player.Instance.transform.position);
        while (timer <= 3f)
        {
            timer += Time.deltaTime;

            yield return null;
            speed = Time.deltaTime * 2.5f;

            if (timer <= 1.5f)
                transform.Translate(transform.right * speed);
            else
            {
                speed += Time.deltaTime;
                transform.position = Vector3.LerpUnclamped(transform.position, Player.Instance.transform.position, speed);
            }
        }
        Managers.Pool.Push(GetComponent<Poolable>());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHittable>().OnDamage(2, gameObject, 0);
            Managers.Sound.Play("SoundEffects/Ghost/G_Bullet.wav");
            Managers.Pool.PoolManaging("10.Effects/ghost/Boom", transform.position, Quaternion.Euler(Vector2.zero));
            Managers.Pool.Push(GetComponent<Poolable>());
        }
    }
}
