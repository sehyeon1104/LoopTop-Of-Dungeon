using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Ghost : EnemyDefault
{
    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();

        GameManager.Instance.Player.OnDamage(damage, 0);

        EnemyDead();

        actCoroutine = null;

    }

    public override void EnemyDead()
    {
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_DeSpawn.wav");

        //EnemySpawnManager.Instance.RemoveEnemyInList(poolable);

        gameObject.SetActive(false);
    }
}
