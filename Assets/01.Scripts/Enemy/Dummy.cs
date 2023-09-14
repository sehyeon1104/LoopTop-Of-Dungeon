using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : EnemyDefault
{
    public override void Init()
    {
        base.Init();
        IsControl = false;
    }

    public override void AnimInit()
    {

    }

    public override IEnumerator AttackToPlayer()
    {
        yield return null;
    }

    public override IEnumerator MoveToPlayer()
    {
        yield return null;
    }
}