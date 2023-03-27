using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G_Patterns : BossPattern
{
    #region Initialize
    [SerializeField] protected GameObject warning;

    [SerializeField] protected ParticleSystem SummonFx;
    [SerializeField] protected GameObject SummonTimer;
    [SerializeField] protected Image SummonClock;

    [SerializeField] protected GhostBossJangpanPattern bossRangePattern;

    WaitForSeconds waitTime = new WaitForSeconds(1f);
    #endregion

    #region phase 1
    public IEnumerator Pattern_TH(int count) //가시
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
    public IEnumerator Pattern_BM(int count) //빔
    {
        yield return null;

        for(int i = 0; i < count; i++)
        {
            Vector2 dir = player.position - transform.position;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            switch (i)
            {
                case 0:
                    attackAnim.Play(animArray[1]);
                    Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position, Quaternion.Euler(Vector3.forward * rot));
                    break;
                case 1:
                    attackAnim.Play(animArray[1]);
                    for (int j = -1; j <= 1; j += 2)
                    {
                        Vector3 pos = Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? Vector3.up : Vector3.right;
                        dir = player.position - (transform.position + pos * j * 2);
                        rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                        Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position + (pos * j * 2), Quaternion.Euler(Vector3.forward * (rot + j * 30)));
                    }
                    break;
                case 2:
                    attackAnim.Play(animArray[1]);
                    for (int j = 0; j < 4; j++)
                    {
                        Poolable clone = Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position,Quaternion.Euler(Vector3.forward * (90 * j + 45)));

                        float y = j > 1 ? -1.5f : 1.5f;
                        float x = j >= 1 && j <= 2 ? -1.5f : 1.5f;

                        clone.transform.position = new Vector2(clone.transform.position.x + x, clone.transform.position.y + y);
                    }
                    break;
                case 3:
                    attackAnim.Play(animArray[1]);
                    for (int j = 0; j < 8; j++)
                    {
                        Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position, Quaternion.Euler(Vector3.forward * 45 * j));
                    }
                    break;
                case 4:
                    attackAnim.Play(animArray[1]);
                    int randomCount = Random.Range(8, 13);
                    for(int j = 0; j < randomCount; j++)
                    {
                        Managers.Pool.PoolManaging("10.Effects/ghost/Beam", new Vector2(-9 + j * 7.5f, 18.5f), Quaternion.Euler(Vector3.forward * 270));
                        yield return null;
                    }
                    break;
            }
            yield return new WaitForSeconds(2.5f);
        }
    }
    public IEnumerator Pattern_TP(int count) //텔포 -> 현재 바꾸는 작업중
    {
        Vector3 dir;

        yield return new WaitForSeconds(3f);

        transform.position = player.right * 3 + player.position;

        attackAnim.Play(animArray[2]);
        yield return new WaitForSeconds(0.35f);


        if (count > -4)
        {
            dir = player.position - (transform.position);
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle = 7.2f;


            for (int i = -4; i < count; i++)
            {
                Managers.Pool.PoolManaging("03.Prefabs/Test/Bullet_Guided", transform.position, Quaternion.Euler(Vector3.forward * (angle * i + rot)));
            }
        }
    }
    public IEnumerator Pattern_SM(int count) //힐라
    {
        int finalCount = 0;
        List<GameObject> mobList = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            Poolable clone = Managers.Pool.PoolManaging("03.Prefabs/Enemy/Ghost/G_Mob_02", new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity);
            GameObject pattern33 = Instantiate(SummonFx.gameObject, clone.transform.position, Quaternion.Euler(Vector3.zero));

            ParticleSystem particle = pattern33.GetComponent<ParticleSystem>();

            particle.Play();

            mobList.Add(clone.gameObject);
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
    Coroutine ActCoroutine = null;

    private void Update()
    {
        if (Boss.Instance.Base.Hp <= Boss.Instance.Base.MaxHp * 0.4f && NowPhase == 1)
        {
            isUsingFinalPattern = true;
        }

        if (Boss.Instance.isBInvincible && ActCoroutine != null) StartCoroutine(ECoroutine());

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
                return NowPhase == 1 ? 3 : 5;
            case 2:
                return NowPhase == 1 ? -4 : 4;
            case 3:
                break;
            case 4:
                break;
            case 5:
                return 10;
            default:
                break;
        }
        return 0;

    }

    private Coroutine SCoroutine(IEnumerator act)
    {
        return ActCoroutine = StartCoroutine(act);
    }
    private IEnumerator ECoroutine()
    {
        StopCoroutine(ActCoroutine);
        ActCoroutine = null;
        yield return null;
    }

    public override IEnumerator Pattern1(int count = 0) //장판 패턴
    {


        switch (NowPhase)
        {
            case 1:
                attackAnim.Play(animArray[0]);
                yield return StartCoroutine(bossRangePattern.FloorPatternCircle());
                break;
            case 2:
                StartCoroutine(bossRangePattern.FloorPatternRectangle());
                yield return new WaitForSeconds(5f);
                break;
        }

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator Pattern2(int count = 0) //빔 패턴
    {

        yield return SCoroutine(Pattern_BM(count));

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator Pattern3(int count = 0) //텔레포트 패턴
    {
        switch(NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_TP(count));
                break;
            case 2:
                yield return SCoroutine(Pattern_TP(count));
                break;
        }

        yield return null;
        attackCoroutine = null;
    }

    public override IEnumerator Pattern4(int count = 0) //팔뻗기 패턴, 2페이즈에만 사용
    {
        switch (NowPhase)
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
        switch(NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_SM(count));
                break;
            case 2:
                break;
        }

        yield return null;
        attackCoroutine = null;
    }
}
