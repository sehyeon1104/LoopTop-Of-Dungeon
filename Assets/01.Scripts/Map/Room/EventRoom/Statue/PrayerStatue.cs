using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayerStatue : StatueBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

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

    protected override void StandBy()
    {
        base.StandBy();
    }

    protected override void StatueFunc()
    {
        // TODO : hp�� �Ҹ��ϸ� �Ҹ��� ���� ������ ��� ���
        if (!isUseable)
            return;

        if (GameManager.Instance.Player.playerBase.Hp < 2)
            return;

        // TODO : ������ ����� Legendary�� ��� return

        GameManager.Instance.Player.OnDamage(1, 0);
    }

    public void TakeChest()
    {
        isUseable = false;
    }
}
