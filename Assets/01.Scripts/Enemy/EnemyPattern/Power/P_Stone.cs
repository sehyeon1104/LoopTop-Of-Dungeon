using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Stone : EnemyDefault
{
    //�Ƹ� �÷��̾� �߰��ϸ� �����ؼ� �÷��̾� ��ġ�� ������� ���� �� ��

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();
        //�������

        yield return new WaitForSeconds(2f);
        //��� ����Ʈ ����

        Vector2 warningPos = playerTransform.position;
        yield return new WaitForSeconds(1f);

        //�������� ����Ʈ(������� �ݴ�� �ϸ� �ɵ�)

        Collider2D col = Physics2D.OverlapCircle(warningPos, 1.5f, 1 << 8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(damage, 0);
    }

    //������ ���� �� ������ �� �������� �ߴµ�, �÷��̾����� ���谨�� �� �� ���� �� ����
}
