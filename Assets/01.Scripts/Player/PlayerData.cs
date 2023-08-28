using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int hp;
    public int maxHp;
    public int[] slotLevel;
    public float attack;
    public float damage;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public float critChance;
    public float critDamage;
    public int[] playerSkillNum;
    public int _fragmentAmount;
    public int bossFragmentAmount;
    public float fragmentAddAcq;
    public Define.PlayerTransformTypeFlag playerTransformTypeFlag;
    public int skillCoolDown;
}
