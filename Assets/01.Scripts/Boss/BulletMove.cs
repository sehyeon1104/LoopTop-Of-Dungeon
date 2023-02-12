using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        transform.Translate(transform.right * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHittable>().OnDamage(1);
            //IHittable hittable= collision.GetComponent<IHittable>();
            //hittable.OnDamage(1);
        }
    }
}   
