using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Rito.Debug;

public class ItemEffects : MonoBehaviour
{
    public abstract class ItemBase
    {
        public abstract Define.ItemType itemType { get; }
        public abstract void Use();
    }

    public class Default : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.Default;

        public override void Use()
        {
            Debug.Log("Default");
        }
    }

    public class Heal : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override void Use()
        {
            Debug.Log("피 1칸 회복");
            GameManager.Instance.Player.Hp += 4;
        }
    }

    public class DamageBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("데미지 1 상승");
            GameManager.Instance.Player.Damage += 1;
        }
    }

    public class CritBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("크리티컬 확률 5% 상승");
            GameManager.Instance.Player.CritChance += 5;
        }
    }

    // 테스트용
    public class TakeExp : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.Default;

        public override void Use()
        {
            Debug.Log("경험치 5 획득");
            GameManager.Instance.Player.Exp += 5;
        }
    }

    public static ItemBase[] ShopItems = new ItemBase[]
    {
        new Default(),      // 0번 아이템 ( 0번 메꿈용 )
        new Heal(),         // 1번 아이템
        new DamageBuff(),   // 2번 아이템
        new CritBuff(),     // 3번 아이템
        new TakeExp(),      // 4번 아이템 ( 디버깅용 )
    };
}
