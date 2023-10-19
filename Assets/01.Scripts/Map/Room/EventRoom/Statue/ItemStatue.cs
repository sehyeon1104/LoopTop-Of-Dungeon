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
        // TODO : 인벤토리 내 아이템 하나를 선택하면, 해당 아이템을 파괴하고 랜덤으로 아이템 지급
        if (!isUseable)
            return;
        isUseable = false;
        ToggleInteractiveTMP();
    }
}
