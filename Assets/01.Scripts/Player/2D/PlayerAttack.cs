using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Player Attack Class
public partial class Player
{
    [SerializeField]
    private float attackRange = 1f;

    // 공격 버튼을 눌렀을 때 발동될 함수
    public void InvokeAttackEvents()
    {
        if (isPDead)
            return;

        agentInput.Attack.Invoke();
        playerAnim.SetTrigger("Attack");
    }

    public void Attack()
    {
        if (isPDead)
            return;
        // TODO : 적 공격시 공격 애니메이션 작동 및 적에게 피격판정 체크
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for(int i=0; i<enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss")) { 
                Debug.Log("닿음");
                CinemachineCameraShaking.Instance.CameraShake();
                enemys[i].GetComponent<IHittable>().OnDamage(pBase.Damage, gameObject, pBase.CritChance);
            }
        }
        Debug.Log("Attack");

        //if (Boss.Instance.isBDead)
        //    return;

        //Debug.Log(Vector2.Distance(transform.position, Boss.Instance.transform.position));
        
        //if (Vector2.Distance(transform.position, Boss.Instance.transform.position) < attackRange)
        //{
        //    Boss.Instance.OnDamage(pBase.Damage, gameObject, pBase.CritChance);
        //    CinemachineCameraShaking.Instance.CameraShake();
        //}

    }
}
