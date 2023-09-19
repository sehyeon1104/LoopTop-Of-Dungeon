using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_StoneGolem : EnemyDefault
{
    [SerializeField] GameObject warning;
    //얘는 돌진 느낌으로 하면 좋을 것 같다, 넉백 세게?

    public override IEnumerator AttackToPlayer()
    {
        warning.SetActive(false);
        yield return new WaitForSeconds(1f);

        if (GameManager.Instance.Player.playerBase.IsPDead) yield break;

        anim.SetBool(_move, false);
        if (rigid != null)
            rigid.velocity = Vector3.zero;

        Vector3 dir = (playerTransform.position - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        warning.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        warning.SetActive(true);

        //dir 방향으로 경고 표시 회전
        yield return new WaitForSeconds(1f);

        if (attackClip != null) anim.SetTrigger(_attack);
        rigid.DOMove(transform.position + dir * 7f, 0.1f);

        Collider2D col = Physics2D.OverlapBox(transform.position + dir * 3.5f, new Vector2(1.5f, 7f), angle, 1 << 8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(20, 0);

        yield return new WaitForSeconds(1f);
        warning.SetActive(false);
        yield return new WaitForSeconds(1f);

        actCoroutine = null;
    }

    public override void EnemyDead()
    {
        warning.SetActive(false);
        base.EnemyDead();
    }
}
