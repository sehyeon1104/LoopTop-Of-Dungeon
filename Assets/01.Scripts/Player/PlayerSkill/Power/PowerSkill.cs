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
    private void Start()
    {
        attackPar = Managers.Resource.Instantiate("Assets/10.Effects/player/P_Attack.prefab", transform).GetComponent<ParticleSystem>();
    }
    public override void Attack()
    {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        playerAnim.SetTrigger("Attack");

        attackPar.transform.SetParent(transform);

        attackPar.transform.localPosition = PlayerVisual.Instance.playerSprite.flipX ? Vector3.right : Vector3.left;
        attackPar.Play();

        attackPar.transform.SetParent(null);

        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss"))
            {
                CinemachineCameraShaking.Instance.CameraShake();
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, gameObject, GameManager.Instance.Player.playerBase.CritChance);

            }
        }
    }

    public override void DashSkill()
    {
        
    }

    public override void FifthSkill(int level)
    {
        
    }

    public override void FirstSkill(int level)
    {
        
    }
    public override void SecondSkill(int level)
    {
        
    }

    public override void ThirdSkill(int level)
    {
       
    }

    public override void ForuthSkill(int level)
    {
       
    }


    public override void UltimateSkill()
    {
        
    }
}
