using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEditor;

public class P_Patterns : BossPattern
{
    [Space]
    [Header("�Ŀ�")]
    #region Initialize
    [SerializeField] protected AnimationClip[] groundHit;

    [SerializeField] protected GameObject shorkWarning;
    [SerializeField] protected GameObject dashWarning;
    [SerializeField] protected GameObject dash2Phase;
    [SerializeField] protected Transform dashBar;

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

    #region Phase 1
    public IEnumerator Pattern_SHOTGUN(int count = 0) //�ٴ���� 1������
    {
        for(int i = 0; i < 3; i++)
        {
            //��� �߰�
            shorkWarning.SetActive(true);

            yield return new WaitForSeconds(1f);

            Boss.Instance.bossAnim.overrideController[$"Skill1"] = groundHit[i];
            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

            yield return new WaitForSeconds(0.2f);

            shorkWarning.SetActive(false);

            Collider2D col = Physics2D.OverlapCircle(shorkWarning.transform.position, 8f, 1<<8);
            Managers.Pool.PoolManaging("Assets/10.Effects/power/GroundCrack.prefab", shorkWarning.transform.position, Quaternion.identity);
            CinemachineCameraShaking.Instance.CameraShake(6, 0.2f);

            if(col != null)
                GameManager.Instance.Player.OnDamage(20, 0);

            for(int j = 0; j < count; j++)
            {
                float randDist = Random.Range(0, 360f) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(randDist), Mathf.Sin(randDist)).normalized * 9.5f;
                Managers.Pool.PoolManaging("Assets/10.Effects/power/RockFall.prefab", new Vector2(transform.position.x + dir.x, transform.position.y + 2 + dir.y), Quaternion.identity) ;
            }

            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
    public IEnumerator Pattern_DASHATTACK(int count = 0) //���� 1������
    {
        standupObjects.Clear();
        foreach (var std in FindObjectsOfType<StandupObject>())
            standupObjects.Add(std);

        mainCam.orthographic = false;
        standUpVCam.SetActive(true);
        foreach(var std in standupObjects)
        {
            std.isStandUp = true;
        }

        float timer = 0f;
        Vector3 dir = Boss.Instance.player.position - transform.position;
        float rot = 0;

        //��� ��� �߰�

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

        //��� ��� �߰�
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

        dashWarning.SetActive(false);
        CinemachineCameraShaking.Instance.CameraShake(2, 0.5f);
        while (timer < 1f)
        {
            timer += Time.deltaTime;

            if(Mathf.Sign(dir.x) * transform.position.x < Mathf.Sign(dir.x) * 14.25f + 14.25f 
                    && Mathf.Sign(dir.y) * transform.position.y < Mathf.Sign(dir.y) * 8.75f + 6.75f)
            transform.Translate(dir.normalized * Time.deltaTime * 30f);

            Collider2D col = Physics2D.OverlapCircle(transform.position + Vector3.up * 3.5f + dir.normalized, 3f, 1 << 8);
            if(col != null)
                GameManager.Instance.Player.OnDamage(20, 0);

            yield return null;
        }
        foreach (var std in standupObjects)
        {
            std.isStandUp = false;
        }
        standUpVCam.SetActive(false);
        mainCam.orthographic = true;
        yield return null;
    }
    public IEnumerator Pattern_JUMPATTACK(int count = 0) //��������
    {
        for (int i = 0; i < 3; i++)
        {
            Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/power/WarningFX.prefab", Boss.Instance.player.position, Quaternion.identity);
            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

            if (i == 2) clone.transform.localScale = new Vector3(1.5f, 1.5f);
            else clone.transform.localScale = Vector3.one;

            yield return new WaitForSeconds(0.5f);

            while (Vector2.Distance(clone.transform.position, transform.position) >= 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, clone.transform.position, Time.deltaTime * 5f);
                yield return null;
            }

            Collider2D col = null;
            if(i == 2)
            {
                col = Physics2D.OverlapCircle(clone.transform.position, 7f, 1 << 8);
                CinemachineCameraShaking.Instance.CameraShake(12, 0.2f);
            }
            else
            {
                col = Physics2D.OverlapCircle(clone.transform.position, 4.5f, 1 << 8);
                CinemachineCameraShaking.Instance.CameraShake(10, 0.1f);
            }

            for (int j = 0; j < count; j++)
            {
                float randDist = Random.Range(0, 360f) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(randDist), Mathf.Sin(randDist)).normalized * 5f;
                Managers.Pool.PoolManaging("Assets/10.Effects/power/RockFall.prefab", new Vector2(transform.position.x + dir.x, transform.position.y + 2 + dir.y), Quaternion.identity);
            }

            if (col != null)
                GameManager.Instance.Player.OnDamage(25, 0);

            Managers.Pool.Push(clone);

            yield return new WaitForSeconds(0.5f);
        }
            yield return null;
    }
    public IEnumerator Pattern_COLUMN(int count = 0) //���
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
    public IEnumerator Pattern_THROW(int count = 0) //���Ѹ���
    {
        float angleRange = 25f;
        Vector3 dirToPlayer = (Boss.Instance.player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        for (int i = -2; i < 3; i++)
        {
            Managers.Pool.PoolManaging("Assets/10.Effects/power/RockWarning.prefab", transform.position + dirToPlayer, Quaternion.AngleAxis(angle + angleRange * i, Vector3.forward));
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        for (int i = -2; i < 3; i++)
        {
            Managers.Pool.PoolManaging("Assets/10.Effects/power/Rock.prefab", transform.position + dirToPlayer, Quaternion.AngleAxis(angle + angleRange * i, Vector3.forward));
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
    }
    #endregion

    #region Phase 2
    public IEnumerator Pattern_SG_2(int count = 0) //�ٴ���� 2������
    {
        List<GameObject> bodyList = new List<GameObject>();
        int bodyCount = 0;

        for (int i = 0; i < 3; i++)
        {
            shorkWarning.SetActive(true);
            yield return new WaitForSeconds(1.2f);            
            shorkWarning.SetActive(false);

            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

            Collider2D[] cols = Physics2D.OverlapCircleAll(shorkWarning.transform.position, 8f);
            Managers.Pool.PoolManaging("Assets/10.Effects/power/GroundCrack.prefab", shorkWarning.transform.position, Quaternion.identity);
            CinemachineCameraShaking.Instance.CameraShake(6, 0.2f);

            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                    GameManager.Instance.Player.OnDamage(2, 0);
            }

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
    public IEnumerator Pattern_DS_2(int count = 0) //���� 2������
    {
        dashVCam.Priority = 11;

        int randomInvisible = Random.Range(0, 6);
        partList[randomInvisible].gameObject.SetActive(false);
        dash2Phase.SetActive(true);
        yield return new WaitForSeconds(3f);

        partList[randomInvisible].gameObject.SetActive(true);
        dash2Phase.SetActive(false);

        dashVCam.Priority = 0;
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


    private void Update()
    {
        if (nowBPhaseChange && ActCoroutine != null)
        {
            dashWarning.SetActive(false);
            shorkWarning.SetActive(false);

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
                return NowPhase == 1 ? 0 : 4;
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

    public override IEnumerator Pattern1(int count = 0) //�ٴ� ���
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

    public override IEnumerator Pattern2(int count = 0) //����
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

    public override IEnumerator Pattern3(int count = 0) //��������
    {
        yield return SCoroutine(Pattern_JUMPATTACK(count));

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern4(int count = 0) //���Ѹ���
    {
        switch (NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_THROW());
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, dirToPlayerOld, 30, 5);
        Handles.DrawSolidArc(transform.position, Vector3.up, dirToPlayerOld, -30, 5);
    }
#endif
}
