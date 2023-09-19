using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Eye : EnemyDefault
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject beam;

    public override IEnumerator AttackToPlayer()
    {
        anim.speed = 0.65f;
        yield return base.AttackToPlayer();
        int randomAngle = 45 * Random.Range(0, 2);
        int angle = 90;

        warning.SetActive(true);
        warning.transform.rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);

        yield return new WaitForSeconds(0.75f);

        beam.SetActive(true);
        beam.transform.rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);

        for (int i = 0; i < 2; i++)
        {
            Collider2D col = Physics2D.OverlapBox(transform.position, new Vector2(60f, 0.25f), angle * i + randomAngle, 1 << 8);
            if (col != null)
                GameManager.Instance.Player.OnDamage(15, 0);
        }
        
        yield return new WaitForSeconds(2.5f);

        warning.SetActive(false);
        beam.SetActive(false);
        anim.speed = 1f;
        actCoroutine = null;
    }
    public override void EnemyDead()
    {
        warning.SetActive(false);
        beam.SetActive(false);
        base.EnemyDead();
    }
}
