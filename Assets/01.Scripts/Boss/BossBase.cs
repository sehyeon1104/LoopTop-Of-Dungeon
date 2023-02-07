using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase
{
    public BossBase()
    {
        SetBossStat();
    }

    private int _hp;
    private int _maxHp;

    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            if(_hp > _maxHp)
                _hp = _maxHp;
            else if(_hp < 0)
                _hp = 0;

        }
    }

    public int MaxHp { get { return _maxHp; } }

    private void SetBossStat()
    {
        _maxHp = 100;
        _hp = _maxHp;
    }
}
