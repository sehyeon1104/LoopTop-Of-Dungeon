using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase
{
    public PlayerBase()
    {
        SetPlayerStat();
    }

    private Define.PlayerTransformTypeFlag _playerTransformTypeFlag;

    public Define.PlayerTransformTypeFlag PlayerTransformTypeFlag
    {
        get
        {
            return _playerTransformTypeFlag;
        }
        set
        {
            _playerTransformTypeFlag = value;
        }
    }
    private int _hp;
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if(_hp <= 0)
            {
                Player.Instance.OnDie.Invoke();
                _hp = 0;
            }
            else if(_hp > _maxHp)
            {
                _hp = _maxHp;
            }
        }
    }

    private int _maxHp;
    public int MaxHp { get { return _maxHp; } }

    private float _damage;
    public float Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }

    private float _critChance;
    public float CritChance
    {
        get
        {
            return _critChance;
        }
        set
        {
            _critChance = value;

            if(_critChance > 100)
            {
                _critChance = 100;
            }
            else if(_critChance < 0)
            {
                _critChance = 0;
            }

        }
    }

    private int[] _expTable;

    private float _exp;
    public float Exp
    {
        get 
        { 
            return _exp; 
        }
        set
        {
            _exp = value;

            if(_level >= MaxLevel || _exp < 0)
            {
                return;
            }

            if(_expTable[_level] <= _exp)
            {
                Debug.Log("Level : " + Level);
                Debug.Log($"Current Exp : {_exp}");
                Exp = _exp - _expTable[_level++];
            }

            UIManager.Instance.UpdateUI();
        }
    }

    private int _level;
    private int Level
    {
        get
        {
            return _level;
        }
    }

    private int _maxLevel;
    public int MaxLevel
    {
        get
        {
            return _maxLevel;
        }
    }

    public void SetPlayerStat()
    {
        _maxHp = 12;
        _hp = _maxHp;
        _damage = 5f;
        _critChance = 5f;

        _level = 1;
        _maxLevel = 100;
        _expTable = new int[_maxLevel];
        _exp = 0;

        _playerTransformTypeFlag = Define.PlayerTransformTypeFlag.Power;

        for(int i = 0; i < _maxLevel; ++i)
        {
            _expTable[i] = i + 1;
        }
    }
}
