using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase
{
    // save용

    public PlayerBase()
    {
        InitPlayerStat();
    }

    public void InitPlayerStat()
    {
        
        maxHp = 12;
        hp = maxHp;
        attack = 11f;
        damage = Mathf.RoundToInt(attack * 0.6f);
        attackSpeed = 1.5f;
        moveSpeed = 5.3f;
        critChance = 5f;
        level = 1;
        maxLevel = 100;
        _expTable = new int[maxLevel];
        exp = 0;
        PlayerSkillNum = new int[] { 1, 2 };
        _fragmentAmount = 0;
        _bossFragmentAmount = 0;
        playerTransformTypeFlag = Define.PlayerTransformTypeFlag.Ghost;
        for (int i = 0; i < maxLevel; ++i)
        {
            _expTable[i] = i + 1;
        }
        AttackRange = 1.3f;
        InitAttackRange = AttackRange;
        PlayerTransformDataSOList = new List<PlayerSkillData>();
        PlayerTransformData = null;
        SlotLevel = new int[] { 1, 1 };
    }
    public float AttackRange { get; set; }
    public float InitAttackRange { get; set; }
    public int[] SlotLevel { get; set; }
    public List<PlayerSkillData> PlayerTransformDataSOList { get; set; }
    public PlayerSkillData PlayerTransformData { get; set; }

    private bool isPDead;
    public bool IsPDead { get; set; }
    private Define.PlayerTransformTypeFlag playerTransformTypeFlag;

    public Define.PlayerTransformTypeFlag PlayerTransformTypeFlag
    {
        get => playerTransformTypeFlag;
        set => playerTransformTypeFlag = value;
    }
    private int hp;
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp < 0)
            {
                hp = 0;
            }
            else if (hp > maxHp)
            {
                hp = maxHp;
            }

            GameManager.Instance.Player.HPRelatedItemEffects?.Invoke();
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
    public int InitMaxHp
    {
        get => 12;
    }

    private float attack;
    public float Attack
    {
        get => attack;
        set
        {
            attack = value;
            Damage = Mathf.CeilToInt(attack * 0.6f);

            PlayerSkill.Instance.SkillSelect(playerTransformTypeFlag);
        }
    }
    public float InitAttack
    {
        get => 11;
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
        set { 
            attackSpeed = value;
            PlayerVisual.Instance?.UpdateAttackSpeed(attackSpeed);
        }
        
    }

    private float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    public float InitMoveSpeed
    {
        get => 4.25f;
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

    public int[] PlayerSkillNum { get; set; }
 

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

    private int _bossFragmentAmount;
    public int BossFragmentAmount
    {
        get
        {
            return _bossFragmentAmount;
        }
        set
        {
            _bossFragmentAmount = value;
            if(_bossFragmentAmount < 0)
            {
                _bossFragmentAmount = 0;
            }
            // TODO : UI에 현재 남은 보스조각 개수 표기

        }
    }

}
