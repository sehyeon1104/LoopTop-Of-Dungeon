using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussianRouletteBullet : ProjectileObj
{
    private float damage = 0;

    private float playerAttack = 0f;

    protected override void Init()
    {
        base.Init();
        // TODO : 방향 지정
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerAttack = GameManager.Instance.Player.playerBase.Attack;
        damage = Random.Range(playerAttack * 0.1f, playerAttack * 2f);
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
