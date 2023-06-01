using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Rito.Debug;

public class ItemEffects : MonoBehaviour
{
    public abstract class ItemBase
    {
        public abstract Define.ItemType itemType { get; }
        public abstract Define.ItemRating itemRating { get; }
        public abstract bool isPersitantItem { get; }
        public abstract void Init();
        public abstract void Use();
        public virtual void LastingEffect()
        {

        }
        public virtual bool isOneOff { get; } = false;
    }

    public class Default : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.Default;
        public override Define.ItemRating itemRating => Define.ItemRating.Default;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {

        }
    }

    #region Common

    // 무뎌진 검 (공격력 5% 증가)
    public class DullSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("무뎌진 검 효과 발동");
            Debug.Log("공격력 5% 증가");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.05f;
        }
    }

    // 날개달린 신발 ( 이동속도 10% 증가 )
    public class WingShoes : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("날개달린 신발 효과 발동");
            Rito.Debug.Log("이동속도 10% 증가");
            GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.1f;
        }
    }

    // 거인의 장갑 ( 공격범위 10% 증가 )
    public class GiantGlove : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("거인의 장갑 효과 발동");
            Debug.Log("공격범위 10% 증가");
            GameManager.Instance.Player.playerBase.AttackRange = GameManager.Instance.Player.playerBase.InitAttackRange * 0.1f;
        }
    }

    #endregion

    #region Rare
    public class ChampionBelt : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("헤비급 챔피언의 벨트 효과 발동");
            Debug.Log("하트 1/4칸 증가");
            int incQuantity = 1; //Mathf.RoundToInt(GameManager.Instance.Player.playerBase.InitMaxHp * 0.15f);
            GameManager.Instance.Player.playerBase.MaxHp += incQuantity;
            GameManager.Instance.Player.playerBase.Hp += incQuantity;
            UIManager.Instance.UpdateUI();
        }
    }

    public class TornPaper : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("찢어진 종이 효과 발동");
            Debug.Log("대쉬 쿨타임 10% 감소");
        }
    }

    public class InquisitorsRing : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("탐험가의 반지 효과 발동");
            Debug.Log("치명타 확률 6% 증가");
            GameManager.Instance.Player.playerBase.CritChance += 6;
        }
    }

    // 날카로운 검 (스킬 쿨타임 5% 감소 )
    public class SharpSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("날카로운 검 효과 발동");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.1f;
            //Debug.Log("스킬 쿨타임 5% 감소");
            // TODO : 스킬 쿨타임 감소 구현
            //GameManager.Instance.Player.playerBase. += 5;
        }
    }

    #endregion

    #region Epic
    public class VampireFangs : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Epic;

        public override bool isPersitantItem => true;

        private float probabilityChance = 1f;

        public override void Init()
        {

        }

        public override void Use()
        {
            // TODO : 적 처치시 hp 1 회복 구현
            Debug.Log("뱀파이어의 송곳니 효과 발동");
            Debug.Log("적 공격시 1% 확률로 hp 1 회복");
            //Debug.Log("적 처치 시 hp 1 회복");

            GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(VampireFangsEffect);
            GameManager.Instance.Player.AttackRelatedItemEffects.AddListener(VampireFangsEffect);
        }

        public override void LastingEffect()
        {
            GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(VampireFangsEffect);
            GameManager.Instance.Player.AttackRelatedItemEffects.AddListener(VampireFangsEffect);
        }

        public void VampireFangsEffect()
        {
            // 적 공격시 1% 확률로 hp 1 회복
            if(Random.Range(0, 100) < probabilityChance)
            {
                GameManager.Instance.Player.playerBase.Hp += 1;
            }

        }
    }

    #endregion

    #region Legendary
    public class AllRoundHalfGlove : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("만능 하프글러브 효과 발동");
            Debug.Log("공격력 15% 증가 및 스킬 쿨타임 15% 감소");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.15f;
            // TODO : 스킬쿨타임 감소
        }
    }

    public class BerserkerSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

        public override bool isPersitantItem => true;

        float temp = 0;
        int rise = 0;
        int totalRise = 0;

        int maxIncrease = 15;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("광전사의 검 효과 발동");
            Debug.Log("hp에 반비례하여 공격력 상승 (최대 15)");
            BerserkerSwordEffect();
            GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(BerserkerSwordEffect);
            GameManager.Instance.Player.HPRelatedItemEffects.AddListener(BerserkerSwordEffect);
        }

        public override void LastingEffect()
        {
            GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(BerserkerSwordEffect);
            GameManager.Instance.Player.HPRelatedItemEffects.AddListener(BerserkerSwordEffect);
        }

        // 오류가 유발될 수 있음. 예의주시할것
        private static float lastRise = 0;

        public void BerserkerSwordEffect()
        {
            GameManager.Instance.Player.playerBase.Attack -= lastRise;
            temp = GameManager.Instance.Player.playerBase.Hp;
            rise = maxIncrease / GameManager.Instance.Player.playerBase.MaxHp;
            totalRise = 0;

            while (temp < GameManager.Instance.Player.playerBase.MaxHp && rise < 15)
            {
                if (temp < GameManager.Instance.Player.playerBase.MaxHp)
                {
                    totalRise += rise;
                }
                else
                {
                    break;
                }
                temp++;
            }

            lastRise = totalRise;
            GameManager.Instance.Player.playerBase.Attack += totalRise;
            Debug.Log("Player Attack : " + GameManager.Instance.Player.playerBase.Attack);
        }

    }

    #endregion

    #region ETC

    public class SmallHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("hp포션 (소) 사용");
            Debug.Log("체력 1/2칸 회복");
            GameManager.Instance.Player.playerBase.Hp += 2;
        }
    }

    public class MediumHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("hp포션 (중) 사용");
            Debug.Log("체력 1칸 회복");
            GameManager.Instance.Player.playerBase.Hp += 4;
        }
    }

    public class LargeHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("hp포션 (대) 사용");
            Debug.Log("체력 2칸 회복");
            GameManager.Instance.Player.playerBase.Hp += 8;
        }
    }

    public class ExtraLargeHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("hp포션 (특대) 사용");
            Debug.Log("체력 3칸 회복");
            GameManager.Instance.Player.playerBase.Hp += 12;
        }
    }

    #endregion

    #region Special (BrokenItem)

    public class NailedShoes : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("못박힌 신발 효과 발동");
            Debug.Log("공격력 30% 증가, 이동속도 25% 감소");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.3f;
            GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.25f;
        }
    }

    public class CloudyGlasses : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("흐린 안경 효과 발동");
            Debug.Log("이동속도 20% 증가, 치명타 확률 8% 감소");
            GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.2f;
            GameManager.Instance.Player.playerBase.CritChance -= 8f;
        }
    }

    public class TurtleHat : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {
            Debug.Log("거북 모자 효과 발동");
            Debug.Log("하트 1칸 증가, 공격력 15% 감소");
            GameManager.Instance.Player.playerBase.MaxHp += 4; //(int)(GameManager.Instance.Player.playerBase.InitMaxHp * 0.3f);
            GameManager.Instance.Player.playerBase.Hp += 4;
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.15f;
            UIManager.Instance.MaxHpUpdate();
        }
    }

    public class CursedRing : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => true;

        private static bool isFirst = false;

        public override void Init()
        {
            isFirst = false;
        }

        public override void Use()
        {
            Debug.Log("저주받은 반지 효과 발동");
            Debug.Log("공격력 60% 증가, 받는 데미지 2배 증가");
            if (!isFirst)
            {
                Debug.Log("공격력 증가");
                isFirst = true;
                GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.6f;
            }
            GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(CursedRingEffect);
            GameManager.Instance.Player.OnDamagedRelatedItemEffects.AddListener(CursedRingEffect);

        }

        public override void LastingEffect()
        {
            GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(CursedRingEffect);
            GameManager.Instance.Player.OnDamagedRelatedItemEffects.AddListener(CursedRingEffect);
        }

        private void CursedRingEffect()
        {
            GameManager.Instance.Player.DamageMultiples = 2;
        }

    }

    #endregion

    public static ItemBase[] Items = new ItemBase[]
    {
        new Default(),      // 0번 아이템 ( 효과x )

        // common
        new WingShoes(),
        new GiantGlove(),
        new DullSword(),

        // rare
        new ChampionBelt(),
        new TornPaper(),
        new InquisitorsRing(),
        new SharpSword(),

        // epic
        new VampireFangs(),
        
        // legendary
        new AllRoundHalfGlove(),
        new BerserkerSword(),

        // ETC
        new SmallHpPotion(),
        new MediumHpPotion(),
        new LargeHpPotion(),
        new ExtraLargeHpPotion(),

        // Special
        new NailedShoes(),
        new CloudyGlasses(),
        new TurtleHat(),
        new CursedRing(),
    };
}
