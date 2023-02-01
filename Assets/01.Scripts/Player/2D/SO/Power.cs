using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO", menuName = "Create SO", order = 0)]
public class Power : ScriptableObject
{
    public float skill1Delay = 5f;
    public float skill2Delay = 10f;
    public float ultiSkillDelay = 15f;

    public Image playerImg = null;
}