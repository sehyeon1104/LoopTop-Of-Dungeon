using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

// 플레이어 자체는 싱글톤을 쓰지 않아야해
public class Player : PlayerBase, IHittable , IAgent
{
    private bool isPDamaged = false;
    public bool isPDead { private set; get; } = false;

    [SerializeField]
    private float reviveInvincibleTime = 2f;
    [SerializeField]
    private float invincibleTime = 0.2f;    // 무적시간
    public Sprite playerVisual { private set; get; }
    Rigidbody2D rb;
    public Vector3 hitPoint { get; private set; }
    [SerializeField] UnityEvent transformation;
   [field:SerializeField] public UnityEvent GetHit { get; set; }
   [field:SerializeField] public UnityEvent OnDie { get; set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetPlayerStat();
    }



    // 임시방편
    public void TransformAilen()
    {  
        Time.timeScale = 0;
        Boss.Instance.gameObject.SetActive(false);
        UIManager.Instance.pressF.gameObject.SetActive(false);
    }
    public IEnumerator IEDamaged()
    {
        GetHit.Invoke();
        yield return new WaitForSeconds(invincibleTime);

        isPDamaged = false;

        yield return null;
    }
    public void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
        if (isPDamaged || isPDead)
            return;

        GetHit.Invoke();
        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
        }
        isPDamaged = true;
        // TODO : 피격 애니메이션 재생
        Hp -= (int)damage;
        StartCoroutine(IEDamaged());

        UIManager.Instance.HpUpdate();
        CinemachineCameraShaking.Instance.CameraShake(5,0.4f);
    }

    public void Dead()
    {

        isPDead = true;
        // TODO : 플레이어 죽는 모션실행, 모션이 끝났을 때 게임오버패널 활성화
        CinemachineCameraShaking.Instance.CameraShake();
        UIManager.Instance.ToggleGameOverPanel();
        gameObject.SetActive(false);
    }

    public void RevivePlayer()
    {
        gameObject.SetActive(true); // 임시
        Hp = MaxHp;
        isPDead = false;
        StartCoroutine(Invincibility(reviveInvincibleTime));
    }

    public IEnumerator Invincibility(float time)
    {
        isPDamaged = true;
        yield return new WaitForSeconds(time);
        isPDamaged = false; 
    }

}
