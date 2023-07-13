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
        Debug.Log("찢어진 주문서 효과 발동");
        Debug.Log("10% 확률로 대시 사용시 쿨타임 초기화");
    }
    public override void Disabling()
    {
        
    }
}
