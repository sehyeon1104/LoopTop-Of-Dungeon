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
    protected override void DashSkill()
    {
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        float timer = 0;
        float alphaValue = 0;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        float distance = 0;

        Vector3 firstPosition = transform.position;
        Vector3 changePosition = transform.position;
        playerRigid.velocity = playerMovement.Direction * dashVelocity;
        SpriteRenderer currentPlayerSprite = GetComponent<SpriteRenderer>();
        dashCloneColor = dashSprite.color;
        dashCloneColor.a = 0;
        while (timer < dashTime)
        {
            timer += Time.deltaTime;
            alphaValue = timer / dashTime;
            distance = Vector2.SqrMagnitude(transform.position - changePosition);
            if (distance > instanceClonePerVelocity * instanceClonePerVelocity)
            {
                changePosition = transform.position;
                Poolable dashPool = Managers.Pool.Pop(dashObj, transform.position);
                cloneList.Add(dashPool);
                SpriteRenderer dashPoolSprite = dashPool.GetComponent<SpriteRenderer>();
                dashPoolSprite.sprite = currentPlayerSprite.sprite;
                dashPoolSprite.flipX = currentPlayerSprite.flipX;
                dashCloneColor.a = alphaValue;
                dashPoolSprite.color = dashCloneColor;

            }
            yield return new WaitForFixedUpdate();
        }
        print(Vector3.Magnitude(firstPosition - transform.position));
        //Vector3 playerPoss = transform.position - playerPos;
        //float angle = Mathf.Atan2(playerPoss.y, playerPoss.x) * Mathf.Rad2Deg;
        //Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/DashSmoke.prefab", playerPos, angleAxis);
        playerMovement.IsMove = true;
        player.IsInvincibility = false;
        foreach (var c in cloneList)
        {
            Managers.Pool.Push(c);
        }
        cloneList.Clear();
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
