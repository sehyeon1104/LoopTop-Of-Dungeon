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

        anim.SetInteger(_count, Random.Range(1, 4));

        anim.SetBool(_move, false);
        rigid.velocity = Vector3.zero;

        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(1.5f);
    }
}
