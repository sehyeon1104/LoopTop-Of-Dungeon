using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
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
        nowSpeed += nowSpeed * Time.deltaTime * 5;
        transform.Translate(Vector3.right * Time.deltaTime * nowSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHittable>().OnDamage(2, 0);
        }
    }
}
