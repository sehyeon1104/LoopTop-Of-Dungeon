using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Debug.Log("Attack");

        if (Boss.Instance.isBDead)
            return;

        Debug.Log(Vector2.Distance(transform.position, Boss.Instance.transform.position));

        if (Vector2.Distance(transform.position, Boss.Instance.transform.position) < attackRange)
        {
            Boss.Instance.OnDamage(pBase.Damage, gameObject, pBase.CritChance);
            CinemachineCameraShaking.Instance.CameraShakeOnce();
        }
    }
}
