using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Debug = Rito.Debug;

public class Player : PlayerBase, IHittable, IAgent
{

    private bool isPDamaged = false;
    [SerializeField]
    private float reviveInvincibleTime = 2f;
    [SerializeField]
    private float invincibleTime = 0.2f;
    private Rigidbody2D rb;
    public Vector3 hitPoint { get; private set; }
    [field: SerializeField] public UnityEvent GetHit { get; set; }
    [field: SerializeField] public UnityEvent OnDie { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetPlayerStat();
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
        if (isPDamaged || IsPDead)
            return;

        GetHit.Invoke();
        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
        }
        isPDamaged = true;
        Hp -= (int)damage;
        StartCoroutine(IEDamaged());
        UIManager.Instance.HpUpdate();
        CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);
    }

    public void Dead()
    {

        IsPDead = true;
        CinemachineCameraShaking.Instance.CameraShake();
        UIManager.Instance.ToggleGameOverPanel();
        gameObject.SetActive(false);
    }

    public void RevivePlayer()
    {
        gameObject.SetActive(true);
        UIManager.Instance.ToggleGameOverPanel();
        Hp = MaxHp;
        IsPDead = false;
        StartCoroutine(Invincibility(reviveInvincibleTime));
    }

    public IEnumerator Invincibility(float time)
    {
        isPDamaged = true;
        yield return new WaitForSeconds(time);
        isPDamaged = false;
    }

}
