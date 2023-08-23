using Cinemachine;
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
    [SerializeField] protected int[] phase_patternCount;
    protected WaitForSeconds patternDelay = new WaitForSeconds(1.5f);

    [Space]

    [Header("페이즈별 애니메이션")]
    public AnimationClip[] Phase_One_AnimArray;
    public AnimationClip[] Phase_Two_AnimArray;

    public CinemachineVirtualCamera boss2PhaseVcam;

    #endregion
    #region init
    protected int[] patternCount = new int[6];
    protected int[] patternWeight = new int[6];

    protected bool[] isThisSkillCoolDown = new bool[6];
    protected bool isCanUseFinalPattern = true;
    protected bool isUsingFinalPattern = false;

    protected bool nowBPhaseChange = false;
    
    protected int patternChoice = 0;

    public Vector3 hitPoint => throw new System.NotImplementedException();

    #endregion

    public void Init()
    {
        patternDelay = new WaitForSeconds(2f);

        isCanUseFinalPattern = true;
        isUsingFinalPattern = false;

        SetPatternWeight();

        StartCoroutine(RandomPattern());
        StartCoroutine(ChangePhase());
    }
    protected void Update()
    {
        if (Boss.Instance.isBDead)
        {
            Boss.Instance.actCoroutine = null;
            StopAllCoroutines();
        }
        if (Input.GetKeyDown(KeyCode.G))
            Boss.Instance.OnDamage(100, 0);
    }

    protected virtual IEnumerator ChangePhase()
    {
        yield return new WaitUntil(() => NowPhase == 1 && Boss.Instance.Base.Hp <= 0);
        isThisSkillCoolDown[patternChoice] = false;

        if (Boss.Instance.actCoroutine != null)
            StopCoroutine(Boss.Instance.actCoroutine);

        Boss.Instance.actCoroutine = null;

        nowBPhaseChange = true;
        Boss.Instance.isBInvincible = true;
        CinemachineCameraShaking.Instance.CameraShake(6, 10f);

        Boss.Instance.bossAnim.anim.SetBool("FinalEnd", true);
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashPhase);

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

        SetPatternWeight();

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

            int nowPatternWeight = 0;
            int choisedWeight = Random.Range(0, 100);
            for (int i = 0; i < phase_patternCount[NowPhase - 1]; i++)
            {
                nowPatternWeight += patternWeight[i]; 
                if(choisedWeight <= nowPatternWeight)
                {
                    patternChoice = i;
                    break;
                }
            }
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
        yield return new WaitForSeconds(5f);
        isThisSkillCoolDown[nowSkill] = false;
    }

    public virtual int GetRandomCount(int choisedPattern)
    {
        return 0;
    }
    public virtual void SetPatternWeight()
    {
        for(int i = 0; i < phase_patternCount[NowPhase - 1]; i++)
        {
            patternWeight[i] = 100 / phase_patternCount[NowPhase - 1];
        }
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
