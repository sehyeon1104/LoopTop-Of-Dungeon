using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPattern : MonoBehaviour
{
    [HideInInspector] public int NowPhase = 1;

    #region Serialize
    [Header("이동 관련")]

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float minDistance;
    
    [Space]
    
    [Header("보스 옵션")]

    [Tooltip("보스방 입장 시 초기 대기 상태")]
    [SerializeField] private float waitTime = 1f;
    [Tooltip("페이즈별 보스 스킬 개수")]
    [SerializeField] private int[] phase_patternCount;
    [Tooltip("보스 패턴 후딜레이")]
    [SerializeField] private float patternDelay;

    [Space]

    [Header("애니메이션")]

    [SerializeField] protected AnimationClip idleClip;
    [SerializeField] protected AnimationClip moveClip;

    [Tooltip("1페이즈 애니메이션")]
    [SerializeField] protected AnimationClip[] Phase_One_AnimArray;
    [Tooltip("2페이즈 애니메이션")]
    [SerializeField] protected AnimationClip[] Phase_Two_AnimArray;
    #endregion
    #region init
    protected int[] patternCount = new int[6];

    protected Transform player;
    protected Animator anim;
    protected Coroutine attackCoroutine = null;

    protected bool[] isThisSkillCoolDown = new bool[6];
    protected bool isCanUseFinalPattern = true;
    protected bool isUsingFinalPattern = false;

    protected Vector3 constScale;
    protected AnimatorOverrideController overrideController = new AnimatorOverrideController();
    #endregion
    #region AnimHash
    protected readonly int _hashMove = Animator.StringToHash("Move");
    protected readonly int _hashSkill = Animator.StringToHash("Skill");
    protected readonly int _hashAttack = Animator.StringToHash("Attack");
    protected readonly int _hashDeath = Animator.StringToHash("Death");
    #endregion

    private void Start()
    {
        player = GameManager.Instance.Player.transform;
        anim = GetComponent<Animator>();

        isCanUseFinalPattern = true;
        isUsingFinalPattern = false;

        AnimInit();

        StartCoroutine(RandomPattern());
        StartCoroutine(ChangePase());

        constScale = transform.localScale;
    }
    protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Boss.Instance.OnDamage(20, gameObject, 0);
        }
        MoveToPlayer();
        if(Boss.Instance.isBDead)
        {
            attackCoroutine = null;
            StopAllCoroutines();
        }
    }

    public void AnimInit()
    {
        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;

        if (moveClip != null) overrideController["Moving"] = moveClip;
        if (idleClip != null) overrideController["Idle"] = idleClip;

        overrideController = SetSkillAnimation(overrideController);

        anim.runtimeAnimatorController = overrideController;
    }
    public AnimatorOverrideController SetSkillAnimation(AnimatorOverrideController overrideController)
    {
        for (int i = 0; i < 5; i++)
        {
            switch (NowPhase)
            {
                case 1:
                    if (Phase_One_AnimArray[i] != null) overrideController[$"Skill{i + 1}"] = Phase_One_AnimArray[i];
                    break;
                case 2:
                    if (Phase_Two_AnimArray[i] != null) overrideController[$"Skill{i + 1}"] = Phase_Two_AnimArray[i];
                    break;
            }
        }

        return overrideController;
    }

    public void MoveToPlayer()
    {
        if (attackCoroutine != null || Boss.Instance.isBDead || Boss.Instance.isBInvincible) return;

        float playerDistance = Vector2.Distance(player.position, transform.position);
        if (playerDistance <= minDistance) return;

        anim.SetBool(_hashMove,true);
        Vector2 dir = (player.position - transform.position);
        Vector3 scale = transform.localScale;

        CheckFlipValue(dir, scale);

        transform.Translate((Vector2.up * dir.normalized + Vector2.right * Mathf.Sign(scale.x)) * Time.deltaTime * moveSpeed);
    }
    public Vector3 CheckFlipValue(Vector2 dir, Vector3 scale)
    {
        scale.x = Mathf.Sign(dir.x) * constScale.x;

        if (Mathf.Abs(dir.x) > 0.2f)
            transform.localScale = scale;

        return scale;
    }

    private IEnumerator ChangePase()
    {
        yield return new WaitUntil(() => NowPhase == 1 && Boss.Instance.Base.Hp <= 0);

        StopCoroutine(RandomPattern());
        if(attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        Boss.Instance.isBInvincible = true;

        yield return patternDelay;

        while (Boss.Instance.Base.Hp < Boss.Instance.Base.MaxHp)
        {
            Boss.Instance.Base.Hp += 2;
            yield return null;
        }
        Boss.Instance.Base.Hp = Boss.Instance.Base.MaxHp;
        isCanUseFinalPattern = true;
        NowPhase = 2;
        overrideController = SetSkillAnimation(overrideController);

        yield return patternDelay;

        Boss.Instance.isBInvincible = false;
        Boss.Instance.Phase2();
        StartCoroutine(RandomPattern());
        attackCoroutine = null;
    }

    private IEnumerator RandomPattern()
    {
        yield return new WaitForSeconds(waitTime);

        while (!Boss.Instance.isBDead)
        {
            yield return null;

            if (Boss.Instance.isBInvincible)
            {
                continue;
            }

            int patternChoice = 0;
            patternChoice = Random.Range(0, phase_patternCount[NowPhase - 1]);
            patternCount[patternChoice] = GetRandomCount(patternChoice);

            if (isThisSkillCoolDown[patternChoice]) continue;

            if (attackCoroutine == null)
            {
                anim.SetBool(_hashMove, false);
                anim.SetInteger(_hashSkill, patternChoice);

                if (isCanUseFinalPattern && isUsingFinalPattern)
                {
                    isCanUseFinalPattern = false;

                    patternCount[5] = GetRandomCount(5);
                    attackCoroutine = StartCoroutine(PatternFinal(patternCount[5]));

                    isUsingFinalPattern = false;
                }
                else
                {
                    isThisSkillCoolDown[patternChoice] = true;
                    switch (patternChoice)
                    {
                        case 0:
                            attackCoroutine = StartCoroutine(Pattern1(patternCount[0]));
                            break;
                        case 1:
                            attackCoroutine = StartCoroutine(Pattern2(patternCount[1]));
                            break;
                        case 2:
                            attackCoroutine = StartCoroutine(Pattern3(patternCount[2]));
                            break;
                        case 3:
                            attackCoroutine = StartCoroutine(Pattern4(patternCount[3]));
                            break;
                        case 4:
                            attackCoroutine = StartCoroutine(Pattern5(patternCount[4]));
                            break;
                    }
                }
            }

            yield return null;
            
            if (attackCoroutine != null)
            {
                yield return new WaitUntil(() => attackCoroutine == null);
                yield return new WaitForSeconds(patternDelay);
                StartCoroutine(CoolDownCheck(patternChoice));
            }

        }
        attackCoroutine = null;
        StopAllCoroutines();
    }
    public IEnumerator CoolDownCheck(int nowSkill)
    {
        yield return new WaitForSeconds(3f);
        isThisSkillCoolDown[nowSkill] = false;
    }

    public virtual int GetRandomCount(int choisedPattern)
    {
        return 0;
    }

    public virtual IEnumerator Pattern1(int count = 0)
    {
        yield break;
    }
    public virtual IEnumerator Pattern2(int count = 0)
    {
        yield break;
    }
    public virtual IEnumerator Pattern3(int count = 0)
    {
        yield break;
    }
    public virtual IEnumerator Pattern4(int count = 0)
    {
        yield break;
    }
    public virtual IEnumerator Pattern5(int count = 0)
    {
        yield break;
    }
    public virtual IEnumerator PatternFinal(int count = 0)
    {
        yield break;
    }

}
