using UnityEngine;
using Debug = Rito.Debug;

public class BerserkerSword : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

    public override bool isPersitantItem => true;
    public override bool isStackItem => true;

    private int curHp = 0;
    private int maxHp = 0;
    private int temp = 0;
    private static float rise = 0;

    public override void Init()
    {
        curHp = GameManager.Instance.Player.playerBase.Hp;
        maxHp = GameManager.Instance.Player.playerBase.MaxHp;
    }

    public override void Use()
    {
        Debug.Log("광전사의 검 효과 발동");
        BerserkerSwordAbility();
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(BerserkerSwordAbility);
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * rise;
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.HPRelatedItemEffects.RemoveListener(BerserkerSwordAbility);
        GameManager.Instance.Player.HPRelatedItemEffects.AddListener(BerserkerSwordAbility);
    }

    public void BerserkerSwordAbility()
    {
        if(rise > 0)
        {
            GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * rise;
        }

        temp = maxHp - curHp;

        rise = (temp / maxHp) * 100;
        Mathf.CeilToInt(rise);
        rise = Mathf.Clamp(rise, 0, 70);
        rise /= 100;

        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * rise;
    }
}