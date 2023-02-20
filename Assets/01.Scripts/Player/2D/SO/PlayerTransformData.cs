using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO", menuName = "Create SO", order = 0)]
public class PlayerTransformData : ScriptableObject
{
    public string skill1Name = "";
    public float skill1Delay = 0f;
    public string skill2Name = "";
    public float skill2Delay = 0f;
    public string skill3Name = "";
    public float skill3Delay = 0f;
    public string skill4Name = "";
    public float skill4Delay = 0f;
    public string skill5Name = "";
    public float skill5Delay = 0f;



    public float ultiSkillDelay = 15f;

    public Sprite playerImg = null;
    public RuntimeAnimatorController playerAnim;
}