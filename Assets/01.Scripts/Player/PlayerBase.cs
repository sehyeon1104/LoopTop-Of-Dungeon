using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase
{
    public void SetPlayerStat()
    {
        // TODO : JSON 파일 내에 있는 플레이어 정보 불러오기

        maxHp = 12;
        hp = maxHp;
        damage = 5f;
        critChance = 5f;
        level = 1;
        maxLevel = 100;
        _expTable = new int[maxLevel];
        exp = 0;
        _fragmentAmount = PlayerPrefs.GetInt("PlayerFragmentAmount");
        _playerTransformTypeFlag = Define.PlayerTransformTypeFlag.Power;
        for (int i = 0; i < maxLevel; ++i)
        {
            _expTable[i] = i + 1;
        }
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        playerTransformDataSOArr.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));
        playerTransformDataSO = playerTransformDataSOArr[0];
    }
        List<PlayerSkillData> playerTransformDataSOArr = new List<PlayerSkillData>();
    public List<PlayerSkillData> PlayerTransformDataSOArr() => playerTransformDataSOArr;
    PlayerSkillData playerTransformDataSO;
    public PlayerSkillData PlayerTransformDataSO => playerTransformDataSO;

    private bool isPDead;
    public bool IsPDead { get; set; }
    private Define.PlayerTransformTypeFlag _playerTransformTypeFlag;

    public Define.PlayerTransformTypeFlag PlayerTransformTypeFlag
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
    public int MaxHp => maxHp;

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

            PlayerPrefs.SetInt("PlayerFragmentAmount", _fragmentAmount);
            UIManager.Instance.UpdateGoods();
        }
    }

}
