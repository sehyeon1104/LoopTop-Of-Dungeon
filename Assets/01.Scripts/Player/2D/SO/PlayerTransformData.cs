using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO", menuName = "Create SO", order = 0)]
public class PlayerTransformData : ScriptableObject
{
   
    public List<PlayerSkillInfo> skill;
    public float ultiSkillDelay = 15f;

    public Sprite playerImg = null;
    public RuntimeAnimatorController playerAnim;
}

[Serializable]
public class PlayerSkillInfo 
{   
   public string skillName;
   public float skillDelay;
}