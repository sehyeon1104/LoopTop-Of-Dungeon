using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase
{
    private int _hp;
    private int _maxHp;
    private float _damage;
    public PlayerBase()
    {
        SetPlayerStat();
    }

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

    public int MaxHp { get { return _maxHp; } }

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
    public void SetPlayerStat()
    {
        _maxHp = 3;
        _hp = _maxHp;
        _damage = 5f;
    }
}
