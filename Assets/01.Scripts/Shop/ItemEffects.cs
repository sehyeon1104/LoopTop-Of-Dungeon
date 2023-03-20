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

    public class Heal : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override void Use()
        {
            Debug.Log("ÇÇ 1Ä­ È¸º¹");
            GameManager.Instance.Player.pBase.Hp += 4;
        }
    }

    public class DamageBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("µ¥¹ÌÁö 1 »ó½Â");
            GameManager.Instance.Player.pBase.Damage += 1;
        }
    }

    public class CritBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("Å©¸®Æ¼ÄÃ È®·ü 5% »ó½Â");
            GameManager.Instance.Player.pBase.CritChance += 5;
        }
    }

    public ItemBase[] items = new ItemBase[]
    {
        new Heal(),
        new DamageBuff(),
        new CritBuff(),
    };
}
