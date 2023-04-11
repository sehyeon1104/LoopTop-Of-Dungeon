using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class G_Patterns : BossPattern
{
    [Space]
    [Header("°í½ºÆ®")]
    #region Initialize

    [SerializeField] protected GameObject SummonTimer;
    [SerializeField] protected Image SummonClock;


    [SerializeField] protected GhostBossFieldPattern bossFieldPattern;
    [SerializeField] protected GameObject bossObject;
    [SerializeField] protected GameObject bossAura;

    [Space]
    [SerializeField] protected AnimationClip absorbEnd;

    protected GhostBossJangpanPattern bossRangePattern;


    protected List<Poolable> mobList = new List<Poolable>();
    protected WaitForSeconds waitTime = new WaitForSeconds(1f);
    #endregion

    private void Awake()
    {
        bossRangePattern = GetComponent<GhostBossJangpanPattern>();
    }

    #region patterns
    public IEnumerator Pattern_BM(int count) //ºö
    {
        WaitForSeconds wait = NowPhase == 1 ? new WaitForSeconds(1.75f) : new WaitForSeconds(1.25f);

        yield return null;

        for (int i = 0; i < count; i++)
        {
            Vector2 dir = Boss.Instance.player.position - transform.position;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Ghost/G_Warning.wav");

            switch (i)
            {
                case 0:
                    Managers.Pool.PoolManaging("10.Effects/ghost/Beam", transform.position, Quaternion.Euler(Vector3.forward * rot));
                    break;
                case 1:
                    for (int j = -1; j <= 1; j += 2)
                    {
                        Vector3 pos = Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? Vector3.up : Vector3.right;
                        dir = Boss.Instance.player.position - (transform.position + pos * j * 2);
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
                    int randomCount = Random.Range(10, 16);
                    Vector2 randDir = Vector2.zero;
                    Vector3 randRot = Vector3.zero;

                    for (int j = 0; j < randomCount; j++)
                    {
                        int rand = Random.Range(0, 2);
                        switch (rand)
                        {
                            case 0:
                                randDir = new Vector2(-4.5f,Random.Range(-2.5f,18.5f));
                                randRot = Vector3.forward * Random.Range(-30f, 30f);
                                break;
                            case 1:
                                randDir = new Vector2(Random.Range(-4.5f,33.5f),18.5f);
                                randRot = Vector3.forward * (270 + Random.Range(-30f, 30f));
                                break;
                            }
                        Managers.Pool.PoolManaging("10.Effects/ghost/Beam", randDir, Quaternion.Euler(randRot));
                        yield return null;
                    }
                    break;
            }
            yield return wait;
        }
    }
    public IEnumerator Pattern_TP(int count) //ÅÚÆ÷
    {
        Vector2 dir;

        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Teleport.prefab", transform.position + Vector3.down, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        bossObject.SetActive(false);
        bossAura.SetActive(false);
        Boss.Instance.isBInvincible = true;
        Managers.Pool.PoolManaging("10.Effects/ghost/Hide",transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);

        Boss.Instance.isBInvincible = false;
        bossObject.SetActive(true);
        bossAura.SetActive(true);

        dir = Boss.Instance.player.position - transform.position;
        Vector3 scale = transform.localScale;
        scale = Boss.Instance.bossMove.CheckFlipValue(dir, scale);

        transform.position = Vector3.left * scale.x * 3f + Boss.Instance.player.position;

        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        yield return waitTime;

        if (count > -4)
        {
            dir = Boss.Instance.player.position - transform.position;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle = 7.2f;

            for (int i = -4; i < count; i++)
            {
                Managers.Pool.PoolManaging("03.Prefabs/Test/Bullet_Guided", transform.position, Quaternion.Euler(Vector3.forward * (angle * i + rot * 0.5f)));
            }
        }

        yield return waitTime;
    }
    public IEnumerator Pattern_SM(int count) //Èú¶ó
    {
        int finalCount = 0;
        WaitForSeconds waitTime = new WaitForSeconds(2f);

        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

        Managers.Pool.PoolManaging("10.Effects/ghost/Absorb", transform.position + Vector3.down * 2f, Quaternion.identity);
        for (int i = 0; i < count; i++)
        {
            Poolable clone = Managers.Pool.PoolManaging("03.Prefabs/Enemy/Ghost/G_Mob_01", new Vector2(Random.Range(-2.5f, 29.5f), Random.Range(-3, 17.5f)), Quaternion.identity);
            mobList.Add(clone);
        }

        SummonTimer.SetActive(true);

        Boss.Instance.isBInvincible = true;

        for (int i = 1; i < 13; i++)
        {
            yield return waitTime;
            SummonClock.fillAmount = (float)i / 12;
        }

        Boss.Instance.isBInvincible = false;

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
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Heal.prefab", transform.position, Quaternion.identity);


        CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);
        Boss.Instance.bossAnim.overrideController[$"SkillFinal"] = absorbEnd;
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

        int hpFinal = Boss.Instance.Base.Hp + finalCount * 10;
        while (Boss.Instance.Base.Hp < hpFinal && Boss.Instance.Base.Hp < Boss.Instance.Base.MaxHp)
        {
            Boss.Instance.Base.Hp += 1;
            yield return null;
        }
        mobList.Clear();

        yield return new WaitForSeconds(2f);
    }
    public IEnumerator Pattern_GA(int count) //ÆÈ»¸±â
    {
        bossObject.SetActive(false);
        bossAura.SetActive(false);
        Boss.Instance.isBDamaged = true;
        Managers.Pool.PoolManaging("10.Effects/ghost/Hide", transform.position, Quaternion.identity);

        yield return StartCoroutine(bossFieldPattern.GhostBossArmPattern());

        bossObject.SetActive(true);
        bossAura.SetActive(true);
        Boss.Instance.isBDamaged = false;
    }
    #endregion
}

