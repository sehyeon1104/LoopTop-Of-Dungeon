using Debug = Rito.Debug;

public class RunawayHorseSeal : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.broken;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("폭주마 휘장 효과 발동");
        Debug.Log("이동속도 20% 증가, 치명타 확률 8% 감소");
        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.2f;
        GameManager.Instance.Player.playerBase.CritChance -= 8f;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.MoveSpeed -= GameManager.Instance.Player.playerBase.InitMoveSpeed * 0.2f;
        GameManager.Instance.Player.playerBase.CritChance += 8f;
    }
}