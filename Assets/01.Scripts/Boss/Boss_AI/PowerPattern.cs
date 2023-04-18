using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Patterns : BossPattern
{
    [Space]
    [Header("파워")]
    #region Initialize
    [SerializeField] private GameObject dashWarning;
    #endregion

    #region patterns
    public IEnumerator Pattern_SG(int count = 0) //바닥찍기 1페이즈
    {
        for(int i = 0; i < 3; i++)
        {
            //모션 추가
            yield return new WaitForSeconds(1f);
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 3.5f);
            Managers.Pool.PoolManaging("10.Effects/power/ShockWave.prefab", transform.position, Quaternion.identity);

            foreach(Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                    GameManager.Instance.Player.OnDamage(2, 0);
            }

            for(int j = 0; j < count; j++)
            {
                float xDist = Random.Range(-5f, 5f);
                //아래 스트링에 오브젝트 넣어주기
                Managers.Pool.PoolManaging("10.Effects/power/ShockWave.prefab", new Vector2(transform.position.x + xDist, transform.position.y + (5f - Mathf.Abs(xDist)) * Mathf.Sign(Random.Range(0,2))),Quaternion.identity);
            }

            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }
    public IEnumerator Pattern_DS(int count = 0) //돌진 1페이즈
    {
        float timer = 0f;
        Vector2 dir = Boss.Instance.player.position - transform.position;
        float rot = 0;

        dashWarning.SetActive(true);

        while (timer < 1f)
        {
            timer += Time.deltaTime;
            dir = Boss.Instance.player.position - transform.position;
            rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            dashWarning.transform.rotation = Quaternion.Euler(Vector3.forward * (rot - 90 + Mathf.Sign(transform.lossyScale.x) * 90));

            yield return null;
        }

        timer = 0;
        yield return new WaitForSeconds(0.5f);

        dashWarning.SetActive(false);
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            transform.Translate(dir.normalized * Time.deltaTime * 12f);

            yield return null;
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
                yield return SCoroutine(Pattern_DS());
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
                yield return SCoroutine(Pattern_DS());
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
                yield return SCoroutine(Pattern_DS());
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
