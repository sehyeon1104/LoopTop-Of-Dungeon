using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDownEmit : MonoBehaviour
{
    float dmg;
    Collider2D circleCollider;
    private void Awake()
    {
        dmg = GameManager.Instance.Player.playerBase.Attack;
        circleCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IHittable>().OnDamage(dmg);
        }
    }
}
