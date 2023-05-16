using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoSingleton<Boss>, IHittable
{
    public BossBase Base;

    public BossPattern bossPattern { private set; get; }
    public BossMove bossMove { private set; get; }
    public BossAnim bossAnim { private set; get; }

    public Transform player;

    public Coroutine actCoroutine = null;

    // public MultiGage.TargetGageValue TargetGage;

    private BossUI bossUI;


    public bool isBDamaged { set; get; } = false;
    public bool isBInvincible { set; get; } = false;
    public bool isBDead { private set; get; } = false;
    public bool isStartMotionPlaying { private set; get; } = false;

    public Vector3 hitPoint { get; }

    public Dictionary<int, SpriteRenderer> sprites = new Dictionary<int, SpriteRenderer>();

    #region AnimHash
    public readonly int _hashMove = Animator.StringToHash("Move");
    public readonly int _hashSkill = Animator.StringToHash("Skill");
    public readonly int _hashAttack = Animator.StringToHash("Attack");
    public readonly int _hashDeath = Animator.StringToHash("Death");
    #endregion

    private void Awake()
    {
        Base = new BossBase();

        isStartMotionPlaying = true;

        bossPattern = GetComponent<BossPattern>();
        bossMove = GetComponent<BossMove>();
        bossAnim = GetComponent<BossAnim>();

        bossMove.Init();
        bossAnim.Init();

        SpriteRenderer[] sprite = GetComponentsInChildren<SpriteRenderer>();

        for(int i = 0; i < GetComponentsInChildren<SpriteRenderer>().Length; i++)
        {
            sprites.Add(i,sprite[i]);
        }

    }

    private void Start()
    {
        bossUI = FindObjectOfType<BossUI>();

        player = GameManager.Instance.Player.transform;

        UpdateBossHP();
    }

    public IEnumerator IEHitAction()
    {
        // TODO : 피격 애니메이션
        // TODO : 피격시 받은 데미지 표시

        //StartCoroutine(CameraShaking.Instance.IECameraShakeOnce());

        yield return new WaitForSeconds(0.05f);
        isBDamaged = false;

        yield break;
    }

    public void Die()
    {
        if (isBDead) return;
        
        // StartCoroutine(CameraShaking.Instance.IECameraShakeMultiple(2f));
        // MultiGage.Instance.ObserveEnd();

        isBDead = true;

        bossAnim.anim.SetTrigger("Death");

        actCoroutine = null;
        StopAllCoroutines();
        //gameObject.SetActive(false);
    }

    public void OnDamage(float damage, float critChance)
    {
        if (isBDead) return;
        if (isBDamaged) return;
        if (isBInvincible) return;

        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
            StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject, true));
        }
        else
        {
            StartCoroutine(EnemyUIManager.Instance.showDamage(damage, gameObject));
        }

        isBDamaged = true;

        if(Base.Shield > 0)
        {
            Base.Shield -= (int)damage;
        }
        else
        {
            Base.Hp -= (int)damage;
        }

        UpdateBossHP();
        StartCoroutine(IEHitAction());

        if (Base.Hp <= 0 && bossPattern.NowPhase == 2)
        {
            Die();
            return;
        }
    }

    public void UpdateBossHP()
    {
        bossUI.UpdateHpBar();
        // TargetGage.value = Base.Hp;
    }

    public void UpdateBossShield()
    {
        bossUI.UpdateShieldBar();
    }

    public void Phase2()
    {
        bossUI.TogglePhase2Icon();
    }

    public void StartBossAct()
    {
        isStartMotionPlaying = false;

        bossAnim.AnimInit();
        bossPattern.Init();
    }
}
