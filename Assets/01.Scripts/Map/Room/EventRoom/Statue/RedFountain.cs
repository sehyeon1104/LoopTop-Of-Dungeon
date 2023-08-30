using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFountain : StatueBase
{
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
        if (!isUseable)
            return;

        if (GameManager.Instance.Player.playerBase.Hp <= GameManager.Instance.Player.playerBase.MaxHp * 0.6f)
            GameManager.Instance.Player.playerBase.Hp = GameManager.Instance.Player.playerBase.MaxHp;
        else
        {
            GameManager.Instance.Player.playerBase.Hp -= 15;
            // TODO : ���־����� ���
        }
        effectTmp.SetText("����� �м��� �Ǹ� �긳�ϴ�...");
        StartCoroutine(IETextAnim());
        isUseable = false;
    }
}
