using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWave : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        transform.localScale += new Vector3(Time.deltaTime * 10, Time.deltaTime * 10);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            collision.GetComponent<IHittable>().OnDamage(2, 0);
        }
    }
}
