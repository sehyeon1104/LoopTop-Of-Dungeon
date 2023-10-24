using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO/PlayerSkillData", order = 0)]
public class PlayerSkillData : ScriptableObject
{
    public PlayerSkillInfo[] skill;
    public Sprite playerImg = null;
    
    //public RuntimeAnimatorController playerAnim;

    public AnimationClip idlClip;
    public AnimationClip movClip;
    public AnimationClip atkClip;
    public AnimationClip dieClip;
}

[Serializable]
public class PlayerSkillInfo
{   
   public string skillName;
   public string skillExplanation;
   public Sprite[] skillIcon;
   public float skillDelay;
}