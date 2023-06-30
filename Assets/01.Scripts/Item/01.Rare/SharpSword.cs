using Debug = Rito.Debug;

// 날카로운 검 (스킬 쿨타임 5% 감소 )
public class SharpSword : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Rare;

    public override bool isPersitantItem => false;

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("날카로운 검 효과 발동");
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.1f;
        //Debug.Log("스킬 쿨타임 5% 감소");
        // TODO : 스킬 쿨타임 감소 구현
        //GameManager.Instance.Player.playerBase. += 5;
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.playerBase.Attack -= GameManager.Instance.Player.playerBase.InitAttack * 0.1f;
        // TODO : 스킬 쿨타임 증가 구현
        //GameManager.Instance.Player.playerBase. -= 5;
    }
}