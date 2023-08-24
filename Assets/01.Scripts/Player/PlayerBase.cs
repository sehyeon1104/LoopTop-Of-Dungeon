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
        maxHp = 100;
        hp = maxHp;
        attack = 11f;
        damage = Mathf.RoundToInt(attack * 0.6f);
        attackSpeed = 1.5f;
        moveSpeed = 5.3f;
        critChance = 5f;
        PlayerSkillNum = new int[] { 3, 4 };
        _fragmentAmount = 0;
        _bossFragmentAmount = 0;
        _fragmentAddAcq = 1f;
        playerTransformTypeFlag = Define.PlayerTransformTypeFlag.Power;
        AttackRange = 1.3f;
        InitAttackRange = AttackRange;
        PlayerTransformDataSOList = new List<PlayerSkillData>();
        PlayerTransformData = null;
        SlotLevel = new int[] { 1, 1 };
        coolDown =90;
    }

    public float AttackRange { get; set; }
    public float InitAttackRange { get; set; }
    public int[] SlotLevel { get; set; }
    public int coolDown { get; set; }
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
            recentReceiveDamage = hp - value;
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
            //UIManager.Instance.HpUpdate();
            UIManager.Instance.NewHpUpdate();
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
        get => 100;
    }

    private int recentReceiveDamage;
    public int RecentReceiveDamage { get { return recentReceiveDamage; } set { recentReceiveDamage = value; } }

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

    // 최종 데미지에 곱해지는 계수
    private float finalDamageMul;
    public float FinalDamageMul 
    {
        get
        {
            float temp = finalDamageMul;
            finalDamageMul = InitFinalDamageMul;
            return temp;
        } 
        set => finalDamageMul = value; 
    }
    public float InitFinalDamageMul { get => 1f; }

    private float attackSpeed;
    public float AttackSpeed
    {
        get => attackSpeed;
        set { 
            attackSpeed = value;
            PlayerVisual.Instance?.UpdateAttackSpeed(attackSpeed);
        }
        
    }
    public float InitAttackSpeed
    {
        get => 1.5f;
    }

    private float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
            if (moveSpeed < 1f)
                moveSpeed = 1f;
        }
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

    // 재화 추가 획득량
    private float _fragmentAddAcq;
    public float FragmentAddAcq
    {
        get { return _fragmentAddAcq; }
        set
        {
            _fragmentAddAcq = value;
        }
    }

    public float InitFragmentAddAcq
    {
        get => 1f;
    }
}
