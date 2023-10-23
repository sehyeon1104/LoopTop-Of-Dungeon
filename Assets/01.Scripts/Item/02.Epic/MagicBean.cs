using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBean : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Epic;

    public override bool isPersitantItem => true;

    public override bool isSetElement => true;

    public override void Init()
    {

    }

    public override void Use()
    {
        LastingEffect();
    }

    public override void Disabling()
    {
        GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(MagicBeanAbility);
    }

    public override void LastingEffect()
    {
        GameManager.Instance.Player.AttackRelatedItemEffects.RemoveListener(MagicBeanAbility);
        GameManager.Instance.Player.AttackRelatedItemEffects.AddListener(MagicBeanAbility);
    }

    public void MagicBeanAbility()
    {
        GameObject bean = Managers.Resource.Instantiate("Assets/03.Prefabs/Item/MagicBeanBullet.prefab");
        bean.transform.position = GameManager.Instance.Player.transform.position;
        bean.transform.right = PlayerVisual.Instance.IsFlipX() ? GameManager.Instance.Player.transform.right : -GameManager.Instance.Player.transform.right;
    }
}
