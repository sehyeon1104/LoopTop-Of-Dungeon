using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Phantom : EnemyDefault
{
    [SerializeField] private GameObject ghostPrefab;
    WaitForSeconds attackWait = new WaitForSeconds(1.5f);
    SpriteRenderer spriteRenderer;

    float timer = 2.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        Vector2 dir;
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;

            if (moveClip != null) anim.SetBool(_move, true);


            dir = (playerTransform.position - transform.position).normalized;
            sprite.flipX = Mathf.Sign(dir.x) > 0 ? true : false;
            transform.Translate(dir * Time.deltaTime * speed);

            spriteRenderer.color = new Color(1, 1, 1, timer);

            yield return null;

        }

        timer = 2.5f;
        spriteRenderer.color = new Color(1, 1, 1, 1);

        dir = (transform.position - playerTransform.position).normalized;
        transform.position = playerTransform.position + transform.right * Mathf.Sign(dir.x);

        yield return base.AttackToPlayer();

        yield return attackWait;

        if (distanceToPlayer <= 1.5f)
            Player.Instance.OnDamage(damage, gameObject, 0);

        actCoroutine = null;
    }
}

