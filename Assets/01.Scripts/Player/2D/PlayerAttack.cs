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

    // 공격 버튼을 눌렀을 때 발동될 함수
    public void InvokeAttackEvents()
    {
        agentInput.Attack.Invoke();
    }

    public void Attack()
    {
        // TODO : 적 공격시 공격 애니메이션 작동 및 적에게 피격판정 체크

        Debug.Log("Attack");
    }
}
