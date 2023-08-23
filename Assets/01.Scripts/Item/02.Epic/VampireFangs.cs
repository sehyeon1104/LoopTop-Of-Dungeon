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
        // TODO : 적 처치시 hp 1 회복 구현
        Debug.Log("뱀파이어의 송곳니 효과 발동");
        Debug.Log("적 공격시 1% 확률로 hp 1 회복");
        //Debug.Log("적 처치 시 hp 1 회복");

        // TODO : 적 사망 시 발동되는 이벤트에 추가
    }

    public override void Disabling()
    {

    }

    public override void LastingEffect()
    {

    }

    public void VampireFangsEffect()
    {
        // 적 10마리 처치 시 hp 1/4칸 회복
        stack++;
        if(stack >= 10)
        {
            stack = 0;
            GameManager.Instance.Player.playerBase.Hp += 1;
        }
    }
}