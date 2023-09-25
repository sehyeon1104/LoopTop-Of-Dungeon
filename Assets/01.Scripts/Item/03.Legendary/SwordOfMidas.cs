using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOfMidas : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

    public override bool isPersitantItem => true;
    public override bool isStackItem => true;

    private static float rise = 0;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("미다스의 검 효과 발동");
        SwordOfMidasAbility();
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(SwordOfMidasAbility);
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * rise;
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(SwordOfMidasAbility);
        GameManager.Instance.Player.HPRelatedItemEffects.AddListener(SwordOfMidasAbility);
    }

    public void SwordOfMidasAbility()
    {
        if (rise > 0)
        {
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * (rise * 0.01f);
        }

        rise = GameManager.Instance.Player.playerBase.FragmentAmount / 20;
        rise = Mathf.Clamp(Mathf.CeilToInt(rise), 0, 70);

        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * (rise * 0.01f);
    }
}
