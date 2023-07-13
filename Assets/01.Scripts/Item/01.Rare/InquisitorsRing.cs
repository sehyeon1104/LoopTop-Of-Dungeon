using Debug = Rito.Debug;

public class InquisitorsRing : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => false;

    private float critChanceAdd = 10f;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("탐험가의 반지 효과 발동");
        Debug.Log("치명타 확률 10% 증가");
        GameManager.Instance.Player.playerBase.CritChance += critChanceAdd;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.CritChance -= critChanceAdd;
    }
}