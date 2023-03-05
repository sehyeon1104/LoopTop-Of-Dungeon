using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Player Attack Class
public partial class Player
{
    [SerializeField]
    private float attackRange = 1f;

    // ���� ��ư�� ������ �� �ߵ��� �Լ�
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
        // TODO : �� ���ݽ� ���� �ִϸ��̼� �۵� �� ������ �ǰ����� üũ
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for(int i=0; i<enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss")) { 
                Debug.Log("����");
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
