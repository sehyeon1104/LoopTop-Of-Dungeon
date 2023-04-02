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
    // 공격 버튼을 눌렀을 때 발동될 함수
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
        // TODO : 적 공격시 공격 애니메이션 작동 및 적에게 피격판정 체크
    }
   
}
