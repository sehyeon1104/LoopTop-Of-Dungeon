using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonation : StatueBase
{
    private int requireHp = 5;

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        button.onClick.RemoveListener(StatueFunc);
    }

    protected override void InteractiveWithPlayer()
    {
        base.InteractiveWithPlayer();
        button.onClick.RemoveListener(StatueFunc);
        button.onClick.AddListener(StatueFunc);
    }

    protected override void StatueFunc()
    {
        if (requireHp > GameManager.Instance.Player.playerBase.Hp)
        {
            effectTmp.SetText($"ü���� �����մϴ�! �䱸ġ : {requireHp}");
            StartCoroutine(IETextAnim());
            return;
        }

        GameManager.Instance.Player.playerBase.Hp -= requireHp;
        if(Random.Range(0, 100) < requireHp)
        {
            StageManager.Instance.InstantiateChest(new Vector3(transform.position.x, transform.position.y - 3), Define.ChestRating.Common);
        }


        effectTmp.SetText($"ü�� {requireHp * 5}�Ҹ�");
        StartCoroutine(IETextAnim());

        requireHp *= 2;
    }
}
