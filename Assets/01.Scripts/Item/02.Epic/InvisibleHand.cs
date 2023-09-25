using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleHand : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => true;

    public override bool isSetElement => true;

    public override void Disabling()
    {

    }

    public override void LastingEffect()
    {
        ItemManager.Instance.FragmentDropRelatedItemEffects.RemoveListener(InvisibleHandAbility);
        ItemManager.Instance.FragmentDropRelatedItemEffects.AddListener(InvisibleHandAbility);
    }

    public override void SetItemCheck()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public void InvisibleHandAbility(Vector3 pos)
    {
        if (Random.Range(0, 5) == 0)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(pos, 1f, 1 << 9);
            Managers.Pool.PoolManaging("Assets/10.Effects/player/@Item/ExplosionFragment.prefab", pos, Quaternion.identity);
            CinemachineCameraShaking.Instance.CameraShake(6, 0.1f);
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Attack * 1.5f);
            }
        }
    }
}
