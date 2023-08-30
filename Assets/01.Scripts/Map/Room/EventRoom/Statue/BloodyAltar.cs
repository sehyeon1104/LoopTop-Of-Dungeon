using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyAltar : StatueBase
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
        GameManager.Instance.Player.playerBase.MaxHp -= 20;
        GameManager.Instance.Player.playerBase.Attack += GameManager.Instance.Player.playerBase.InitAttack * 0.5f;

        effectTmp.SetText("최대체력 20 감소, 공격력 50% 증가.");
        StartCoroutine(IETextAnim());

        isUseable = false;
    }
}
