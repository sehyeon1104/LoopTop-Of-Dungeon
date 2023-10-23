using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatue : StatueBase
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
        if (collision.CompareTag("Player"))
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
        // TODO : �κ��丮 �� ������ �ϳ��� �����ϸ�, �ش� �������� �ı��ϰ� �������� ������ ����
        if (!isUseable)
            return;
        isUseable = false;
        ToggleInteractiveTMP();
    }
}
