using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSlash : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float nowSpeed;
    private void Awake()
    {
        nowSpeed = speed;
    }
    private void OnEnable()
    {
        nowSpeed = speed;
    }
    private void Update()
    {
        nowSpeed += nowSpeed * Time.deltaTime * 7.5f;
        transform.Translate(Vector3.right * Time.deltaTime * nowSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            collision.GetComponent<IHittable>().OnDamage(10, 0);
        }
    }
}
