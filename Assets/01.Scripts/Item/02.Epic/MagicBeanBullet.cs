using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBeanBullet : ProjectileObj
{
    private float damage = 0;

    private float playerAttack = 0f;

    protected override void Init()
    {
        base.Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        damage = GameManager.Instance.Player.playerBase.Attack * 0.1f;
        damage = Mathf.RoundToInt(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IHittable>().OnDamage(damage);
            Pool();
        }
    }
}
