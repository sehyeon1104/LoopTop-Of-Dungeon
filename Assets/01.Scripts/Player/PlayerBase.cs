using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private void Start()
    {
        SetPlayerStat();
    }
    private PlayerSkillData skillData;
    public PlayerSkillData SkillData { get; set; }

    private bool isPDead;
    public bool IsPDead { get; set; }
    private Define.PlayerTransformTypeFlag _playerTransformTypeFlag;

    protected Define.PlayerTransformTypeFlag PlayerTransformTypeFlag
    {
        get => _playerTransformTypeFlag;
        set => _playerTransformTypeFlag = value;

    }
    private int hp;
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                GameManager.Instance.Player.OnDie.Invoke();
                hp = 0;
            }
            else if (hp > maxHp)
            {
                hp = maxHp;
            }
            UIManager.Instance.HpUpdate();
        }
    }

    private int maxHp;
    protected int MaxHp => maxHp;

    private float damage;
    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    private float critChance;
    public float CritChance
    {
        get => critChance;
        set
        {
            critChance = value;

            if (critChance > 100)
            {
                critChance = 100;
            }
            else if (critChance < 0)
            {
                critChance = 0;
            }

        }
    }

    private int[] _expTable;

    private float exp;
    public float Exp
    {
        get => exp;
        set
        {
            exp = value;

            if (level >= MaxLevel || exp < 0)
            {
                return;
            }

            if (_expTable[level] <= exp)
            {
                Debug.Log("Level : " + Level);
                Debug.Log($"Current Exp : {exp}");
                Exp = exp - _expTable[level++];
            }

            UIManager.Instance.UpdateUI();
        }
    }

    private int level;
    private int Level
    {
        get => level;
    }

    private int maxLevel;
    public int MaxLevel
    {
        get
        {
            return maxLevel;
        }
    }

    protected void SetPlayerStat()
    {
        maxHp = 12;
        hp = maxHp;
        damage = 5f;
        critChance = 5f;
        
        level = 1;
        maxLevel = 100;
        _expTable = new int[maxLevel];
        exp = 0;

        _playerTransformTypeFlag = Define.PlayerTransformTypeFlag.Power;

        for (int i = 0; i < maxLevel; ++i)
        {
            _expTable[i] = i + 1;
        }
    }
}
