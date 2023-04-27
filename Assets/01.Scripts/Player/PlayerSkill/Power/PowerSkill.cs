using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSkill : PlayerSkillBase
{
    
    Animator playerAnim;
    ParticleSystem attackPar;
    float attackRange=1f;
    private void Awake()
    {
        Cashing();
        playerAnim = GetComponent<Animator>();
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
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || player.playerBase.IsPDead)
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
                CinemachineCameraShaking.Instance.CameraShake(5,0.3f);
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, GameManager.Instance.Player.playerBase.CritChance);

            }
        }
    }



    protected override void FirstSkill(int level)
    {
        
    }

    protected override void SecondSkill(int level)
    {
        
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
        
    }

    protected override void SecondSkillUpdate(int level)
    {
       
    }

    protected override void ThirdSkillUpdate(int level)
    {
       
    }

    protected override void ForuthSkillUpdate(int level)
    {
        
    }

    protected override void FifthSkillUpdate(int level)
    {
        
    }
}
