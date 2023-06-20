using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShuffleStatue : StatueBase
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
        // TODO : ½ºÅ³ ·£´ý¼ÅÇÃ ¹× Àû¿ë
    }
}
