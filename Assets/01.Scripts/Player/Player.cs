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

// 플레이어 자체는 싱글톤을 쓰지 않아야해
public class Player : MonoBehaviour, IHittable , IAgent
{
    public PlayerBase pBase;
    public Volume hitVolume;

    private bool isPDamaged = false;
    public bool isPDead { private set; get; } = false;

    [SerializeField]
    private float reviveInvincibleTime = 2f;
    [SerializeField]
    private float invincibleTime = 0.2f;    // 무적시간
    private PlayerTransformation transformat;
    private AgentInput agentInput = null;
    private Animator playerAnim = null;
    private SpriteRenderer playerSprite = null;
    private PlayerSkillData playerSkillData =null;
    public Sprite playerVisual { private set; get; }
    private Rigidbody2D rb;
    private Joystick _joystick = null;
    public Vector3 hitPoint { get; private set; }
    [SerializeField] UnityEvent transformation;
   [field:SerializeField] public UnityEvent GetHit { get; set; }
   [field:SerializeField] public UnityEvent OnDie { get; set; }

    private void Awake()
    {
        transformat = GetComponent<PlayerTransformation>();
        InitPlayerData();
    }

    private void InitPlayerData()
    {
        pBase = new PlayerBase();

        //transformat.playerTransformDataSOArr = new PlayerSkillData[2];

        //transformat.playerTransformDataSOArr[0] = Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset");
        //transformat.playerTransformDataSOArr[1] = Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset");

        agentInput = GetComponent<AgentInput>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _joystick = FindObjectOfType<FloatingJoystick2D>();
    }

    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        { 
            if (Boss.Instance.isBDead)
            {   
                transformation.Invoke();
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            pBase.Hp -= 3;
        }
    }
    public void TransformAilen()
    {
        _joystick.enabled = false;
       
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
    public IEnumerator IEHitMotion()
    {
        float timer = 0f;

        playerSprite.color = Color.red;
        Managers.Pool.PoolManaging("10.Effects/player/Hit_main", transform.position, Quaternion.identity);
        Managers.Pool.PoolManaging("10.Effects/player/Hit_sub", transform.position, Quaternion.identity);
        Managers.Sound.Play("SoundEffects/Player/Damaged.wav");
        while (timer <= 0.25f)
        {
            timer += Time.unscaledDeltaTime;

            Time.timeScale -= 0.015f;
            hitVolume.weight += 0.05f;

            yield return null;
        }
        Time.timeScale = 1f;
        hitVolume.weight = 0;
        playerSprite.color = Color.white;
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
        pBase.Hp -= (int)damage;
        StartCoroutine(IEDamaged());
        StartCoroutine(IEHitMotion());

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
        UIManager.Instance.ToggleGameOverPanel();
        pBase.Hp = pBase.MaxHp;
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
