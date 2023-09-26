using UnityEngine;
using Debug = Rito.Debug;

public class VampireFangs : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;
    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => true;

    private int stack;

    public override void Init()
    {

    }

    public override void Use()
    {
        // TODO : 적 사망 시 발동되는 이벤트에 추가
        LastingEffect();
    }

    public override void Disabling()
    {
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.RemoveListener(VampireFangsEffect);
    }

    public override void LastingEffect()
    {
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.RemoveListener(VampireFangsEffect);
        EnemyManager.Instance.EnemyDeadRelatedItemEffects.AddListener(VampireFangsEffect);
    }

    public void VampireFangsEffect(Vector3 transform)
    {
        // 적 10마리 처치시 hp 3 회복
        stack++;
        if(stack >= 10)
        {
            stack = 0;
            GameManager.Instance.Player.playerBase.Hp += 4;
        }
    }
}