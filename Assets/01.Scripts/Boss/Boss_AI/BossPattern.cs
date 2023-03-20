using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPattern : MonoBehaviour
{
    [Header("보스 이동 관련 스탯")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float minDistance;

    [Space]

    [Header("보스 옵션")]

    [Header("보스방 입장 시 초기 대기 상태")]
    [SerializeField]
    private float waitTime = 1f;
    [Header("페이즈별 보스 스킬 개수")]
    [SerializeField] private int[] pase_patternCount;
    [Header("보스 패턴 후딜레이")]
    [SerializeField] private float patternDelay;
    
    protected int[] patternCount = new int[5];
    protected int nowPase = 1;


    protected Transform player;
    
    protected Animation attackAnim;
    protected List<string> animArray;

    protected Coroutine attackCoroutine = null;

    protected bool[] isThisSkillCoolDown = new bool[5];

    private void Start()
    {
        player = GameManager.Instance.Player.transform;
        attackAnim = GetComponent<Animation>();

        AnimationArray();
        StartCoroutine(RandomPattern());
    }

    protected void Update()
    {
        MoveToPlayer();
        if(Boss.Instance.isBDead)
        {
            attackCoroutine = null;
            StopAllCoroutines();
        }
    }

    public void AnimationArray()
    {
        animArray = new List<string>();

        foreach (AnimationState states in attackAnim)
        {
            animArray.Add(states.name);
        }
    }

    public void MoveToPlayer()
    {
        if (attackCoroutine != null || Boss.Instance.isBDead) return;

        float playerDistance = Vector2.Distance(player.position, transform.position);
        if (playerDistance <= minDistance) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * moveSpeed);
    }

    private IEnumerator RandomPattern()
    {
        yield return new WaitForSeconds(waitTime);

        while (!Boss.Instance.isBDead)
        {
            int patternChoice = 0;
            patternChoice = Random.Range(0, pase_patternCount[nowPase]);
            patternCount[patternChoice] = GetRandomCount(patternChoice);

            if (isThisSkillCoolDown[patternChoice]) continue;


            if (attackCoroutine == null)
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

            yield return null;
            
            if (attackCoroutine != null)
            {
                yield return new WaitUntil(() => attackCoroutine == null);
                yield return new WaitForSeconds(patternDelay);
            }

            StartCoroutine(CoolDownCheck(patternChoice));
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
