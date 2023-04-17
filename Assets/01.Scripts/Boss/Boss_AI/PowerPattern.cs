using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Patterns : BossPattern
{
    [Space]
    [Header("파워")]
    #region Initialize
    [SerializeField] private float test;
    #endregion

    #region patterns
    public IEnumerator Pattern_SG(int count = 0) //바닥찍기 1페이즈
    {
        for(int i = 0; i < 3; i++)
        {
            //모션 추가
            yield return new WaitForSeconds(1f);
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 5f);
            foreach(Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                    GameManager.Instance.Player.OnDamage(2, 0);
            }

            for(int j = 0; j < count; j++)
            {
                float xDist = Random.Range(-3f, 3f);
                //아래 스트링에 오브젝트 넣어주기
                Managers.Pool.PoolManaging(" ", new Vector2(transform.position.x + xDist, transform.position.y + (3f - Mathf.Abs(xDist)) * Mathf.Sign(Random.Range(0,2))),Quaternion.identity);
            }

            yield return new WaitForSeconds(2f);
        }
        yield return null;
    }
    public IEnumerator Pattern_DS(int count = 0) //돌진
    {
        Vector2 dir = Boss.Instance.player.position - transform.position;
        transform.LookAt(dir);

        yield return new WaitForSeconds(2f);

        float timer = 0f;
        while(timer < 3f)
        {
            timer += Time.deltaTime;
        }

        yield return null;
    }
    #endregion
}
public class PowerPattern : P_Patterns
{
    Coroutine ActCoroutine = null;

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

    public override int GetRandomCount(int choisedPattern)
    {
        switch (choisedPattern)
        {
            case 0:
                return Random.Range(5, 8);
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

    public override IEnumerator Pattern1(int count = 0) //바닥 찍기
    {
        switch (NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_SG(count));
                break;
            case 2:
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern2(int count = 0) //돌진
    {
        switch (NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_DS());
                break;
            case 2:
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern3(int count = 0)
    {
        switch (NowPhase)
        {
            case 1:
                break;
            case 2:
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern4(int count = 0)
    {
        switch (NowPhase)
        {
            case 1:
                break;
            case 2:
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }
    public override IEnumerator Pattern5(int count = 0)
    {
        switch (NowPhase)
        {
            case 1:
                break;
            case 2:
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator PatternFinal(int count = 0)
    {
        switch (NowPhase)
        {
            case 1:
                break;
            case 2:
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }
}
