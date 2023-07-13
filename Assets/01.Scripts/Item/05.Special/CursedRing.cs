using Debug = Rito.Debug;

public class CursedRing : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.broken;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => true;

    private static bool isFirst = false;

    public override void Init()
    {
        isFirst = false;
    }

    public override void Use()
    {
        Debug.Log("저주받은 반지 효과 발동");
        Debug.Log("공격력 60% 증가, 받는 데미지 2배 증가");
        if (!isFirst)
        {
            Debug.Log("공격력 증가");
            isFirst = true;
            GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.6f;
        }
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(CursedRingEffect);
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.AddListener(CursedRingEffect);

    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.6f;
        GameManager.Instance.Player.DamageMultiples = GameManager.Instance.Player.DamageMultiples / 2;
        isFirst = false;
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.RemoveListener(CursedRingEffect);
        GameManager.Instance.Player.OnDamagedRelatedItemEffects.AddListener(CursedRingEffect);
    }

    private void CursedRingEffect()
    {
        GameManager.Instance.Player.DamageMultiples += 1;
    }

}