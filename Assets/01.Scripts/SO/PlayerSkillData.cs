using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO/PlayerSkillData", order = 0)]
public class PlayerSkillData : ScriptableObject
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