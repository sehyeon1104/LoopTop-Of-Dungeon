using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G_Patterns : BossPattern
{
    #region Initialize
    [SerializeField] protected GameObject warning;
    [SerializeField] protected GameObject bossMonster;

    [SerializeField] protected ParticleSystem SummonFx;
    [SerializeField] protected GameObject SummonTimer;
    [SerializeField] protected Image SummonClock;

    [SerializeField] protected BossRangePattern bossRangePattern;

    WaitForSeconds waitTime = new WaitForSeconds(1f);
    bool isUsedSummonSkill = false;
    #endregion

    #region pase 1
    public IEnumerator Pattern_TH(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //보스 애니메이션 
            attackAnim.Play(animArray[1]);

            GameObject clone = Instantiate(warning, player.position, Quaternion.identity);
            Managers.Sound.Play("SoundEffects/Ghost/G_Warning.wav");
            yield return waitTime;

            Managers.Pool.PoolManaging("10.Effects/ghost/Thorn", clone.transform.position, Quaternion.identity);
            CinemachineCameraShaking.Instance.CameraShake();
            Managers.Sound.Play("SoundEffects/Ghost/G_Thorn.wav");

            Destroy(clone);
        }
    } // 팔뻗기로 대체 예정
    public IEnumerator Pattern_BM()
    {
        yield return null;
        for(int i = 0; i < 4; i++)
            Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position, Quaternion.Euler(new Vector3(0, 0, i * 90)));
    }
    public IEnumerator Pattern_TP()
    {
        moveSpeed *= 0.5f;
        float timer = 0f;
        Vector3 dir;

        while (timer <= 3f)
        {
            timer += Time.deltaTime;
            yield return null;

            dir = player.position - transform.position;
            transform.Translate(dir.normalized * Time.deltaTime * moveSpeed);
        }

        transform.position = player.forward + player.position;
        moveSpeed *= 2f;

        attackAnim.Play(animArray[2]);
        yield return new WaitForSeconds(0.35f);

        dir = player.position - transform.position;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angle = 7.2f;


        for (int i = -4; i < 4; i++)
        {
            Managers.Pool.PoolManaging("03.Prefabs/Test/Bullet_Guided", transform.position + Vector3.up * 2, Quaternion.Euler(Vector3.forward * (angle * i + rot * 0.5f)));
        }
    }
    public IEnumerator Pattern_SM(int count)
    {
        int finalCount = 0;
        List<GameObject> mobList = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(bossMonster, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity);
            GameObject pattern33 = Instantiate(SummonFx.gameObject, clone.transform.position, Quaternion.Euler(Vector3.zero));

            ParticleSystem particle = pattern33.GetComponent<ParticleSystem>();

            particle.Play();

            mobList.Add(clone);
        }

        SummonTimer.SetActive(true);

        Boss.Instance.isBDamaged = true;
        for (int i = 1; i < 13; i++)
        {
            yield return new WaitForSeconds(2f);
            SummonClock.fillAmount = (float)i / 12;
        }
        Boss.Instance.isBDamaged = false;

        SummonClock.fillAmount = 0;
        SummonTimer.SetActive(false);

        foreach (var mob in mobList)
        {
            if (mob != null)
            {
                finalCount++;
                Destroy(mob);
            }

        }
        Boss.Instance.Base.Hp += finalCount * 10;
        mobList.Clear();
    }
    #endregion
}
public class GhostPattern : G_Patterns
{

    private void Awake()
    {
        isThisSkillCoolDown[3] = true;
    }
    private void Update()
    {
        if (Boss.Instance.Base.Hp <= Boss.Instance.Base.MaxHp * 0.4f)
        {

        }

        if (Boss.Instance.isBDead) SummonTimer.gameObject.SetActive(false);
        base.Update();
    }
    public override int GetRandomCount(int choisedPattern)
    {
        switch (choisedPattern)
        {
            case 0:
                return Random.Range(3, 6);
            case 1:
                return Random.Range(20, 30);
            case 2:
                break;
            case 3:
                return 10;
            case 4:
                break;
            default:
                break;
        }
        return 0;

    }

    public override IEnumerator Pattern1(int count = 0) //가시 소환 패턴 -> 장판 패턴으로 교체 예정
    {
        switch(NowPase)
        {
            case 1:
                yield return StartCoroutine(Pattern_TH(count));
                break;
            case 2:
                break;
        }

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator Pattern2(int count = 0) //빔 패턴
    {
        switch (NowPase)
        {
            case 1:
                yield return StartCoroutine(Pattern_BM());
                break;
            case 2:
                break;
        }

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator Pattern3(int count = 0) //텔레포트 패턴
    {
        switch(NowPase)
        {
            case 1:
                yield return StartCoroutine(Pattern_TP());
                break;
            case 2:
                break;
        }

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator Pattern4(int count = 0) //장판 패턴
    {
        switch (NowPase)
        {
            case 1:
                yield return StartCoroutine(bossRangePattern.FloorPatternCircle());
                break;
            case 2:
                yield return StartCoroutine(bossRangePattern.FloorPatternRectangle());
                break;
        }

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator Pattern5(int count = 0) //팔뻗기 패턴, 2페이즈에만 사용
    {
        switch (NowPase)
        {
            case 1:
                break;
            case 2:
                break;
        }

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator PatternFinal(int count = 0)
    {
        switch(NowPase)
        {
            case 1:
                yield return StartCoroutine(Pattern_SM(count));
                break;
            case 2:
                break;
        }

        yield return null;
        attackCoroutine = null;
    }
}
