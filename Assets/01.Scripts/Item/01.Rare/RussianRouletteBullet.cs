using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RussianRouletteBullet : ProjectileObj
{
    private float damage = 0;
    private float playerAttack = 0f;
    private bool isStronger = false;
    private Material mat;

    protected override void Init()
    {
        mat = GetComponent<SpriteRenderer>().material;
        isStronger = false;
        base.Init();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isStronger = false;
        playerAttack = GameManager.Instance.Player.playerBase.Attack;
        damage = Random.Range(playerAttack * 0.1f, playerAttack * 2f);
        damage = Mathf.RoundToInt(damage);

        if (damage >= playerAttack * 1.5f)
        {
            mat.SetColor("_SetColor", new Color(375f, 50f, 0f));
            transform.localScale = Vector3.one;
            isStronger = true;
        }
        else
        {
            mat.SetColor("_SetColor", new Color(0.75f, 0.75f, 0.75f));
            transform.localScale = Vector3.one * 0.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.GetComponent<IHittable>().OnDamage(damage);
            if(!isStronger)
                StartCoroutine(Pool(0));
        }
    }
}
