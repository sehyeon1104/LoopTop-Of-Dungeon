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
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            return;

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


    protected override void FifthSkill(int level)
    {
        
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
        float timerA = 0;
        float flusA = 0;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        Vector3 playerPos = transform.position;
        GameObject dashSprite = new GameObject();
        dashSprite.AddComponent<SpriteRenderer>();
        dashSprite.AddComponent<Poolable>();
        dashSprite.GetComponent<SpriteRenderer>().sprite = playerSprite.sprite;
        dashSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Skill";
        dashSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);



        playerRigid.velocity = playerMovement.Direction * dashVelocity;
        while (timer < dashTime)
        {
            if (timerA > 0.02f)
            {
                flusA += 0.2f;
                dashSprite.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;
                Poolable clone = Managers.Pool.Pop(dashSprite, transform.position);
                clone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, flusA);
                cloneList.Add(clone);
            }
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            yield return null;
        }
        Vector3 playerPoss = transform.position - playerPos;
        float angle = Mathf.Atan2(playerPoss.y, playerPoss.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/DashSmoke.prefab", playerPos, angleAxis);
        playerMovement.IsMove = true;
        player.IsInvincibility = false;
        foreach (var c in cloneList)
        {
            Managers.Pool.Push(c);
        }
        cloneList.Clear();
    }
}
