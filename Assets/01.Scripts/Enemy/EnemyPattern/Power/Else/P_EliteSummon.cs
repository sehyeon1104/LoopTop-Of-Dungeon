using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_EliteSummon : EnemyDefault
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject beam;

    public override IEnumerator AttackToPlayer()
    {
        isInvincible = true;

        Vector3 startDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        transform.DOMove(transform.position + startDir * 10f, 0.5f);
        yield return new WaitForSeconds(0.7f);

        anim.speed = 0.65f;
        yield return base.AttackToPlayer();

        Vector3 dir = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;

        warning.SetActive(true);
        float timer = 0;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;

            dir = (playerTransform.position - transform.position).normalized;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;

            warning.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        beam.SetActive(true);
        beam.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        CinemachineCameraShaking.Instance.CameraShake(5, 0.1f);
            
        Collider2D col = Physics2D.OverlapBox(transform.position, new Vector2(60f, 0.25f), angle, 1 << 8);
            
        if (col != null)
            GameManager.Instance.Player.OnDamage(10, 0);

        yield return new WaitForSeconds(0.5f);

        warning.SetActive(false);
        beam.SetActive(false);
        anim.speed = 1f;
        EnemyDead();
        actCoroutine = null;
    }
    public override void EnemyDead()
    {
        if (!isDead)
        {
            warning.SetActive(false);
            beam.SetActive(false);
            isDead = true;
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_DeSpawn.wav");

            if (GameManager.Instance.sceneType == Define.Scene.Field)
            {
                EnemySpawnManager.Instance.RemoveEnemyInList(poolable);
            }

            Managers.Pool.Push(poolable);
            EnemyManager.Instance.EnemyDeadRelatedItemEffects.Invoke();
        }
    }
}