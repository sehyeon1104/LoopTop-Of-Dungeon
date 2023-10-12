using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Stone : EnemyDefault
{
    //아마 플레이어 발견하면 점프해서 플레이어 위치에 내려찍는 공격 쓸 듯
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
        //점프모션

        hpBar.gameObject.SetActive(false);
        isInvincible = true;
        yield return new WaitForSeconds(0.3f);
        sprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(1.5f);
        //경고 이펙트 띄우기
        Vector2 warningPos = playerTransform.position;
        Managers.Pool.PoolManaging("Assets/10.Effects/power/StoneMobwarning.prefab", warningPos, Quaternion.identity);

        yield return new WaitForSeconds(1f);
        sprite.color = new Color(1, 1, 1, 1);
        transform.position = warningPos;
        //떨어지는 이펙트(점프모션 반대로 하면 될듯)
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
