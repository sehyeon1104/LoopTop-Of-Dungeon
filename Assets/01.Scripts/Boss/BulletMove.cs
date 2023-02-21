using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float timer = 0f;
        while (timer <= 3f)
        {
            timer += Time.deltaTime;
            Vector3 dir = Player.Instance.transform.position - transform.position;

            yield return null;

            if (timer <= 1.5f)
                transform.Translate(transform.right * Time.deltaTime * speed);
            else
                transform.Translate(dir.normalized * Time.deltaTime * speed);
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
