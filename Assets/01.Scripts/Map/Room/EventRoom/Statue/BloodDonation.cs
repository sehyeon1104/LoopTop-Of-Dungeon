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
        GameManager.Instance.Player.playerBase.Hp -= requireHp;



        effectTmp.SetText($"체력 {requireHp * 5}소모");
        StartCoroutine(IETextAnim());

        requireHp *= 2;
    }
}
