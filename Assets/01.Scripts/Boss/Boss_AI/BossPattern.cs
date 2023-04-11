using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPattern : MonoBehaviour
{
    [HideInInspector] public int NowPhase = 1;

    #region Serialize
    
    [Space]
    
    [Header("보스 옵션")]

    [Tooltip("페이즈별 보스 스킬 개수")]
    [SerializeField] private int[] phase_patternCount;
    private WaitForSeconds patternDelay = new WaitForSeconds(1.5f);
    [Tooltip("보스 2페이즈 색상")]
    [SerializeField] public Color Phase_Two_Color;

    [Space]

    [Header("페이즈별 애니메이션")]
    public AnimationClip[] Phase_One_AnimArray;
    public AnimationClip[] Phase_Two_AnimArray;

    #endregion
    #region init
    protected int[] patternCount = new int[6];

    protected bool[] isThisSkillCoolDown = new bool[6];
    protected bool isCanUseFinalPattern = true;
    protected bool isUsingFinalPattern = false;

    protected bool nowBPhaseChange = false;
    
    int patternChoice = 0;

    #endregion

    public void Init()
    {
        patternDelay = new WaitForSeconds(1.5f);

        isCanUseFinalPattern = true;
        isUsingFinalPattern = false;

        StartCoroutine(RandomPattern());
        StartCoroutine(ChangePhase());
    }
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Boss.Instance.OnDamage(20, 0);
        }
        if (Boss.Instance.isBDead)
        {
            Boss.Instance.actCoroutine = null;
            StopAllCoroutines();
        }

        for (int i = 0; i < Boss.Instance.sprites.Count; i++)
        {
            Boss.Instance.sprites[i].color = Phase_Two_Color;
        }
    }

    private IEnumerator ChangePhase()
    {
        yield return new WaitUntil(() => NowPhase == 1 && Boss.Instance.Base.Hp <= 0);
        isThisSkillCoolDown[patternChoice] = false;

        if (Boss.Instance.actCoroutine != null)
            StopCoroutine(Boss.Instance.actCoroutine);

        Boss.Instance.actCoroutine = null;

        nowBPhaseChange = true;
        Boss.Instance.isBInvincible = true;

        yield return patternDelay;

        while (Boss.Instance.Base.Hp < Boss.Instance.Base.MaxHp)
        {
            Boss.Instance.Base.Hp += 2;
            yield return null;
        }
        Boss.Instance.Base.Hp = Boss.Instance.Base.MaxHp;
        isCanUseFinalPattern = true;
        isUsingFinalPattern = false;
        patternDelay = new WaitForSeconds(1.2f);
        NowPhase = 2;

        for (int i = 0; i < Boss.Instance.sprites.Count; i++)
        {
            Boss.Instance.sprites[i].color = Phase_Two_Color;
        }

        Boss.Instance.bossAnim.overrideController = Boss.Instance.bossAnim.SetSkillAnimation(Boss.Instance.bossAnim.overrideController);

        yield return patternDelay;

        Boss.Instance.isBInvincible = false;
        nowBPhaseChange = false;
        Boss.Instance.Phase2();
    }

    private IEnumerator RandomPattern()
    {
        while (!Boss.Instance.isBDead)
        {
            yield return null;

            if (nowBPhaseChange) continue;

            patternChoice = Random.Range(0, phase_patternCount[NowPhase - 1]);
            patternCount[patternChoice] = GetRandomCount(patternChoice);

            if (isThisSkillCoolDown[patternChoice]) continue;

            if (Boss.Instance.actCoroutine == null)
            {
                Boss.Instance.bossAnim.anim.SetBool(Boss.Instance._hashMove, false);
                Boss.Instance.bossAnim.anim.SetInteger(Boss.Instance._hashSkill, patternChoice);

                if (isCanUseFinalPattern && isUsingFinalPattern)
                {
                    isCanUseFinalPattern = false;

                    patternCount[5] = GetRandomCount(5);
                    Boss.Instance.bossAnim.anim.SetInteger(Boss.Instance._hashSkill, 5);
                    Boss.Instance.actCoroutine = StartCoroutine(PatternFinal(patternCount[5]));

                    isUsingFinalPattern = false;
                }
                else
                {
                    isThisSkillCoolDown[patternChoice] = true;
                    Debug.Log($"현재 패턴 : {patternChoice}");
                    switch (patternChoice)
                    {
                        case 0:
                            Boss.Instance.actCoroutine = StartCoroutine(Pattern1(patternCount[0]));
                            break;
                        case 1:
                            Boss.Instance.actCoroutine = StartCoroutine(Pattern2(patternCount[1]));
                            break;
                        case 2:
                            Boss.Instance.actCoroutine = StartCoroutine(Pattern3(patternCount[2]));
                            break;
                        case 3:
                            Boss.Instance.actCoroutine = StartCoroutine(Pattern4(patternCount[3]));
                            break;
                        case 4:
                            Boss.Instance.actCoroutine = StartCoroutine(Pattern5(patternCount[4]));
                            break;
                    }
                }
            }

            yield return new WaitUntil(() => Boss.Instance.actCoroutine == null);
            StartCoroutine(CoolDownCheck(patternChoice));
            yield return patternDelay;
        }
    }
    public IEnumerator CoolDownCheck(int nowSkill)
    {
        Debug.Log($"쿨다운 패턴 : {patternChoice}");
        print($"Skill{nowSkill}Started! Time : {Time.time}");
        yield return new WaitForSeconds(10f);
        print($"Skill{nowSkill} CoolDowned! Time : {Time.time}");
        isThisSkillCoolDown[nowSkill] = false;
    }

    public virtual int GetRandomCount(int choisedPattern)
    {
        return 0;
    }

    #region Patterns
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
    #endregion

}
