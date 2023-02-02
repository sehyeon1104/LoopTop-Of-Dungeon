using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    AgentInput agentInput = null;
    Animator playerAnim = null;

    [SerializeField]
    private float attackRange = 1f;

    private void Awake()
    {
        agentInput = GetComponent<AgentInput>();
        playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        agentInput.Attack.AddListener(Attack);
    }

    // ���� ��ư�� ������ �� �ߵ��� �Լ�
    public void InvokeAttackEvents()
    {
        agentInput.Attack.Invoke();
        playerAnim.SetTrigger("Attack");
    }

    public void Attack()
    {
        // TODO : �� ���ݽ� ���� �ִϸ��̼� �۵� �� ������ �ǰ����� üũ

        Debug.Log("Attack");

        if (Boss.Instance.isDead)
            return;

        Debug.Log(Vector2.Distance(transform.position, Boss.Instance.transform.position));

        if (Vector2.Distance(transform.position, Boss.Instance.transform.position) < attackRange)
        {
            Boss.Instance.Hit((int)Player.Instance.pBase.Damage);
            StartCoroutine(CameraShaking.Instance.IECameraShakeOnce());
        }
    }
}
