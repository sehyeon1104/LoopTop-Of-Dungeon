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

    public Vector3 hitPoint { get; private set; }
    private void Awake()
    {
        playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));
    }
    private void Start()
    {
        playerBase.PlayerTransformData = playerBase.PlayerTransformDataSOList[(int)playerBase.PlayerTransformTypeFlag];
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
        gameObject.SetActive(false);
    }

    public void RevivePlayer()
    {   
        gameObject.SetActive(true);
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
