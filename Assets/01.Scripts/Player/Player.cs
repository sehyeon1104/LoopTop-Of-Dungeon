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

// �÷��̾� ��ü�� �̱����� ���� �ʾƾ���
public class Player : MonoBehaviour, IHittable , IAgent
{
    public PlayerBase pBase;
    public Volume hitVolume;

    private bool isPDamaged = false;
    public bool isPDead { private set; get; } = false;

    [SerializeField]
    private float reviveInvincibleTime = 2f;
    [SerializeField]
    private float invincibleTime = 0.2f;    // �����ð�
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
        rb = GetComponent<Rigidbody2D>();
        _joystick = FindObjectOfType<FloatingJoystick2D>();
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
        // TODO : �ǰ� �ִϸ��̼� ���
        pBase.Hp -= (int)damage;
        StartCoroutine(IEDamaged());

        UIManager.Instance.HpUpdate();
        CinemachineCameraShaking.Instance.CameraShake(5,0.4f);
    }

    public void Dead()
    {

        isPDead = true;
        // TODO : �÷��̾� �״� ��ǽ���, ����� ������ �� ���ӿ����г� Ȱ��ȭ
        CinemachineCameraShaking.Instance.CameraShake();
        UIManager.Instance.ToggleGameOverPanel();
        gameObject.SetActive(false);
    }

    public void RevivePlayer()
    {
        gameObject.SetActive(true); // �ӽ�
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