public class GhostPattern : G_Patterns
{
    Coroutine ActCoroutine = null;

    private void Update()
    {

        if (Boss.Instance.Base.Hp <= Boss.Instance.Base.MaxHp * 0.4f)
        {
            isUsingFinalPattern = true;
        }

        if (nowBPhaseChange && ActCoroutine != null) StartCoroutine(ECoroutine());

        if (Boss.Instance.isBDead || NowPhase == 2)
        {
            if(mobList.Count > 0)
                for(int i = 0; i < mobList.Count; i++)
                    Managers.Pool.Push(mobList[i]);

            if (nowBPhaseChange)
            {
                bossObject.SetActive(true);
                bossAura.SetActive(true);
                SummonTimer.gameObject.SetActive(false);
            }
        }

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

    public override IEnumerator Pattern1(int count = 0) //ÀåÆÇ ÆÐÅÏ
    {
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

        switch (NowPhase)
        {
            case 1:
                yield return StartCoroutine(bossRangePattern.FloorPatternCircle());
                break;
            case 2:
                StartCoroutine(bossRangePattern.FloorPatternRectangle());
                yield return new WaitForSeconds(4f);
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern2(int count = 0) //ºö ÆÐÅÏ
    {

        yield return SCoroutine(Pattern_BM(count));

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern3(int count = 0) //ÅÚ·¹Æ÷Æ® ÆÐÅÏ
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
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator Pattern4(int count = 0) //ÆÈ»¸±â ÆÐÅÏ, 2ÆäÀÌÁî¿¡¸¸ »ç¿ë
    {
        switch (NowPhase)
        {
            case 1:
                break;
            case 2:
                yield return StartCoroutine(Pattern_GA(count));
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }

    public override IEnumerator PatternFinal(int count = 0) //Èú¶ó, ±Ã±Ø±â
    {
        switch(NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_SM(count));
                break;
            case 2:
                yield return SCoroutine(bossFieldPattern.GhostUltStart());
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }
}
