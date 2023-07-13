using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
//using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Debug = Rito.Debug;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour, IHittable
{

    public PlayerBase playerBase = new PlayerBase();
    private GameObject playerVisual;
    private bool invincibility = false;
    public bool IsInvincibility
    {
        get => invincibility;
        set => invincibility = value;
    }
    private bool isPDamaged = false;
    [SerializeField]
    private float reviveInvincibleTime = 2f;
    [SerializeField]
    private float invincibleTime = 0.2f;

    // HP 관련 아이템 효과
    [field: SerializeField]
    public UnityEvent HPRelatedItemEffects;
    // 공격 관련 아이템 효과
    [field:SerializeField]
    public UnityEvent AttackRelatedItemEffects { get; private set; }
    // 스킬 관련 아이템 효과
    [field:SerializeField]
    public UnityEvent SkillRelatedItemEffects { get; private set; }
    // 피격 관련 아이템 효과
    [field: SerializeField]
    public UnityEvent OnDamagedRelatedItemEffects { get; private set; }
    public Vector3 hitPoint { get; private set; }
    private void Awake()
    {
        HPRelatedItemEffects.RemoveListener(Test);
        HPRelatedItemEffects.AddListener(Test);

        UIManager.Instance.reviveButton.onClick.AddListener(RevivePlayer);
        playerVisual = transform.Find("PlayerVisual").gameObject;
    }
    private void Start()
    {

        PlayerVisual.Instance.UpdateVisual(playerBase.PlayerTransformData);
    }

    public IEnumerator IEDamaged(float damage = 0)
    {
        PlayerVisual.Instance.StartHitMotion(damage);
        yield return new WaitForSeconds(invincibleTime);

        isPDamaged = false;
        yield return null;
    }

    private float damageMultiples = 1;
    public float DamageMultiples
    {
        get => damageMultiples;
        set => damageMultiples = value;
    }

    public void Test()
    {
        Debug.Log("Test1");
    }

    public void OnDamage(float damage, float critChance, Poolable hitEffect = null)
    {
        if (isPDamaged || playerBase.IsPDead || invincibility)
            return;
        
        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
        }

        OnDamagedRelatedItemEffects?.Invoke();
        playerBase.Hp -= (int)(damage * damageMultiples);

        isPDamaged = true;
        
        if (playerBase.Hp <= 0)
            Dead();
        else
        {
            StartCoroutine(IEDamaged(damage));
            CinemachineCameraShaking.Instance.CameraShake(5, 0.2f);
        }

        // HPRelatedItemEffects?.Invoke();
    }
    
    public void Dead()
    {
        playerBase.IsPDead = true;
        CinemachineCameraShaking.Instance.CameraShake();
        PlayerVisual.Instance.PlayerAnimator.SetTrigger("Death");
        StartCoroutine(GameoverPlayer());
    }

    public IEnumerator GameoverPlayer()
    {
        yield return new WaitForSeconds(2.5f);
        UIManager.Instance.ToggleGameOverPanel();
        playerVisual.SetActive(false);
    }

    public void RevivePlayer()
    {
        playerVisual.SetActive(true);
        UIManager.Instance.ToggleGameOverPanel();
        //UIManager.Instance.CloseGameOverPanel();
        playerBase.Hp = playerBase.MaxHp;
        playerBase.IsPDead = false;
        StartCoroutine(Invincibility(reviveInvincibleTime));
    }

    public IEnumerator Invincibility(float time)
    {
        isPDamaged = true;
        yield return new WaitForSeconds(time);
        isPDamaged = false;
    }

}
