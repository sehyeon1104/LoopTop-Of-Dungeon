using Debug = Rito.Debug;

public class AllRoundHalfGlove : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Legendary;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("만능 하프글러브 효과 발동");
        Debug.Log("공격력 15% 증가 및 스킬 쿨타임 15% 감소");
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.15f;
        GameManager.Instance.Player.playerBase.coolDown += 15;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.15f;
        GameManager.Instance.Player.playerBase.coolDown += 15;
    }
}