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
            Debug.Log("�� 1ĭ ȸ��");
            GameManager.Instance.Player.Hp += 4;
        }
    }

    public class DamageBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("������ 1 ���");
            GameManager.Instance.Player.Damage += 1;
        }
    }

    public class CritBuff : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;

        public override void Use()
        {
            Debug.Log("ũ��Ƽ�� Ȯ�� 5% ���");
            GameManager.Instance.Player.CritChance += 5;
        }
    }

    // �׽�Ʈ��
    public class TakeExp : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.Default;

        public override void Use()
        {
            Debug.Log("����ġ 5 ȹ��");
            GameManager.Instance.Player.Exp += 5;
        }
    }

    public static ItemBase[] ShopItems = new ItemBase[]
    {
        new Default(),      // 0�� ������ ( 0�� �޲޿� )
        new Heal(),         // 1�� ������
        new DamageBuff(),   // 2�� ������
        new CritBuff(),     // 3�� ������
        new TakeExp(),      // 4�� ������ ( ������ )
    };
}
