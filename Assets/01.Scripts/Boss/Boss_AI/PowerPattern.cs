using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class P_Patterns : BossPattern
{
    [Space]
    [Header("파워")]
    #region Initialize
    [SerializeField] protected AnimationClip[] groundHit;

    [SerializeField] protected VisualEffect shorkWarning;
    [SerializeField] protected GameObject dashWarning;
    [SerializeField] protected GameObject dash2Phase;

    [SerializeField] protected CinemachineVirtualCamera dashVCam;
    [SerializeField] protected GameObject standUpVCam;

    private List<Transform> partList = new List<Transform>();
    private List<StandupObject> standupObjects = new List<StandupObject>();
    private Camera mainCam;
    #endregion

    protected Vector2 dirToPlayerOld = Vector2.zero;

    private void Awake()
    {
        mainCam = Camera.main;
        for (int i = 1; i <= 6; i++)
        {
            partList.Add(dash2Phase.transform.Find($"Part{i}"));
        }
        foreach (var std in FindObjectsOfType<StandupObject>())
            standupObjects.Add(std);
    }
    public void StandUp(bool isStdUp = true)
    {
        mainCam.orthographic = !isStdUp;
        standUpVCam.SetActive(isStdUp);
        foreach (var std in standupObjects)
        {
            std.isStandUp = isStdUp;
        }
    }

    #region Phase 1
    public IEnumerator Pattern_SHOTGUN(int count = 0) //바닥찍기 1페이즈
    {
        for(int i = 0; i < 3; i++)
        {
            shorkWarning.SetFloat("LifeTime", i == 2 ? 1.2f : 0.5f);
            shorkWarning.gameObject.SetActive(true);

            yield return new WaitForSeconds(i == 2? 1f : 0.5f);

            Boss.Instance.bossAnim.overrideController[$"Skill1"] = groundHit[i];
            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

            yield return new WaitForSeconds(i == 2? 0.5f : 0.2f);

            shorkWarning.gameObject.SetActive(false);

            Collider2D col = Physics2D.OverlapCircle(shorkWarning.transform.position, 8f, 1<<8 | 1<<15);
            Managers.Pool.PoolManaging("Assets/10.Effects/power/GroundCrack.prefab", shorkWarning.transform.position, Quaternion.identity);
            CinemachineCameraShaking.Instance.CameraShake(6, 0.2f);

            if(col != null)
                col.GetComponent<IHittable>().OnDamage(20, 0);

            if (i == 2)
            {
                for (int j = 0; j < count; j++)
                {
                    float randDist = Random.Range(0, 360f) * Mathf.Deg2Rad;
                    Vector2 dir = new Vector2(Mathf.Cos(randDist), Mathf.Sin(randDist)).normalized * 9.5f;
                    Managers.Pool.PoolManaging("Assets/10.Effects/power/RockFall.prefab", new Vector2(transform.position.x + dir.x, transform.position.y + 2 + dir.y), Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
    public IEnumerator Pattern_DASHATTACK(int count = 0) //돌진 1페이즈
    {
        standupObjects.Clear();
        foreach (var std in FindObjectsOfType<StandupObject>())
            standupObjects.Add(std);

        StandUp();

        float timer = 0f;
        Vector3 dir = Boss.Instance.player.position - transform.position;
        float rot = 0;

        dashWarning.SetActive(true);

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

        yield return new WaitForSeconds(0.5f);

        //대시 모션 추가
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

        dashWarning.SetActive(false);
        CinemachineCameraShaking.Instance.CameraShake(2, 1.5f);
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;

            if (Mathf.Sign(dir.x) * transform.position.x < Mathf.Sign(dir.x) * 14.25f + 16.25f 
               && Mathf.Sign(dir.y) * transform.position.y < Mathf.Sign(dir.y) * 5.5f + 11.5f)
            transform.Translate(dir.normalized * Time.deltaTime * 30f);

            Collider2D col = Physics2D.OverlapCircle(transform.position + Vector3.up * 3.5f + dir.normalized, 3f, 1 << 8 | 1 << 15);
            if(col != null)
                col.GetComponent<IHittable>().OnDamage(20, 0);

            yield return null;
        }

        StandUp(false);
        yield return null;
    }
    public IEnumerator Pattern_JUMPATTACK(int count = 0) //점프어택
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 playerPos = Boss.Instance.player.position;

            Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/power/WarningFX.prefab", playerPos, Quaternion.identity);
            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

            if (i == 2) clone.transform.localScale = new Vector3(1.75f, 1.75f);
            else clone.transform.localScale = new Vector3(1.25f, 1.25f);

            transform.DOMove(transform.position + Vector3.up * 600, 0.2f);
            CinemachineCameraShaking.Instance.CameraShake(6, 0.2f);
            yield return new WaitForSeconds(0.2f);

            transform.DOMove(clone.transform.position, 1f);
            yield return new WaitForSeconds(1f);

            Collider2D col = null;
            if(i == 2)
            {
                col = Physics2D.OverlapCircle(clone.transform.position, 7.5f, 1 << 8 | 1 << 15);
                CinemachineCameraShaking.Instance.CameraShake(12, 0.4f);
            }
            else
            {
                col = Physics2D.OverlapCircle(clone.transform.position, 5.5f, 1 << 8 | 1 << 15);
                CinemachineCameraShaking.Instance.CameraShake(10, 0.2f);
            }
            Managers.Pool.PoolManaging("Assets/10.Effects/power/JumpShock.prefab", clone.transform.position, Quaternion.identity);

            if (col != null)
                col.GetComponent<IHittable>().OnDamage(25, 0);

            Managers.Pool.Push(clone);

            yield return new WaitForSeconds(0.5f);
        }
            yield return null;
    }
    public IEnumerator Pattern_COLUMN(int count = 0) //기둥
    {
        while(!Boss.Instance.isBDead)
        {
            if (nowBPhaseChange)
            {
                yield return null;
                continue;
            }

            Vector2 randPos = new Vector2(Random.Range(0f, 28.5f), Random.Range(-2f, 15.5f));

            yield return new WaitForSeconds(9f / NowPhase);

            Managers.Pool.PoolManaging("Assets/10.Effects/power/Column.prefab", randPos, Quaternion.identity);
        }
    }
    public IEnumerator Pattern_THROW(int count = 0) //돌뿌리기
    {
        float angleRange = 25f;
        Vector3 dirToPlayer = (Boss.Instance.player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        for (int i = -7; i < 8; i++)
        {
            Managers.Pool.PoolManaging("Assets/10.Effects/power/RockWarning.prefab", transform.position + dirToPlayer, Quaternion.AngleAxis(angle + angleRange * i, Vector3.forward));
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.75f);
        CinemachineCameraShaking.Instance.CameraShake(8, 0.2f);
        //Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        for (int i = -7; i < 8; i++)
        {
            Managers.Pool.PoolManaging("Assets/10.Effects/power/Rock.prefab", transform.position + dirToPlayer, Quaternion.AngleAxis(angle + angleRange * i, Vector3.forward));
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.5f);
    }
    #endregion

    #region Phase 2
    public IEnumerator Pattern_SG_2(int count = 0) //바닥찍기 2페이즈
    {
        List<GameObject> bodyList = new List<GameObject>();
        int bodyCount = 0;

        for (int i = 0; i < 3; i++)
        {
            shorkWarning.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.2f);            
            shorkWarning.gameObject.SetActive(false);

            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

            Collider2D col = Physics2D.OverlapCircle(shorkWarning.transform.position, 8f , 1 << 8 | 1 << 15);
            Managers.Pool.PoolManaging("Assets/10.Effects/power/GroundCrack.prefab", shorkWarning.transform.position, Quaternion.identity);
            CinemachineCameraShaking.Instance.CameraShake(6, 0.2f);

            if(col != null)
                col.GetComponent<IHittable>().OnDamage(20, 0);

            for (int j = 0; j < count; j++)
            { 
                float randDist = Random.Range(0, 360f) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(randDist), Mathf.Sin(randDist)).normalized * 9.5f;
                if (bodyCount < 3)
                {
                    bodyCount++;
                    GameObject clone = Managers.Pool.PoolManaging("Assets/10.Effects/power/RockFall.prefab", new Vector2(transform.position.x + dir.x, transform.position.y + 2 + dir.y), Quaternion.identity).gameObject;
                    bodyList.Add(clone);
                }
                else
                {
                    Managers.Pool.PoolManaging("Assets/10.Effects/power/RockFall.prefab", new Vector2(transform.position.x + dir.x, transform.position.y + 2 + dir.y), Quaternion.identity);
                }
            }
            bodyCount = 0;
        }
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < bodyList.Count; i++)
        {
            bodyList[i].transform.DOMove(transform.position, 0.5f);
        }

        yield return new WaitForSeconds(0.6f);
    }
    public IEnumerator Pattern_DS_2(int count = 0) //돌진 2페이즈
    {
        transform.DOMove(transform.position + Vector3.up * 600, 0.2f);
        dashVCam.Priority = 11;

        int randomInvisible = Random.Range(0, 6);
        partList[randomInvisible].gameObject.SetActive(false);
        dash2Phase.SetActive(true);

        yield return new WaitForSeconds(2f);
        CinemachineCameraShaking.Instance.CameraShake(8, 0.2f);
        yield return new WaitForSeconds(1f);

        partList[randomInvisible].gameObject.SetActive(true);
        dash2Phase.SetActive(false);
        dashVCam.Priority = 0;

        Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/power/WarningFX.prefab", GameManager.Instance.Player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);

        transform.DOMove(clone.transform.position, 1f);
        yield return new WaitForSeconds(1f);

        Collider2D col = Physics2D.OverlapCircle(clone.transform.position, 5.5f, 1 << 8 | 1 << 15);
        CinemachineCameraShaking.Instance.CameraShake(10, 0.2f);

        Managers.Pool.PoolManaging("Assets/10.Effects/power/JumpShock.prefab", clone.transform.position, Quaternion.identity);

        if (col != null)
            col.GetComponent<IHittable>().OnDamage(25, 0);
        yield return null;
    }
    public IEnumerator Pattern_JA_2(int count = 0) //점프어택 2페이즈
    {
        StandUp();
        yield return null;
    }
    public IEnumerator Pattern_TH_2(int count = 0) //돌뿌리기 2페이즈
    {
        for (int i = 0; i < 5; i++)
        {
            float angleRange = Random.Range(15f, 25f);
            Vector3 dirToPlayer = (Boss.Instance.player.position - transform.position).normalized;
            float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

            for (int j = -2; j < 3; j++)
            {
                Managers.Pool.PoolManaging("Assets/10.Effects/power/RockWarning.prefab", transform.position + dirToPlayer, Quaternion.AngleAxis(angle + angleRange * j, Vector3.forward));
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.5f);
            CinemachineCameraShaking.Instance.CameraShake(8, 0.2f);

            for (int j = -2; j < 3; j++)
            {
                Managers.Pool.PoolManaging("Assets/10.Effects/power/Rock.prefab", transform.position + dirToPlayer, Quaternion.AngleAxis(angle + angleRange * j, Vector3.forward));
                yield return new WaitForSeconds(0.05f);
            }
        }
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


    private void Update()
    {
        if (nowBPhaseChange && ActCoroutine != null)
        {
            dashWarning.SetActive(false);
            shorkWarning.gameObject.SetActive(false);

            StartCoroutine(ECoroutine());
        }

        if(Boss.Instance.isBDead)
        {
            dashVCam.Priority = 0;
        }

        base.Update();
    }

    public override int GetRandomCount(int choisedPattern)
    {
        switch (choisedPattern)
        {
            case 0:
                return Random.Range(6, 9);
            case 1:
                break;
            case 2:
                break;
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

    private void Start()
    {
        StartCoroutine(Pattern_COLUMN());
    }

    public override IEnumerator Pattern1(int count = 0) //바닥 찍기
    {
        switch (NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_SHOTGUN(count));
                break;
            case 2:
                yield return SCoroutine(Pattern_SG_2(count));
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
                yield return SCoroutine(Pattern_DASHATTACK());
                break;
            case 2:
                yield return SCoroutine(Pattern_DS_2());
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern3(int count = 0) //볼리베어
    {
        yield return SCoroutine(Pattern_JUMPATTACK(count));

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern4(int count = 0) //돌뿌리기
    {
        switch (NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_THROW());
                break;
            case 2:
                yield return SCoroutine(Pattern_TH_2());
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
