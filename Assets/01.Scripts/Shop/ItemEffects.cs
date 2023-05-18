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
        public virtual bool isOneOff { get; } = false;
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

    #region Common
    // �����޸� �Ź� ( �̵��ӵ� 10% ���� )
    public class WingShoes : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("�����޸� �Ź� ȿ�� �ߵ�");
            Rito.Debug.Log("�̵��ӵ� 10% ����");
            GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.1f;
        }
    }

    // ������ �尩 ( ���ݹ��� 10% ���� )
    public class GiantGlove : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Common;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("������ �尩 ȿ�� �ߵ�");
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
            Debug.Log("������ �� ȿ�� �ߵ�");
            Debug.Log("���ݷ� 5% ����");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.05f;
        }
    }

    #endregion

    #region Rare
    public class ChampionBelt : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("���� è�Ǿ��� ��Ʈ ȿ�� �ߵ�");
            Debug.Log("�ִ� ����� 15% ����");
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
            Debug.Log("������ ���� ȿ�� �ߵ�");
            Debug.Log("�뽬 ��Ÿ�� 0.5�� ����");
        }
    }

    public class InquisitorsRing : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("Ž�谡�� ���� ȿ�� �ߵ�");
            Debug.Log("ġ��Ÿ Ȯ�� 6% ����");
            GameManager.Instance.Player.playerBase.CritChance += 6;
        }
    }

    // ��ī�ο� �� (��ų ��Ÿ�� 5% ���� )
    public class SharpSword : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Rare;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("��ī�ο� �� ȿ�� �ߵ�");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.1f;
            //Debug.Log("��ų ��Ÿ�� 5% ����");
            // TODO : ��ų ��Ÿ�� ���� ����
            //GameManager.Instance.Player.playerBase. += 5;
        }
    }

    #endregion

    #region Epic
    public class VampireFangs : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.buff;
        public override Define.ItemRating itemRating => Define.ItemRating.Epic;

        //public override bool isPersitantItem => true;
        public override bool isPersitantItem => true;

        private float probabilityChance = 1f;

        public override void Use()
        {
            // TODO : �� óġ�� hp 1 ȸ�� ����
            Debug.Log("�����̾��� �۰��� ȿ�� �ߵ�");
            //Debug.Log("�� óġ �� hp 1 ȸ��");

            GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(VampireFangsEffect);
            GameManager.Instance.Player.AttackRelatedItemEffects.AddListener(VampireFangsEffect);
        }

        public void VampireFangsEffect()
        {
            // �� ���ݽ� 1% Ȯ���� hp 1 ȸ��
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

        public override void Use()
        {
            Debug.Log("���� �����۷��� ȿ�� �ߵ�");
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

        public override void Use()
        {
            Debug.Log("�������� �� ȿ�� �ߵ�");
            Debug.Log("hp�� �ݺ���Ͽ� ���ݷ� ��� (�ִ� 15)");
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

    #endregion

    #region ETC

    public class SmallHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;
        public override void Use()
        {
            Debug.Log("hp���� (��) ���");
            Debug.Log("ü�� 15 ȸ��");
            GameManager.Instance.Player.playerBase.Hp += 15;
        }
    }

    public class MediumHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;
        public override void Use()
        {
            Debug.Log("hp���� (��) ���");
            Debug.Log("ü�� 30 ȸ��");
            GameManager.Instance.Player.playerBase.Hp += 30;
        }
    }

    public class LargeHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;
        public override void Use()
        {
            Debug.Log("hp���� (��) ���");
            Debug.Log("ü�� 50 ȸ��");
            GameManager.Instance.Player.playerBase.Hp += 50;
        }
    }

    public class ExtraLargeHpPotion : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.heal;

        public override Define.ItemRating itemRating => Define.ItemRating.ETC;

        public override bool isPersitantItem => false;
        public override bool isOneOff => true;
        public override void Use()
        {
            Debug.Log("hp���� (Ư��) ���");
            Debug.Log("ü�� 80 ȸ��");
            GameManager.Instance.Player.playerBase.Hp += 80;
        }
    }

    #endregion

    #region Special (BrokenItem)

    public class NailedShoes : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("������ �Ź� ȿ�� �ߵ�");
            Debug.Log("���ݷ� 30% ����, �̵��ӵ� 25% ����");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.3f;
            GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.25f;
        }
    }

    public class CloudyGlasses : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("�帰 �Ȱ� ȿ�� �ߵ�");
            Debug.Log("�̵��ӵ� 20% ����, ġ��Ÿ Ȯ�� 8% ����");
            GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.2f;
            GameManager.Instance.Player.playerBase.CritChance -= 8f;
        }
    }

    public class TurtleHat : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("�ź� ���� ȿ�� �ߵ�");
            Debug.Log("�ִ����� 30% ����, ���ݷ� 15% ����");
            GameManager.Instance.Player.playerBase.MaxHp += (int)(GameManager.Instance.Player.playerBase.InitMaxHp * 0.3f);
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.15f;
        }
    }

    public class GlassCannon : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.broken;

        public override Define.ItemRating itemRating => Define.ItemRating.Special;

        public override bool isPersitantItem => false;

        public override void Use()
        {
            Debug.Log("���� ���� ȿ�� �ߵ�");
            Debug.Log("���ݷ� 60% ����, �޴� ������ 2�� ����");
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.6f;
            //TODO : �޴� ������ 2�� ���� ����

        }
    }

    #endregion

    public static ItemBase[] Items = new ItemBase[]
    {
        new Default(),      // 0�� ������ ( ȿ��x )

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
        new GlassCannon(),
    };
}
