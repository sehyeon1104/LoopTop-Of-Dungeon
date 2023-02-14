using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Player : MonoSingleton<Player> , IHittable , IAgent
{
    public PlayerBase pBase;
    private bool isPDamaged = false;
    private bool isPDead = false;

    [SerializeField]
    private float InvincibleTime = 0.2f;    // 무적시간

    AgentInput agentInput = null;
    Animator playerAnim = null;

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
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        agentInput.Attack.AddListener(Attack);
        InitCooltimeBools();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Boss.Instance.isBDead)
            {
                TransformGhost();
                Boss.Instance.gameObject.SetActive(false);
                UIManager.Instance.pressF.gameObject.SetActive(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            pBase.Exp += 100;
        }
    }

    public IEnumerator IEDamaged()
    {
        GetHit.Invoke();
        yield return new WaitForSeconds(InvincibleTime);

        isPDamaged = false;

        yield return null;
    }

    public void OnDamage(float damage, GameObject damageDealer, float critChance)
    {
        if (isPDamaged)
            return;

        isPDamaged = true;

        // TODO : 피격 애니메이션 재생

        pBase.Hp -= (int)damage;
        StartCoroutine(IEDamaged());
    }

    public void Dead()
    {
        Debug.Log("사망");
        gameObject.SetActive(false);
    }
}
