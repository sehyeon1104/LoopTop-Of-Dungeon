using DG.Tweening;
using Math = System.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Elite_Golem : EnemyElite
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject shock;

    public override void Init()
    {
        base.Init();
        warning.SetActive(false);
        shock.SetActive(false);
    }

    protected override IEnumerator Attack1()
    {
        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 4; i++)
        {
            Poolable clone = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Power/Non_Load/P_EliteMob_Summon.prefab", transform.position, Quaternion.identity);
        }
        CinemachineCameraShaking.Instance.CameraShake(8, 0.2f);
        
        yield return new WaitForSeconds(3f);
    }

    protected override IEnumerator Attack2()
    {
        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.2f);

        CinemachineCameraShaking.Instance.CameraShake(8, 0.1f);
        Vector3 dir = (playerTransform.position - transform.position).normalized;
        for(int i = 1; i < 8; i++)
        {
            Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/power/Elite_Groundhit.prefab", transform.position + dir * i * 3, Quaternion.identity);
            clone.transform.localScale = Vector2.one * (1.125f - i * 0.125f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
    }

    protected override IEnumerator Attack3()
    {
        float timer = 0f;
        float force = 0.1f;
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);

        warning.SetActive(true);
        while (timer < 5f)
        {
            timer += 0.1f;
            timer = (float)Math.Round(timer, 2);
            if(timer % 0.5f <= float.Epsilon)
            {
                Debug.Log("·Ï¹ß»ç");
                float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                Vector3 dir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
                Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/power/EliteRock.prefab", transform.position + dir * 20f, Quaternion.identity);
                clone.transform.DOMove(transform.position, 1.5f);
            }

            Vector3 blackholeDir = (transform.position - playerTransform.position).normalized;
            switch (Vector3.Distance(transform.position, playerTransform.position))
            {
                case var a when a <= 1f:
                    force = 9f;
                    break;
                case var a when a <= 3f:
                    force = 7f;
                    break;
                default:
                    force = 5f;
                    break;
            }
            playerTransform.position += blackholeDir * force * Time.deltaTime;

            yield return waitTime;
        }

        yield return new WaitForSeconds(0.25f);
        
        warning.SetActive(false);
        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.15f);

        shock.SetActive(true);
        Collider2D col = Physics2D.OverlapCircle(transform.position, 5.25f, 1 << 8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(15, 0);
        
        CinemachineCameraShaking.Instance.CameraShake(8, 0.1f);

        yield return new WaitForSeconds(3f);
        shock.SetActive(false);
    }

    public override void EnemyDead()
    {
        base.EnemyDead();
    }
}
