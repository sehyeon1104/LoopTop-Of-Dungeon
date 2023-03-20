using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Player Attack Class
public class PlayerAttack : PlayerBase
{
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    Animator playerAnim;
    // ���� ��ư�� ������ �� �ߵ��� �Լ�
    public void InvokeAttackEvents()
    {
        //if(agentInput == null)
        //{
        //    Debug.LogWarning("agentInput is NULL!");
        //    agentInput = GetComponent<AgentInput>();
        //    agentInput.Attack.AddListener(Attack);
        //    return;
        //}
        //agentInput.Attack.Invoke();
        //playerAnim.SetTrigger("Attack");
    }

    public void Attack()
    {
        // TODO : �� ���ݽ� ���� �ִϸ��̼� �۵� �� ������ �ǰ����� üũ
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for(int i=0; i<enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss")) { 
                Debug.Log("����");    
                CinemachineCameraShaking.Instance.CameraShake();
                playerAnim.SetTrigger("Attack");
                enemys[i].GetComponent<IHittable>().OnDamage(Damage, gameObject, CritChance);
            }
        }
    }
}
