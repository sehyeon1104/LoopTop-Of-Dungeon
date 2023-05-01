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
            Rito.Debug.Log("이동속도 10% 증가");
            GameManager.Instance.Player.playerBase.MoveSpeed *= 1.1f;
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
            Debug.Log("최대 생명력 15% 증가");
            GameManager.Instance.Player.playerBase.MaxHp *= Mathf.RoundToInt(GameManager.Instance.Player.playerBase.MaxHp * 1.5f);
        }
    }

    public class InquisitorsWatch : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
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
            Debug.Log("치명타 데미지 5% 증가");
            GameManager.Instance.Player.playerBase.Exp += 5;
        }
    }

    public class VampireFangs : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Epic;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("적 처치 시 hp 회복");

        }
    }


    public class AllRoundHalfGlove : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

        public override bool isPersitantItem => false;

        public override void Use()
        {
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
            Debug.Log("hp에 반비례하여 공격력 상승 (최대 15)");

        }
    }

    public static ItemBase[] ShopItems = new ItemBase[]
    {
        new Default(),      // 0번 아이템 ( 메꿈용 )

    };
}
