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

    public UnityEvent HPRelatedItemEffects { get; private set; }
    public Vector3 hitPoint { get; private set; }
    private void Awake()
    {
        UIManager.Instance.reviveButton.onClick.AddListener(RevivePlayer);
        playerVisual = transform.Find("PlayerVisual").gameObject;
    }
    private void Start()
    {
        if (HPRelatedItemEffects == null)
            HPRelatedItemEffects = new UnityEvent();

        PlayerVisual.Instance.UpdateVisual(playerBase.PlayerTransformData);
    }

    public IEnumerator IEDamaged(float damage = 0)
    {
        PlayerVisual.Instance.StartHitMotion(damage);
        yield return new WaitForSeconds(invincibleTime);

        isPDamaged = false;
        yield return null;
    }

    public void OnDamage(float damage, float critChance)
    {
        if (isPDamaged || playerBase.IsPDead || invincibility)
            return;
        
        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
        }

        playerBase.Hp -= (int)damage;

        isPDamaged = true;
        
        if (playerBase.Hp <= 0)
            Dead();
        else
        {
            StartCoroutine(IEDamaged(damage));
            CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);
        }

        HPRelatedItemEffects.Invoke();
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
