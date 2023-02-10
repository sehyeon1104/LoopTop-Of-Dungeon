using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase
{
    public PlayerBase()
    {
        SetPlayerStat();
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
            if(_hp < 0)
            {
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

    public void SetPlayerStat()
    {
        _maxHp = 3;
        _hp = _maxHp;
        _damage = 5f;
        _critChance = 5f;
    }
}
