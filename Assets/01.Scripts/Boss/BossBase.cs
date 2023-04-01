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

            Boss.Instance.UpdateBossHP();
        }
    }

    public int MaxHp { get { return _maxHp; } }

    private float _shield;
    public float Shield
    {
        get
        {
            return _shield;
        }
        set
        {
            _shield = value;

            if(_shield <= 0)
            {
                Debug.Log("실드 파괴");
                // TODO : 보스 그로기 구현
            }
            if(_shield > _maxShield)
            {
                _shield = _maxShield;
            }

            Boss.Instance.UpdateBossShield();

        }
    }

    private float _maxShield;
    public float MaxShield { get { return _maxShield; } }

    private void SetBossStat()
    {
        _maxHp = 100;
        _hp = _maxHp;
        _maxShield = 50f;
    }
}
