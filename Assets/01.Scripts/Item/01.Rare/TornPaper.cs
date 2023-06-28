using Debug = Rito.Debug;

public class TornPaper : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("찢어진 종이 효과 발동");
        Debug.Log("대쉬 쿨타임 10% 감소");
    }
    public override void Disabling()
    {
        
    }
}
