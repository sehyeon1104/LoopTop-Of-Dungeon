using Debug = Rito.Debug;

public class ChampionBelt : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => false;

    int incQuantity = 1;

    public override void Init()
    {

    }

    public override void Use()
    {
        GameManager.Instance.Player.playerBase.MaxHp += 20;
        GameManager.Instance.Player.playerBase.Hp += 20;
        UIManager.Instance.UpdateUI();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.MaxHp -= 20;
        GameManager.Instance.Player.playerBase.Hp -= 20;
        UIManager.Instance.UpdateUI();
    }
}
