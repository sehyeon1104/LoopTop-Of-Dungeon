using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public partial class Player : MonoSingleton<Player> , IHittable , IAgent
{
    public PlayerBase pBase;
    public Volume hitVolume;
    private bool isPDamaged = false;
    public bool isPDead { private set; get; } = false;

    [SerializeField]
    private float reviveInvincibleTime = 2f;
    [SerializeField]
    private float invincibleTime = 0.2f;    // 무적시간

    AgentInput agentInput = null;
    Animator playerAnim = null;
    SpriteRenderer playerSprite = null;

    public Sprite playerVisual { private set; get; }

    public Vector3 hitPoint { get; private set; }

   [field:SerializeField] public UnityEvent GetHit { get; set; }
   [field:SerializeField] public UnityEvent OnDie { get; set; }

    private void Awake()
    {
        pBase = new PlayerBase();
        if (playerTransformDataSO == null)
        {
            playerTransformDataSO = playerTransformDataSOArr[0];
        }
        agentInput = GetComponent<AgentInput>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        SkillShuffle();
        UIManager.Instance.SkillNum(randomSkillNum);
        agentInput.Attack.AddListener(Attack);
        //if(StageManager.Instance != null)
        //{
        //    transform.position = StageManager.Instance.SetPlayerSpawnPos().position;
        //}
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            StageManager.Instance.SetWallGrid();

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Boss.Instance.isBDead)
            {
                _joystick.enabled = false;
                skillSelect.SetActive(true);
                Time.timeScale = 0;
                TransformGhost();
                Boss.Instance.gameObject.SetActive(false);
                UIManager.Instance.pressF.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            pBase.Exp += 100;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            pBase.Hp += 1;
        }
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
        if (isPDamaged)
            return;

        if (Random.Range(1, 101) <= critChance)
        {
            damage *= 1.5f;
        }

        isPDamaged = true;

        // TODO : 피격 애니메이션 재생

        pBase.Hp -= (int)damage;
        UIManager.Instance.HpUpdate();

        if (isPDead)
            return;

        StartCoroutine(IEDamaged());
        StartCoroutine(IEHitMotion());
        CinemachineCameraShaking.Instance.CameraShake(5,0.4f);
    }

    public void Dead()
    {
        if (isPDead)
            return;

        isPDead = true;
        // TODO : 플레이어 죽는 모션실행, 모션이 끝났을 때 게임오버패널 활성화

        CinemachineCameraShaking.Instance.CameraShake();
        UIManager.Instance.ToggleGameOverPanel();
        gameObject.SetActive(false);
    }

    public void RevivePlayer()
    {
        gameObject.SetActive(true); // 임시
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
