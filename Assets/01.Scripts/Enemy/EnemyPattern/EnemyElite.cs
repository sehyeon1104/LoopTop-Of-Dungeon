using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElite : EnemyDefault
{
    [SerializeField] private AnimationClip[] attackClips;
    protected int _count = Animator.StringToHash("Count");
    

    public override void AnimInit()
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController();

        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (idleClip != null) overrideController["Idle"] = idleClip;
        if (moveClip != null) overrideController["Move"] = moveClip;

        for(int i = 0; i < attackClips.Length; i++)
        {
           overrideController[$"Attack{i+1}"] = attackClips[i];
        }

        anim.runtimeAnimatorController = overrideController;
    }

    public override IEnumerator AttackToPlayer()
    {
        if (GameManager.Instance.Player.playerBase.IsPDead) yield break;

        int nowAttack = Random.Range(1, 4);

        anim.SetInteger(_count, nowAttack);

        anim.SetBool(_move, false);
        rigid.velocity = Vector3.zero;

        switch(nowAttack)
        {
            case 1:
                yield return StartCoroutine(Attack1());
                break;
            case 2:
                yield return StartCoroutine(Attack2());
                break;
            case 3:
                yield return StartCoroutine(Attack3());
                break;
        }

        actCoroutine = null;
    }

    protected virtual IEnumerator Attack1() { yield break; }
    protected virtual IEnumerator Attack2() { yield break; }
    protected virtual IEnumerator Attack3() { yield break; }
}
