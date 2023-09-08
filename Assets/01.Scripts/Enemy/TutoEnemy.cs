using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnemy : EnemyDefault
{
    WaitForSeconds attackWait = new WaitForSeconds(2f);
    WaitForSeconds waitTime = new WaitForSeconds(0.5f);

    private bool isDead = false;

    public override IEnumerator MoveToPlayer()
    {
        return base.MoveToPlayer();
    }

    public override IEnumerator AttackToPlayer()
    {
        yield return base.AttackToPlayer();

        yield return waitTime;

        Collider2D col = Physics2D.OverlapCircle(transform.position, 1, 1 << 8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(damage, 0);

        yield return attackWait;

        actCoroutine = null;

    }

    public override void OnDamage(float damage, float critChance, Poolable hitEffect = null)
    {
        base.OnDamage(damage, critChance);
    }

    public override void EnemyDead()
    {
        if (!isDead)
        {
            isDead = true;
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_DeSpawn.wav");

            CinemachineCameraShaking.Instance.CameraShake(8, 0.2f);

            TutorialManager.Instance.tutoEnemyRoom.EnemyDead(gameObject);

            Managers.Pool.Push(poolable);
        }
    }
}
