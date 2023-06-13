using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Stone : EnemyDefault
{
    //아마 플레이어 발견하면 점프해서 플레이어 위치에 내려찍는 공격 쓸 듯

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();
        //점프모션

        yield return new WaitForSeconds(2f);
        //경고 이펙트 띄우기

        Vector2 warningPos = playerTransform.position;
        yield return new WaitForSeconds(1f);

        //떨어지는 이펙트(점프모션 반대로 하면 될듯)

        Collider2D col = Physics2D.OverlapCircle(warningPos, 1.5f, 1 << 8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(damage, 0);
    }

    //원래는 죽을 때 터지는 걸 넣으려고 했는데, 플레이어한테 불쾌감을 줄 수 있을 것 같음
}
