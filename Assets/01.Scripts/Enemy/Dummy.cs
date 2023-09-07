using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : EnemyDefault
{
    private void Start()
    {
        isControl = false;
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