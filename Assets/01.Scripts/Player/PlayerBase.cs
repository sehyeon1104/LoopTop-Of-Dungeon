using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase
{
    // save��

    public PlayerBase()
    {
        InitPlayerStat();
    }

    public void InitPlayerStat()
    {
        maxHp = 12;
        hp = maxHp;
        damage = 13f;
        attackSpeed = 0.3f;
        critChance = 5f;
        level = 1;
        maxLevel = 100;
        _expTable = new int[maxLevel];
        exp = 0;
        _fragmentAmount = 0;
        playerTransformTypeFlag = Define.PlayerTransformTypeFlag.Power;
        for (int i = 0; i < maxLevel; ++i)
        {
            _expTable[i] = i + 1;
        }

        PlayerTransformDataSOList = new List<PlayerSkillData>();
        PlayerTransformData = null;
    }

    public List<PlayerSkillData> PlayerTransformDataSOList { get; set; }
    public PlayerSkillData PlayerTransformData { get; set; }

    private bool isPDead;
    public bool IsPDead { get; set; }
    private Define.PlayerTransformTypeFlag playerTransformTypeFlag;

    public Define.PlayerTransformTypeFlag PlayerTransformTypeFlag { get; set; }

    private int hp;
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
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
    public int MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
        }
    }

    private float damage;
    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    private float attackSpeed;
    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = value;
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

    public int[] ExpTable
    {
        get
        {
            return _expTable;
        }
        set
        {
            _expTable = value;
        }
    }

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

            // UIManager.Instance.UpdateUI();
        }
    }

    private int level;
    public int Level
    {
        get => level;
        set => level = value;
    }

    private int maxLevel;
    public int MaxLevel 
    {  
        get 
        { 
            return maxLevel; 
        } 
        set 
        { 
            maxLevel = value; 
        } 
    }

    private int _fragmentAmount;
    public int FragmentAmount
    {
        get
        {
            return _fragmentAmount;
        }
        set
        {
            _fragmentAmount = value;
            if (_fragmentAmount <= 0)
            {
                _fragmentAmount = 0;
            }

            UIManager.Instance.UpdateGoods();
        }
    }

}
