using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : EnemyDefault
{
    public override void MoveToPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime);
    }
}
