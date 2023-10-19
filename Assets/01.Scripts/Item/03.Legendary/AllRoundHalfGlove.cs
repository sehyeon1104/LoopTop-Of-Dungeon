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
        GameManager.Instance.Player.playerBase.MaxHp += 10;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.3f;
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.15f;
        GameManager.Instance.Player.playerBase.CritChance += 5f;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.MaxHp -= 10;
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.3f;
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.15f;
        GameManager.Instance.Player.playerBase.CritChance -= 5f;
    }
}