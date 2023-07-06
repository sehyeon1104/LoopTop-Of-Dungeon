using UnityEngine;
using Debug = Rito.Debug;

public class VampireFangs : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => true;

    private float probabilityChance = 1f;

    public override void Init()
    {

    }

    public override void Use()
    {
        // TODO : 적 처치시 hp 1 회복 구현
        Debug.Log("뱀파이어의 송곳니 효과 발동");
        Debug.Log("적 공격시 1% 확률로 hp 1 회복");
        //Debug.Log("적 처치 시 hp 1 회복");

        GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(VampireFangsEffect);
        GameManager.Instance.Player.AttackRelatedItemEffects.AddListener(VampireFangsEffect);
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(VampireFangsEffect);
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(VampireFangsEffect);
        GameManager.Instance.Player.AttackRelatedItemEffects.AddListener(VampireFangsEffect);
    }

    public void VampireFangsEffect()
    {
        // 적 공격시 1% 확률로 hp 1 회복
        Debug.Log("doing VampireFangsEffect");
        if(Random.Range(0, 100) < probabilityChance)
        {
            Debug.Log("뱀파이어 효과 터짐");
            GameManager.Instance.Player.playerBase.Hp += 1;
        }

    }
}