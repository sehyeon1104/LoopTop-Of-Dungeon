using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private TrailRenderer[] trail;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo());
    }

    private void OnDisable()
    {
        if (trail != null)
        {
            for(int i = 0; i < trail.Length; i++)
            {
                trail[i].Clear();
            }
        }
    }

    private IEnumerator MoveTo()
    {
        while (true)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 15)
        {
            collision.GetComponent<IHittable>().OnDamage(15, 0);
        }
    }
}
