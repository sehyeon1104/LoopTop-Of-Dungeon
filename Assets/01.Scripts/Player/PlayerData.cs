using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int hp;
    public int maxHp;
    public int level;
    public int maxLevel;
    public int[] slotLevel;
    public float attack;
    public float damage;
    public float attackSpeed;
    public float moveSpeed;
    public float critChance;
    public float exp;
    public int[] expTable;
    public int[] playerSkillNum;
    public int _fragmentAmount;
    public int bossFragmentAmount;
    public Define.PlayerTransformTypeFlag playerTransformTypeFlag;
}
