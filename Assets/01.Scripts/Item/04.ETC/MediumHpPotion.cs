using Debug = Rito.Debug;

public class MediumHpPotion : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.heal;

    public override Define.ItemRating itemRating => Define.ItemRating.ETC;

    public override bool isPersitantItem => false;
    public override bool isOneOff => true;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("hp포션 (중) 사용");
        Debug.Log("체력 1칸 회복");
        GameManager.Instance.Player.playerBase.Hp += 4;
    }
}