using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBeanBullet : ProjectileObj
{
    private float damage = 0;
    private Transform enemyTransform = null;

    protected override void Init()
    {
        base.Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        enemyTransform = null;

        Collider2D col = Physics2D.OverlapCircle(transform.position, 10f, 1 << 9);
        if(col != null)
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
            transform.position = Vector3.LerpUnclamped(transform.position, enemyTransform.position, moveSpeed * Time.deltaTime);
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
