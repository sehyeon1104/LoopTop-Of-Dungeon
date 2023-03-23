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
    Material hitMat;
    Material spriteLitMat;
    Animator playerAnim;
    // ���� ��ư�� ������ �� �ߵ��� �Լ�
    private void Awake()
    {
        hitMat = Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Mat/HitMat.mat");
        spriteLitMat = Managers.Resource.Load<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Sprite-Lit-Default.mat");
        playerAnim = GetComponent<Animator>();  
        GameObject.FindGameObjectWithTag("Attack").GetComponent<Button>().onClick.AddListener(Attack);
    }
    public void Attack()
    {
        // TODO : �� ���ݽ� ���� �ִϸ��̼� �۵� �� ������ �ǰ����� üũ
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;
        playerAnim.SetTrigger("Attack");
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss"))
            {
                CinemachineCameraShaking.Instance.CameraShake();
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, gameObject, GameManager.Instance.Player.playerBase.CritChance);
                Material targetMat = enemys[i].GetComponent<Renderer>().material;
                StartCoroutine(ChangeMat(targetMat, enemys[i].GetComponent<SpriteRenderer>().sprite.texture));
            }
        }
    }
   IEnumerator ChangeMat(Material mat,Texture2D texture)
    {
        print(texture.name);
        hitMat.SetTexture("_Texture2D", texture);
        mat = hitMat;
        yield return new WaitForSeconds(0.1f);
        mat = spriteLitMat;
    }
}
