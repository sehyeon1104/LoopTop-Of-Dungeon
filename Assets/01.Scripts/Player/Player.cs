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

public class Player : MonoBehaviour, IHittable
{
    public PlayerBase playerBase = new PlayerBase();   
    private bool isPDamaged = false;
    [SerializeField]
    private float reviveInvincibleTime = 2f;
    [SerializeField]
    private float invincibleTime = 0.2f;
    public Vector3 hitPoint { get; private set; }
    private void Start()
    {
        playerBase.SetPlayerStat();
    }
    public IEnumerator IEDamaged()
    {
        PlayerVisual.Instance.StartHitMotion();
        yield return new WaitForSeconds(invincibleTime);
        isPDamaged = false;
        yield return null;
    }

    public void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
        if (isPDamaged || playerBase.IsPDead)
            return;

        
        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
        }
        isPDamaged = true;
        playerBase.Hp -= (int)damage;
        if(playerBase.Hp < 0) 
            Dead();
        StartCoroutine(IEDamaged());
        UIManager.Instance.HpUpdate();
        CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);
    }

    public void Dead()
    {

        playerBase.IsPDead = true;
        CinemachineCameraShaking.Instance.CameraShake();
        UIManager.Instance.ToggleGameOverPanel();
        gameObject.SetActive(false);
    }

    public void RevivePlayer()
    {
        gameObject.SetActive(true);
        UIManager.Instance.ToggleGameOverPanel();
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
