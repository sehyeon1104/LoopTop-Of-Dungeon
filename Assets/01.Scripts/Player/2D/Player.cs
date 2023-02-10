using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoSingleton<Player>, IHitAble
{
    public PlayerBase pBase;

    private bool isPDamaged = false;
    private bool isPDead = false;

    [SerializeField]
    private float InvincibleTime = 0.2f;    // 무적시간

    AgentInput agentInput = null;
    Animator playerAnim = null;

    private void Awake()
    {
        pBase = new PlayerBase();
        agentInput = GetComponent<AgentInput>();
        playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        agentInput.Attack.AddListener(Attack);
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
    }

    // TODO : 적과 플레이어의 거리에 따라 피격판정

    public void GetHit(float damage, GameObject damageDealer, float critChance)
    {
        if (isPDamaged)
            return;

        isPDamaged = true;

        // TODO : 피격 애니메이션 재생

        pBase.Hp -= (int)damage;
        StartCoroutine(IEDamaged());
    }

    public IEnumerator IEDamaged()
    {
        yield return new WaitForSeconds(InvincibleTime);

        isPDamaged = false;

        yield break;
    }
}
