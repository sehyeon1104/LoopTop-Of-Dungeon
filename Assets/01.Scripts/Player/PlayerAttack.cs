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
        GameObject.FindGameObjectWithTag("Attack").GetComponent<Button>().onClick.AddListener(Attack);
    }
    public void Attack()
    {
        // TODO : 적 공격시 공격 애니메이션 작동 및 적에게 피격판정 체크
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;
        playerAnim.SetTrigger("Attack");
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for(int i=0; i<enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss")) { 
                CinemachineCameraShaking.Instance.CameraShake();
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, gameObject, GameManager.Instance.Player.playerBase.CritChance);
            }
        }
    }
}
