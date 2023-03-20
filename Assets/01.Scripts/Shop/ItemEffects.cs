using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Rito.Debug;

public class ItemEffects : MonoBehaviour
{
    private void Awake()
    {
        
    }
    public abstract class ItemBase : PlayerBase
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
            Debug.Log("�� 1ĭ ȸ��");
            GameManager.Instance.Player.pBase.Hp += 4;
        }
    }

    public class DamageBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("������ 1 ���");
            Damage += 1;
        }
    }

    public class CritBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("ũ��Ƽ�� Ȯ�� 5% ���");
            CritChance += 5;
        }
    }

    public static ItemBase[] ShopItems = new ItemBase[]
    {
        new Default(),      // 0�� ������ ( 0�� �޲޿� )
        new Heal(),         // 1�� ������
        new DamageBuff(),   // 2�� ������
        new CritBuff(),     // 3�� ������
    };
}
