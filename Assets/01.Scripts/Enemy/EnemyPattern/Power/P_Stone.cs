using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Stone : EnemyDefault
{
    //�Ƹ� �÷��̾� �߰��ϸ� �����ؼ� �÷��̾� ��ġ�� ������� ���� �� ��
    [SerializeField] private AnimationClip jumpDownAnim;
    private SpriteRenderer sprite;

    public override void Init()
    {
        base.Init();
        sprite = GetComponent<SpriteRenderer>();
    }

    public override IEnumerator AttackToPlayer()
    {
        overrideController["Attack"] = attackClip;
        anim.runtimeAnimatorController = overrideController;
        yield return base.AttackToPlayer();
        //�������
        yield return new WaitForSeconds(0.3f);
        sprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(1.5f);
        //��� ����Ʈ ����
        Vector2 warningPos = playerTransform.position;
        Managers.Pool.PoolManaging("Assets/10.Effects/power/StoneMobwarning.prefab", warningPos, Quaternion.identity);

        yield return new WaitForSeconds(1f);
        sprite.color = new Color(1, 1, 1, 1);
        transform.position = warningPos;
        //�������� ����Ʈ(������� �ݴ�� �ϸ� �ɵ�)
        overrideController["Attack"] = jumpDownAnim;
        anim.runtimeAnimatorController = overrideController;
        anim.SetTrigger(_attack);

        Collider2D col = Physics2D.OverlapCircle(warningPos, 1.5f, 1 << 8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(damage, 0);

        yield return new WaitForSeconds(3f);
        actCoroutine = null;
    }
}
