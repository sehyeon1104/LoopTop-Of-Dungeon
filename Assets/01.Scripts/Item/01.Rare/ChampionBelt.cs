using Debug = Rito.Debug;

public class ChampionBelt : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("헤비급 챔피언의 벨트 효과 발동");
        Debug.Log("하트 1/4칸 증가");
        int incQuantity = 1; //Mathf.RoundToInt(GameManager.Instance.Player.playerBase.InitMaxHp * 0.15f);
        GameManager.Instance.Player.playerBase.MaxHp += incQuantity;
        GameManager.Instance.Player.playerBase.Hp += incQuantity;
        UIManager.Instance.UpdateUI();
    }
}
