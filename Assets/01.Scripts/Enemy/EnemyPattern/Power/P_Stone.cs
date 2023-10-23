using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Stone : EnemyDefault
{
    //�Ƹ� �÷��̾� �߰��ϸ� �����ؼ� �÷��̾� ��ġ�� ������� ���� �� ��
    [SerializeField] private AnimationClip jumpDownAnim;
    [SerializeField] private GameObject crack;
    private SpriteRenderer sprite;

    public override void Init()
    {
        base.Init();
        sprite = GetComponent<SpriteRenderer>();
        crack.SetActive(false);
    }

    public override IEnumerator AttackToPlayer()
    {
        overrideController["Attack"] = attackClip;
        anim.runtimeAnimatorController = overrideController;
        yield return base.AttackToPlayer();
        //�������

        hpBar.gameObject.SetActive(false);
        isInvincible = true;
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
        hpBar.gameObject.SetActive(true);

        crack.SetActive(true);
        Collider2D col = Physics2D.OverlapCircle(warningPos, 2.5f, 1 << 8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(damage, 0);

        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
        yield return new WaitForSeconds(2f);
        crack.SetActive(false);
        actCoroutine = null;
    }
    public override void EnemyDead()
    {
        crack.SetActive(false);
        base.EnemyDead();
    }
}
