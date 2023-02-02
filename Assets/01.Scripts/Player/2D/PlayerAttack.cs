using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    AgentInput agentInput = null;

    private void Awake()
    {
        agentInput = GetComponent<AgentInput>();
    }

    private void Start()
    {
        agentInput.Attack.AddListener(Attack);
    }

    // ���� ��ư�� ������ �� �ߵ��� �Լ�
    public void InvokeAttackEvents()
    {
        agentInput.Attack.Invoke();
    }

    public void Attack()
    {
        // TODO : �� ���ݽ� ���� �ִϸ��̼� �۵� �� ������ �ǰ����� üũ

        Debug.Log("Attack");

        Debug.Log(Vector2.Distance(transform.position, Boss.Instance.transform.position));

        if (Vector2.Distance(transform.position, Boss.Instance.transform.position) < 2f)
        {
            Boss.Instance.Hit((int)Player.Instance.pBase.Damage);
        }
    }
}
