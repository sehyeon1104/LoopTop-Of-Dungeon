using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class G_Patterns : BossPattern
{
    #region Initialize

    [SerializeField] protected GameObject SummonTimer;
    [SerializeField] protected Image SummonClock;

    [SerializeField] protected GhostBossJangpanPattern bossRangePattern;
    [SerializeField] protected GameObject bossObject;

    WaitForSeconds waitTime = new WaitForSeconds(1f);
    #endregion
    #region phase 1
    public IEnumerator Pattern_BM(int count) //빔
    {
        yield return null;

        for (int i = 0; i < count; i++)
        {
            Vector2 dir = player.position - transform.position;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            anim.SetTrigger(_hashAttack);

            switch (i)
            {
                case 0:
                    Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position, Quaternion.Euler(Vector3.forward * rot));
                    break;
                case 1:
                    for (int j = -1; j <= 1; j += 2)
                    {
                        Vector3 pos = Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? Vector3.up : Vector3.right;
                        dir = player.position - (transform.position + pos * j * 2);
                        rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                        Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position + (pos * j * 2), Quaternion.Euler(Vector3.forward * (rot + j * 30)));
                    }
                    break;
                case 2:
                    for (int j = 0; j < 4; j++)
                    {
                        Poolable clone = Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position,Quaternion.Euler(Vector3.forward * (90 * j + 45)));

                        float y = j > 1 ? -1.5f : 1.5f;
                        float x = j >= 1 && j <= 2 ? -1.5f : 1.5f;

                        clone.transform.position = new Vector2(clone.transform.position.x + x, clone.transform.position.y + y);
                    }
                    break;
                case 3:
                    for (int j = 0; j < 8; j++)
                    {
                        Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position, Quaternion.Euler(Vector3.forward * 45 * j));
                    }
                    break;
                case 4:
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
        Vector2 dir;
        bossObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        bossObject.SetActive(true);

        dir = player.position - transform.position;
        Vector3 scale = transform.localScale;
        scale = CheckFlipValue(dir, scale);

        bool playerDir = player.GetComponentInChildren<SpriteRenderer>().flipX;
        transform.position = (playerDir? -player.right : player.right) * 3f + player.position;

        anim.SetTrigger(_hashAttack);
        yield return new WaitForSeconds(0.5f);



        Debug.Log(scale.x);
        Poolable clone = Managers.Pool.PoolManaging("10.Effects/ghost/Claw",transform.position + transform.right * scale.x * 2.5f, Quaternion.Euler(new Vector3(0,0,237.5f)));

        Vector3 cloneScale = new Vector3(2.8f, scale.x * 1.75f, 1f);
        clone.transform.localScale = cloneScale;
        clone.GetComponent<VisualEffect>().Play();

        yield return new WaitForSeconds(0.5f);

        if (count > -4)
        {
            dir = player.position - transform.position;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle = 7.2f;

            for (int i = -4; i < count; i++)
            {
                Managers.Pool.PoolManaging("03.Prefabs/Test/Bullet_Guided", transform.position, Quaternion.Euler(Vector3.forward * (angle * i + rot * 0.5f)));
            }
        }
    }
    public IEnumerator Pattern_SM(int count) //힐라
    {
        int finalCount = 0;
        List<Poolable> mobList = new List<Poolable>();
        WaitForSeconds waitTime = new WaitForSeconds(2f);

        for (int i = 0; i < count; i++)
        {
            Poolable clone = Managers.Pool.PoolManaging("03.Prefabs/Enemy/Ghost/G_Mob_02", new Vector2(Random.Range(-2.5f, 29.5f), Random.Range(-3, 17.5f)), Quaternion.identity);
            mobList.Add(clone);
        }

        SummonTimer.SetActive(true);

        Boss.Instance.isBDamaged = true;

        for (int i = 1; i < 13; i++)
        {
            yield return waitTime;
            SummonClock.fillAmount = (float)i / 12;
        }

        Boss.Instance.isBDamaged = false;

        SummonClock.fillAmount = 0;
        SummonTimer.SetActive(false);

        foreach (Poolable mob in mobList)
        {
            if (mob.isActiveAndEnabled)
            {
                finalCount++;
                Managers.Pool.PoolManaging("10.Effects/ghost/Soul", mob.transform.position, Quaternion.identity);
                Managers.Pool.Push(mob);
            }

        }
        int hpFinal = Boss.Instance.Base.Hp + finalCount * 10;
        Boss.Instance.Base.Hp = hpFinal;
        mobList.Clear();

        yield return new WaitForSeconds(0.75f);
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
                anim.SetTrigger(_hashAttack);
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
