using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MICKnife : ItemBase
{
    public override Define.ItemType itemType => Define.ItemType.buff;

    public override Define.ItemRating itemRating => Define.ItemRating.Common;

    public override bool isPersitantItem => true;

    public override void Disabling()
    {

    }

    public override void Init()
    {

    }

    public override void Use()
    {
        Debug.Log("MIC������ ȿ�� �ߵ�");
        Debug.Log("�⺻���� �� 5% Ȯ���� ���� �ο�");
        // TODO : �÷��̾� �⺻���ݿ� MICKnifeAbilityȿ�� �߰�
    }

    public override void LastingEffect()
    {
        // TODO : �÷��̾� �⺻���ݿ� MICKnifeAbilityȿ�� �߰�
    }

    private int bleedingChance = 0;
    public void MICKnifeAbility()
    {
        bleedingChance = Random.Range(0, 100);
        if(bleedingChance < 5)
        {
            // TODO : ������ ������ ��󿡰� �����̻�:���� �ο�
        }
    }

}
