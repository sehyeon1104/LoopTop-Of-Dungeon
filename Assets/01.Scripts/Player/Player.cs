using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
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
    public UnityEvent HPRelatedItemEffects { get; private set; } = new UnityEvent();
    // 공격 관련 아이템 효과
    public UnityEvent AttackRelatedItemEffects { get; private set; } = new UnityEvent();
    // 스킬 관련 아이템 효과
    public UnityEvent<int> SkillRelatedItemEffects { get; private set; } = new UnityEvent<int>();
    // 피격 관련 아이템 효과
    public UnityEvent OnDamagedRelatedItemEffects { get; private set; } = new UnityEvent();
    // 대쉬 관련 아이템 효과
    public UnityEvent DashRelatedItemEffects { get; private set; } = new UnityEvent();
    // 사망 관련 아이템 효과
    public UnityEvent DeadRelatedItemEffects { get; private set; } = new UnityEvent();

    public Vector3 hitPoint { get; private set; }

    [HideInInspector]
    public float dmgMul = 1f;
    [HideInInspector]
    public float dmgAdd = 0f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            var dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
            dropItemObj.transform.position = transform.position;
            dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Common);
        }
    }
    private void Awake()
    {
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

    public void OnDamage(float damage, float critChance, Poolable hitEffect = null)
    {
        if (isPDamaged || playerBase.IsPDead || invincibility)
            return;

        damage = damage * dmgMul + dmgAdd;

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
        if (ItemManager.Instance.curItemDic.ContainsKey("LifeInsurance"))
        {
            ItemAbility.Items[404].Use();
            return;
        }

        playerBase.IsPDead = true;
        CinemachineCameraShaking.Instance.CameraShake();
        PlayerVisual.Instance.PlayerAnimator.SetTrigger("Death");
        StartCoroutine(GameoverPlayer());
    }

    public IEnumerator GameoverPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.gameOverUI.UpdateValue();
        yield return new WaitForSeconds(2.3f);
        UIManager.Instance.gameOverUI.ToggleGameOverPanel();
        playerVisual.SetActive(false);
    }

    public void RevivePlayer()
    {
        playerVisual.SetActive(true);
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
