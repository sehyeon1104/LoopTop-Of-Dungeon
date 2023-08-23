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
        Debug.Log("헤비급 챔피언의 벨트 효과 발동");
        Debug.Log("하트 1/4칸 증가");
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
