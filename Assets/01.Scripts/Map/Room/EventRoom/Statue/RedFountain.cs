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


        effectTmp.SetText("����� �м��� �Ǹ� �긳�ϴ�...");
        StartCoroutine(IETextAnim());
    }
}
