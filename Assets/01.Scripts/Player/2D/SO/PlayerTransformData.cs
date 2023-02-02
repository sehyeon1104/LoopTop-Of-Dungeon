using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO", menuName = "Create SO", order = 0)]
public class PlayerTransformData : ScriptableObject
{
    public float skill1Delay = 5f;
    public float skill2Delay = 10f;
    public float ultiSkillDelay = 15f;

    public Sprite playerImg = null;
    public Animator playerIdleAnimator = null;
    public Animator playerAttackAnimator = null;
    public Animator playerSkillAnimator = null;
}