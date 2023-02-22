using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletToPlayer : MonoBehaviour
{
    private float speed = 0f;

    void Start()
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
                float t = speed / dist;

                transform.position = Vector3.LerpUnclamped(transform.position, Player.Instance.transform.position, t);
            }
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
        }
    }
}
