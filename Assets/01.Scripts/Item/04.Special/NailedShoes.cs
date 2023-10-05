using Debug = Rito.Debug;

public class NailedShoes : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.broken;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.5f;
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.25f;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.5f;
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.25f;
    }
}