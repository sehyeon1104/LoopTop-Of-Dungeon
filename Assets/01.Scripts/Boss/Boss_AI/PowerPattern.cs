using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_Patterns : BossPattern
{
    [Space]
    [Header("파워")]
    #region Initialize
    [SerializeField] protected GameObject shorkWarning;
    [SerializeField] protected GameObject dashWarning;
    [SerializeField] protected Transform dashBar;
    #endregion

    #region patterns
    public IEnumerator Pattern_SG(int count = 0) //바닥찍기 1페이즈
    {
        for(int i = 0; i < 3; i++)
        {

            //모션 추가
            shorkWarning.SetActive(true);

            yield return new WaitForSeconds(0.5f);
            shorkWarning.SetActive(false);

            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
            Collider2D[] cols = Physics2D.OverlapCircleAll(shorkWarning.transform.position, 6f);
            Managers.Pool.PoolManaging("10.Effects/power/ShockWave.prefab", shorkWarning.transform.position, Quaternion.identity);
            CinemachineCameraShaking.Instance.CameraShake(6, 0.3f);

            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                    GameManager.Instance.Player.OnDamage(2, 0);
            }

            for(int j = 0; j < count; j++)
            {
                float randDist = Random.Range(0, 360f) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(randDist), Mathf.Sin(randDist)).normalized * 7.5f;
                //얘네 나오기 전에 경고표시 나오게 수정하기
                Managers.Pool.PoolManaging("Assets/10.Effects/power/RockFall.prefab", new Vector2(transform.position.x + dir.x, transform.position.y + 2 + dir.y),Quaternion.identity);
            }

            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
    public IEnumerator Pattern_DS(int count = 0) //돌진 1페이즈
    {
        float timer = 0f;
        Vector3 dir = Boss.Instance.player.position - transform.position;
        float rot = 0;

        //대기 모션 추가

        dashWarning.SetActive(true);

        dashBar.localScale = new Vector3(1, 0, 1);
        dashBar.DOScaleY(1, 1.5f);

        while (timer < 1f)
        {
            timer += Time.deltaTime;
            dir = Boss.Instance.player.position - dashWarning.transform.position;
            rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Boss.Instance.bossMove.CheckFlipValue(dir, transform.localScale);

            dashWarning.transform.rotation = Quaternion.Euler(Vector3.forward * (rot - 90 - Mathf.Sign(transform.lossyScale.x) * 90));

            yield return null;
        }

        timer = 0;
        dir = Boss.Instance.player.position - dashWarning.transform.position;

        yield return new WaitForSeconds(0.4f);

        dashBar.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        dashBar.GetComponentInChildren<SpriteRenderer>().color = new Color32(200,0,0,128);

        //대시 모션 추가

        dashWarning.SetActive(false);
        CinemachineCameraShaking.Instance.CameraShake(2, 0.5f);
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            transform.Translate(dir.normalized * Time.deltaTime * 40f);

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + Vector3.up * 3.5f + dir.normalized, 4f);
            for(int i = 0; i < cols.Length; i++)
            {
                if (cols[i].CompareTag("Player"))
                {
                    GameManager.Instance.Player.OnDamage(2, 0);
                    break;
                }
            }
            //충돌을 어떤 방식으로 작동하게할까?

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
