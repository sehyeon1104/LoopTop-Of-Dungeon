using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSkill : PlayerSkillBase
{

    float choppingDmg = 20;
    float rushDmg = 13;
    float rushVelocity = 2;
    float rushDuration = 2f;
    WaitForFixedUpdate rushWait = new WaitForFixedUpdate();
    ParticleSystem attackPar;
    private void Awake()
    {
        Cashing();
    }
    protected override void Update()
    {
        base.Update();
    }
    private void Start()
    {
        attackPar = Managers.Resource.Instantiate("Assets/10.Effects/player/P_Attack.prefab", transform).GetComponent<ParticleSystem>();
    }
    protected override void Attack()
    {
        if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || player.playerBase.IsPDead)
            return;
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Player/Power/Attack.wav");

        CinemachineCameraShaking.Instance.CameraShake();
        playerAnim.SetTrigger("Attack");

        attackPar.transform.SetParent(transform);

        attackPar.transform.localPosition = playerSprite.flipX ? Vector3.right : Vector3.left;
        attackPar.Play();

        attackPar.transform.SetParent(null);

        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss"))
            {
                CinemachineCameraShaking.Instance.CameraShake(5, 0.3f);
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, GameManager.Instance.Player.playerBase.CritChance);
            }
        }
    }



    protected override void FirstSkill(int level)
    {
        StartCoroutine(BottomingOut());
    }

    protected override void SecondSkill(int level)
    {
        StartCoroutine(Rush());
    }

    protected override void ThirdSkill(int level)
    {

    }

    protected override void ForuthSkill(int level)
    {

    }

    protected override void FifthSkill(int level)
    {

    }

    protected override void UltimateSkill()
    {

    }
    protected override IEnumerator Dash()
    {
        return base.Dash();
    }

    protected override void FirstSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 1, 0);
    }

    protected override void SecondSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 2, 0);
    }

    protected override void ThirdSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 3, 0);
    }

    protected override void ForuthSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 4, 0);
    }

    protected override void FifthSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 5, 0);
    }
    #region 스킬 구현
    IEnumerator BottomingOut()
    {
        CinemachineCameraShaking.Instance.CameraShake(10, 0.2f);
        Poolable choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/BottomingOutEffect.prefab", transform.position, Quaternion.identity);
        Collider2D[] enemys = Physics2D.OverlapCircleAll(choppingObj.transform.position, 2, 1 << enemyLayer);
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GetComponent<IHittable>().OnDamage(choppingDmg, 0);
        }
        yield return null;
    }
    IEnumerator Rush()
    {
        float timer = 0;
        float tickTimer = 0;
        float dmgTickTime = rushDuration / 5;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        playerRigid.velocity = playerMovement.Direction * rushVelocity;
        do
        {
            timer += Time.fixedDeltaTime;
            tickTimer += Time.fixedDeltaTime;
            if (tickTimer > dmgTickTime)
            {
                CinemachineCameraShaking.Instance.CameraShake(5, 0.2f);
                Poolable choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEffect.prefab", transform.position, Quaternion.identity);
                Collider2D[] enemys = Physics2D.OverlapCircleAll(choppingObj.transform.position, 1.5f, 1 << enemyLayer);
                for (int i = 0; i < enemys.Length; i++)
                {
                    enemys[i].GetComponent<IHittable>().OnDamage(rushDmg, 0);
                }
                tickTimer = 0;
            }
            yield return rushWait;
        }
        while (timer < rushDuration);
        playerMovement.IsMove = true;
        player.IsInvincibility = false;
        playerRigid.velocity = Vector3.zero;
        yield return null;
    }
    IEnumerator Jump()
    {
        yield return null;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
