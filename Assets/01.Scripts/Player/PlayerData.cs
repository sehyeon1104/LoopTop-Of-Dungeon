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
    public float damage;
    public float critChance;
    public float exp;
    public int[] expTable;
    public int _fragmentAmount;
    public Define.PlayerTransformTypeFlag playerTransformTypeFlag;
}
