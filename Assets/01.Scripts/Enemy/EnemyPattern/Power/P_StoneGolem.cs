using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_StoneGolem : EnemyDefault
{
    //��� ���� �������� �ϸ� ���� �� ����, �˹� ����?

    public override IEnumerator AttackToPlayer()
    {
        if (GameManager.Instance.Player.playerBase.IsPDead) yield break;

        anim.SetBool(_move, false);
        if (rigid != null)
            rigid.velocity = Vector3.zero;

        Vector2 dir = (playerTransform.position - transform.position).normalized;
        //dir �������� ��� ǥ�� ȸ��
        yield return new WaitForSeconds(2f);

        float timer = 0f;

        if (attackClip != null) anim.SetTrigger(_attack);
        while(timer < 1f)
        {
            timer += 0.01f;
            rigid.velocity = dir * speed;

            if(timer % 0.1f == 0)
            {
                Collider2D col = Physics2D.OverlapCircle(transform.position, 1f, 1 << 8);
                if (col != null)
                {
                    GameManager.Instance.Player.OnDamage(damage, 0);
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2f);
    }
}
