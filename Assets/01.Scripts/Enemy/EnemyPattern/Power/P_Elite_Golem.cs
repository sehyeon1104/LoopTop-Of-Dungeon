using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Elite_Golem : EnemyElite
{

    public override void Init()
    {
        base.Init();
    }

    protected override IEnumerator Attack1()
    {
        yield return null;
    }

    protected override IEnumerator Attack2()
    {
        yield return null;
    }

    protected override IEnumerator Attack3()
    {
        yield return null;
    }

    public override void EnemyDead()
    {
        base.EnemyDead();
    }
}
