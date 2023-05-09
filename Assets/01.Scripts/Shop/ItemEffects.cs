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
        public abstract void Use();
    }

    public class Default : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.Default;
        public override Define.ItemRating itemRating => Define.ItemRating.Default;

        public override bool isPersitantItem => false;

        public override void Use()
        {

        }
    }

    // 날개달린 신발 ( 이동속도 10% 증가 )
    public class WingShoes : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("날개달린 신발 효과 발동");
            Rito.Debug.Log("이동속도 10% 증가");
            GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 1.1f;
        }
    }

    public class GiantGlove : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("거인의 장갑 효과 발동");
            Debug.Log("공격범위 10% 증가");
        }
    }

    // 무뎌진 검 ( 공격력 5% 증가 )
    public class DullSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("무뎌진 검 효과 발동");
            Debug.Log("공격력 5% 증가");
            GameManager.Instance.Player.playerBase.Attack *= 1.05f;
        }
    }

    public class ChampionBelt : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("헤비급 챔피언의 벨트 효과 발동");
            Debug.Log("최대 생명력 15% 증가");
            int incQuantity = Mathf.RoundToInt(GameManager.Instance.Player.playerBase.InitMaxHp * 0.15f);
            Debug.Log(incQuantity);
            GameManager.Instance.Player.playerBase.MaxHp += incQuantity;
            GameManager.Instance.Player.playerBase.Hp += incQuantity;
        }
    }

    public class TornPaper : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("찢어진 종이 효과 발동");
            Debug.Log("대쉬 쿨타임 0.5초 감소");
        }
    }

    public class InquisitorsRing : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("탐험가의 반지 효과 발동");
            Debug.Log("치명타 확률 6% 증가");
            GameManager.Instance.Player.playerBase.CritChance += 6;
        }
    }

    // 날카로운 검 (치명타 데미지 5% 상승 )
    public class SharpSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("날카로운 검 효과 발동");
            Debug.Log("스킬 쿨타임 5% 감소");
            // TODO : 스킬 쿨타임 감소 구현
            //GameManager.Instance.Player.playerBase. += 5;
        }
    }

    public class VampireFangs : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Epic;

        public override bool isPersitantItem => true;

        public override void Use()
        {
            Debug.Log("뱀파이어의 송곳니 효과 발동");
            Debug.Log("적 처치 시 hp 1 회복");

        }

        public void VampireFangsEffect()
        {

        }
    }


    public class AllRoundHalfGlove : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

        public override bool isPersitantItem => false;

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

        int temp = 0;
        int rise = 0;

        public override void Use()
        {
            Debug.Log("광전사의 검 효과 발동");
            Debug.Log("hp에 반비례하여 공격력 상승 (최대 15)");
            BerserkerSwordEffect();
        }

        private static float lastRise = 0;

        public void BerserkerSwordEffect()
        {
            GameManager.Instance.Player.playerBase.Attack -= lastRise;
            temp = GameManager.Instance.Player.playerBase.Hp;
            rise = 0;
            while (temp < GameManager.Instance.Player.playerBase.MaxHp || rise < 15)
            {
                temp += 10;
                if (temp < GameManager.Instance.Player.playerBase.MaxHp)
                {
                    rise++;
                }
                else
                {
                    break;
                }
            }

            lastRise = rise;
            GameManager.Instance.Player.playerBase.Attack += rise;
            Debug.Log("Player Attack : " + GameManager.Instance.Player.playerBase.Attack);
        }
    }

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
    };
}
