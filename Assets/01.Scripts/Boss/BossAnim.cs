using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    public Animator anim;
    public AnimatorOverrideController overrideController;

    [SerializeField] protected AnimationClip idleClip;
    [SerializeField] protected AnimationClip moveClip;
    [SerializeField] protected AnimationClip deathClip;

    public void Init()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        overrideController = new AnimatorOverrideController();
    }

    public void AnimInit()
    {
        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (moveClip != null) overrideController["Move"] = moveClip;
        if (idleClip != null) overrideController["Idle"] = idleClip;
        if (deathClip != null) overrideController["death"] = deathClip;

        overrideController = SetSkillAnimation(overrideController);

        anim.runtimeAnimatorController = overrideController;
    }
    public AnimatorOverrideController SetSkillAnimation(AnimatorOverrideController overrideController)
    {
        for (int i = 0; i < 5; i++)
        {
            switch (Boss.Instance.bossPattern.NowPhase)
            {
                case 1:
                    if (Boss.Instance.bossPattern.Phase_One_AnimArray[i] != null) overrideController[$"Skill{i + 1}"] = Boss.Instance.bossPattern.Phase_One_AnimArray[i];
                    if (Boss.Instance.bossPattern.Phase_One_AnimArray[5] != null) overrideController[$"SkillFinal"] = Boss.Instance.bossPattern.Phase_One_AnimArray[5];
                    break;
                case 2:
                    if (Boss.Instance.bossPattern.Phase_Two_AnimArray[i] != null) overrideController[$"Skill{i + 1}"] = Boss.Instance.bossPattern.Phase_Two_AnimArray[i];
                    if (Boss.Instance.bossPattern.Phase_Two_AnimArray[5] != null) overrideController[$"SkillFinal"] = Boss.Instance.bossPattern.Phase_Two_AnimArray[5];
                    break;
            }
        }

        return overrideController;
    }
}
