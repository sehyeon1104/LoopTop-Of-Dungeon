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

    // �����޸� �Ź� ( �̵��ӵ� 10% ���� )
    public class WingShoes : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Rito.Debug.Log("�̵��ӵ� 10% ����");
            GameManager.Instance.Player.playerBase.MoveSpeed *= 1.1f;
        }
    }

    public class GiantGlove : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("���ݹ��� 10% ����");
        }
    }
    
    // ������ �� ( ���ݷ� 5% ���� )
    public class DullSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("���ݷ� 5% ����");
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
            Debug.Log("�ִ� ����� 15% ����");
            GameManager.Instance.Player.playerBase.MaxHp *= Mathf.RoundToInt(GameManager.Instance.Player.playerBase.MaxHp * 1.5f);
        }
    }

    public class YoumuTail : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("�뽬 ��Ÿ�� 0.5�� ����");
        }
    }

    public class InquisitorsWatch : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("ġ��Ÿ Ȯ�� 6% ����");
            GameManager.Instance.Player.playerBase.CritChance += 6;
        }
    }

    // ��ī�ο� �� (ġ��Ÿ ������ 5% ��� )
    public class SharpSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("ġ��Ÿ ������ 5% ����");
            GameManager.Instance.Player.playerBase.Exp += 5;
        }
    }

    public class VampireFangs : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Epic;

        public override bool isPersitantItem => true;

        public override void Use()
        {
            Debug.Log("�� óġ �� hp 1 ȸ��");

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
            Debug.Log("���ݷ� 15% ���� �� ��ų ��Ÿ�� 15% ����");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.15f;
            // TODO : ��ų��Ÿ�� ����
        }
    }

    public class BerserkerSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

        public override bool isPersitantItem => true;

        int temp = 0;
        int rise = 0;
        int lastRiseAmount = 0;

        public override void Use()
        {
            Debug.Log("hp�� �ݺ���Ͽ� ���ݷ� ��� (�ִ� 15)");
            GameManager.Instance.Player.HPRelatedItemEfects.RemoveListener(BerserkerSwordEffect);
            GameManager.Instance.Player.HPRelatedItemEfects.AddListener(BerserkerSwordEffect);
            BerserkerSwordEffect();
        }

        public void BerserkerSwordEffect()
        {
            GameManager.Instance.Player.playerBase.Attack -= lastRiseAmount;
            temp = GameManager.Instance.Player.playerBase.Hp;
            while(temp > 0 || rise < 15)
            {
                temp -= 10;
                if(temp > 0)
                {
                    rise++;
                }
                else
                {
                    break;
                }
            }
            GameManager.Instance.Player.playerBase.Attack += rise;
            lastRiseAmount = rise;
            Debug.Log("Player Attack : " + GameManager.Instance.Player.playerBase.Attack);
        }
    }

    public static ItemBase[] ShopItems = new ItemBase[]
    {
        new Default(),      // 0�� ������ ( �޲޿� )

    };
}
