using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Player Attack Class
public class PlayerAttack :  MonoBehaviour
{
    [SerializeField]
    private float attackRange = 1f;
    Animator playerAnim;
    // ���� ��ư�� ������ �� �ߵ��� �Լ�
    private void Awake()
    {
        playerAnim = GetComponent<Animator>();  
        UIManager.Instance.playerUI.transform.Find("RightDown/Btns/AttackBtn").GetComponent<Button>().onClick.AddListener(Attack);
    }
    private void Start()
    {
    }
    public void Attack()
    {
        // TODO : �� ���ݽ� ���� �ִϸ��̼� �۵� �� ������ �ǰ����� üũ
    }
   
}
