﻿using Debug = Rito.Debug;

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
        lastRise = 0;
        BerserkerSwordEffect();
        LastingEffect();
    }

    public override void Disabling()
    {
        Debug.Log("Disabling");
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(BerserkerSwordEffect);
        GameManager.Instance.Player.playerBase.Attack -= lastRise;
    }

    public override void LastingEffect()
    {
        Debug.Log("아이템 효과 Listener 추가");
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(BerserkerSwordEffect);
        GameManager.Instance.Player.HPRelatedItemEffects.AddListener(BerserkerSwordEffect);
    }

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