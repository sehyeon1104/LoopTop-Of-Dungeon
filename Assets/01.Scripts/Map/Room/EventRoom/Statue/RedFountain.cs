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
            // TODO : 저주아이템 드랍
        }
        effectTmp.SetText("당신은 분수에 피를 흘립니다...");
        StartCoroutine(IETextAnim());
        isUseable = false;
    }
}
