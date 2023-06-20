using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceStatue : StatueBase
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
        // TODO : 주사위 2개를 굴려 효과 획득. 1번주사위 : 효과, 2번주사위 : 수치
    }
}
