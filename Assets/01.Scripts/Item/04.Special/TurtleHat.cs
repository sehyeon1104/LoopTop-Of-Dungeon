using Debug = Rito.Debug;

public class TurtleHat : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.broken;

    public override Define.ItemRating itemRating => Define.ItemRating.Special;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("거북 모자 효과 발동");
        Debug.Log("하트 1칸 증가, 공격력 15% 감소");
        GameManager.Instance.Player.playerBase.MaxHp += 50; //(int)(GameManager.Instance.Player.playerBase.InitMaxHp * 0.3f);
        GameManager.Instance.Player.playerBase.Hp += 50;
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.2f;
        //UIManager.Instance.MaxHpUpdate();
        UIManager.Instance.NewHpUpdate();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.MaxHp -= 50; //(int)(GameManager.Instance.Player.playerBase.InitMaxHp * 0.3f);
        GameManager.Instance.Player.playerBase.Hp -= 50;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.2f;
        //UIManager.Instance.MaxHpUpdate();
        UIManager.Instance.NewHpUpdate();
    }
}