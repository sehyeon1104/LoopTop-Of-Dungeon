using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private void OnEnable()
    {
        StartCoroutine(Push());
    }
    private void Update()
    {
        transform.Translate(transform.right * Time.deltaTime * speed);
    }
    private IEnumerator Push()
    {
        yield return new WaitForSeconds(3f);
        Managers.Pool.Push(GetComponent<Poolable>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHittable>().OnDamage(1, 0);
            Managers.Sound.Play("SoundEffects/Ghost/G_Bullet.wav");
            Managers.Pool.PoolManaging("10.Effects/ghost/Boom", transform.position, Quaternion.Euler(Vector2.zero));
            Managers.Pool.Push(GetComponent<Poolable>());
        }
    }
}   
