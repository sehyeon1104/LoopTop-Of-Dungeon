using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBeanBullet : ProjectileObj
{
    private float damage = 0;
    private static Transform enemyTransform = null;
    private float timer = 0f;
    private float speed = 30f;

    protected override void Init()
    {
        base.Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        timer = 0f;
        speed = 15f;

        if (enemyTransform != null && !enemyTransform.gameObject.activeSelf)
            enemyTransform = null;

        Collider2D col = Physics2D.OverlapCircle(transform.position, 10f, 1 << 9);
        if(col != null && enemyTransform == null)
        {
            enemyTransform = col.transform;
        }
        damage = GameManager.Instance.Player.playerBase.Attack * 0.1f;
        damage = Mathf.RoundToInt(damage);
    }

    protected override void Move()
    {
        if (enemyTransform == null)
            base.Move();

        else
        {
            timer += Time.deltaTime;
            speed /= (timer + 1);

            transform.Translate(Vector2.right * speed * Time.deltaTime);
            Vector3 dir = (enemyTransform.position - transform.position).normalized;
            transform.position += dir * Time.deltaTime * moveSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.GetComponent<IHittable>().OnDamage(damage);
            StartCoroutine(Pool(0));
        }
    }
}
