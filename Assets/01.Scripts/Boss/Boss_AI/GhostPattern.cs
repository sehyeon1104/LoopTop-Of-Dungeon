using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.PostProcessing;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] protected AnimationClip tpStart;
    [SerializeField] protected AnimationClip absorbEnd;

    [SerializeField] protected Material panicMat;
    [SerializeField] protected CinemachineVirtualCamera boss2PhaseVcam;

    protected ShowDamagePopUp showDmg;
    protected GhostBossJangpanPattern bossRangePattern;

    protected CanvasGroup playerPCUI;
    protected CanvasGroup playerPPUI;

    protected List<Poolable> mobList = new List<Poolable>();
    protected WaitForSeconds waitTime = new WaitForSeconds(1f);

    public int panicValue = 0;
    #endregion

    private void Awake()
    {
        showDmg = FindObjectOfType<ShowDamagePopUp>();
        bossRangePattern = GetComponent<GhostBossJangpanPattern>();
        if (boss2PhaseVcam == null)
            boss2PhaseVcam = GetComponentInChildren<CinemachineVirtualCamera>();
        bossAura.SetActive(false);

        playerPCUI = GameObject.Find("PCPlayerUI").transform.Find("UltFade").GetComponent<CanvasGroup>();
        playerPPUI = GameObject.Find("PPPlayerUI").GetComponent<CanvasGroup>();

        panicMat.SetFloat("_VigIntensity", 0);
    }

    #region pattern1
    public IEnumerator Pattern_BM(int count) //ºö
    {
        WaitForSeconds wait = NowPhase == 1 ? new WaitForSeconds(1.75f) : new WaitForSeconds(1.5f);

        yield return null;

        for (int i = 0; i < count; i++)
        {
            Vector2 dir = Boss.Instance.player.position - transform.position;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Warning.wav");

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

        Boss.Instance.bossAnim.overrideController[$"Skill3"] = tpStart;
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Teleport3.wav");
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Teleport.prefab", transform.position + Vector3.down, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        bossObject.SetActive(false);

        Boss.Instance.isBInvincible = true;
        Managers.Pool.PoolManaging("10.Effects/ghost/Hide",transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2.5f);

        dir = Boss.Instance.player.position - transform.position;
        Vector3 scale = transform.localScale;
        scale = Boss.Instance.bossMove.CheckFlipValue(dir, scale);

        transform.position = Vector3.left * scale.x * 3f + Boss.Instance.player.position;

        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Teleport.prefab", transform.position + Vector3.down, Quaternion.identity);
        
        yield return new WaitForSeconds(0.25f);
        Boss.Instance.isBInvincible = false;
        bossObject.SetActive(true);

        Boss.Instance.bossAnim.overrideController[$"Skill3"] = Phase_One_AnimArray[2];
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        yield return waitTime;
    }
    public IEnumerator Pattern_SM(int count) //Èú¶ó
    {
        int finalCount = 0;
        bool isCanceled = false;
        WaitForSeconds waitTime = new WaitForSeconds(2f);

        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

        Managers.Pool.PoolManaging("10.Effects/ghost/Absorb", transform.position + Vector3.down * 2f, Quaternion.identity);
        for (int i = 0; i < count; i++)
        {
            Poolable clone = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Ghost/Normal/G_Mob_01.prefab", new Vector2(Random.Range(-2.5f, 29.5f), Random.Range(-3, 17.5f)), Quaternion.identity);
            mobList.Add(clone);
        }

        SummonTimer.SetActive(true);

        Boss.Instance.isBInvincible = true;

        int nowCount = mobList.Count;
        for (int i = 1; i < 13; i++)
        {
            yield return waitTime;
            for(int j = 0; j < nowCount; j++)
            {
                if (!mobList[j].isActiveAndEnabled)
                {
                    nowCount--;
                    mobList.RemoveAt(j);
                }
            }
            if(nowCount == 0)
            {
                isCanceled = true;
                break;
            }
            SummonClock.fillAmount = (float)i / 12;
        }

        Boss.Instance.isBInvincible = false;

        SummonClock.fillAmount = 0;
        SummonTimer.SetActive(false);

        if (isCanceled)
        {
            CinemachineCameraShaking.Instance.CameraShake(5, 0.4f);
            Boss.Instance.bossAnim.overrideController[$"SkillFinal"] = Boss.Instance.bossAnim.deathClip;
            Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
            yield return new WaitForSeconds(10f);
            Boss.Instance.bossAnim.anim.SetBool("FinalEnd", true);
            Boss.Instance.bossAnim.anim.ResetTrigger(Boss.Instance._hashAttack);
            yield return new WaitForSeconds(0.5f);
        }

        else
        {
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
            Boss.Instance.bossAnim.anim.SetBool("FinalEnd", true);

            int hpFinal = Boss.Instance.Base.Hp + finalCount * 10;
            while (Boss.Instance.Base.Hp < hpFinal && Boss.Instance.Base.Hp < Boss.Instance.Base.MaxHp)
            {
                Boss.Instance.Base.Hp += 1;
                yield return null;
            }
            mobList.Clear();

            yield return new WaitForSeconds(2f);
        }
    }
    public IEnumerator Pattern_SB(int count) //ºÒ¸´
    {
        float angle = Random.Range(0f, 360f);
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/AllBullet.prefab", GameManager.Instance.Player.transform.position, Quaternion.Euler(Vector3.forward * angle));
        yield return new WaitForSeconds(0.5f);
    }
    #endregion
    #region pattern2
    public IEnumerator Pattern_TP_2(int count) //ÅÚÆ÷ 2ÆäÀÌÁî
    {
        Vector2 dir;
        Vector3 scale;

        Boss.Instance.bossAnim.overrideController[$"Skill3"] = tpStart;

        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Teleport3.wav");
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Teleport.prefab", transform.position + Vector3.down, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        bossObject.SetActive(false);
        bossAura.SetActive(false);
        Boss.Instance.isBInvincible = true;
        Managers.Pool.PoolManaging("10.Effects/ghost/Hide", transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 2; i++)
        {
            bossObject.SetActive(false);
            bossAura.SetActive(false);
            Boss.Instance.isBInvincible = true;

            Vector3 pos = new Vector3(Random.Range(0f, 28.5f), Random.Range(-2f, 15.5f));
            Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Teleport.prefab", pos, Quaternion.identity);

            dir = (Boss.Instance.player.position - pos).normalized;
            Vector3 anPos = new Vector3(pos.x + dir.x * 30f, pos.y + dir.y * 25f);
            Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Teleport.prefab", anPos, Quaternion.identity);

            yield return new WaitForSeconds(0.1f);
            transform.position = pos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Managers.Pool.PoolManaging("Assets/10.Effects/ghost/BossTpWarning.prefab", pos + (anPos - pos) / 2, Quaternion.AngleAxis(angle - 90, Vector3.forward));

            yield return new WaitForSeconds(1f);
            CinemachineCameraShaking.Instance.CameraShake(4, 0.2f);
            transform.DOMove(anPos, 0.5f);

            Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/BossTpEffect.prefab", pos + (anPos - pos)/2, Quaternion.AngleAxis(angle - 90, Vector3.forward));
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Ghost/G_Claw.mp3");

            scale = transform.localScale;
            Boss.Instance.bossMove.CheckFlipValue(dir, scale);

            Boss.Instance.isBInvincible = false;
            bossObject.SetActive(true);
            bossAura.SetActive(true);

            yield return new WaitForSeconds(0.1f);
            Collider2D col = Physics2D.OverlapBox(clone.transform.position, new Vector2(2.5f, 20f), clone.transform.eulerAngles.z, 1 << 8);
            if (col != null)
                col.GetComponent<IHittable>().OnDamage(15, 0);

            yield return waitTime;

            bossObject.SetActive(false);
            bossAura.SetActive(false);
            Boss.Instance.isBInvincible = true;
        }

        yield return new WaitForSeconds(1.5f);

        Boss.Instance.isBInvincible = false;
        bossObject.SetActive(true);
        bossAura.SetActive(true);

        dir = Boss.Instance.player.position - transform.position;
        scale = transform.localScale;
        scale = Boss.Instance.bossMove.CheckFlipValue(dir, scale);

        transform.position = Vector3.left * scale.x * 3f + Boss.Instance.player.position;

        Boss.Instance.bossAnim.overrideController[$"Skill3"] = Phase_One_AnimArray[2];
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        yield return waitTime;

    }
    public IEnumerator Pattern_SB_2(int count) //ºÒ¸´ 2ÆäÀÌÁî
    {
        for (int i = 0; i < 5; i++)
        {
            float angle = Random.Range(0f, 360f);
            int randomNum = Random.Range(0, 5);
            string bulletType = "";
            for (int j = 0; j < 5; j++)
            {
                bulletType = j != randomNum?
                    "Assets/10.Effects/ghost/Bubble&Bullet/BulletGroup.prefab" : 
                    "Assets/10.Effects/ghost/Bubble&Bullet/SafeBulletGroup.prefab";

                Managers.Pool.PoolManaging(bulletType, GameManager.Instance.Player.transform.position, Quaternion.Euler(Vector3.forward * (angle + j * 15f)));
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public IEnumerator Pattern_GA(int count) //ÆÈ»¸±â
    {
        bossObject.SetActive(false);
        bossAura.SetActive(false);
        Boss.Instance.isBDamaged = true;
        Managers.Pool.PoolManaging("10.Effects/ghost/Hide", transform.position, Quaternion.identity);

        yield return StartCoroutine(bossFieldPattern.GhostBossArmPattern());

        bossObject.SetActive(true);
        bossAura.SetActive(NowPhase == 2);
        Boss.Instance.isBDamaged = false;
    }
    #endregion
}

public class GhostPattern : G_Patterns
{
    Coroutine ActCoroutine = null;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

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
                SummonTimer.gameObject.SetActive(false);
            }
        }

        SetPanicValue();
        base.Update();
    }
    public override int GetRandomCount(int choisedPattern)
    {
        switch (choisedPattern)
        {
            case 0:
                break;
            case 1:
                return NowPhase == 1 ? 3 : 5;
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
    public override void SetPatternWeight()
    {
        switch (NowPhase)
        {
            case 1:
                patternWeight[0] = 10;
                patternWeight[1] = 30;
                patternWeight[2] = 20;
                patternWeight[3] = 40;
                break;
            case 2:
                patternWeight[0] = 15;
                patternWeight[1] = 35;
                patternWeight[2] = 25;
                patternWeight[3] = 20;
                patternWeight[4] = 5;
                break;
        }

    }

    protected override IEnumerator ChangePhase()
    {
        yield return new WaitUntil(() => NowPhase == 1 && Boss.Instance.Base.Hp <= 0);
        GameManager.Instance.Player.GetComponent<PlayerMovement>().IsControl = false;
        GameManager.Instance.Player.IsInvincibility = true;
        isThisSkillCoolDown[patternChoice] = false;

        if (Boss.Instance.actCoroutine != null)
            StopCoroutine(Boss.Instance.actCoroutine);

        Boss.Instance.actCoroutine = null;

        GhostBossUI.Instance.fillTime = 50f;
        nowBPhaseChange = true;
        Boss.Instance.isBInvincible = true;
        Boss.Instance.bossAnim.anim.SetBool("FinalEnd", true);

        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Phase2.prefab", transform.position, Quaternion.identity);
        playerPCUI.alpha = 0;
        playerPPUI.alpha = 0;

        yield return new WaitForSeconds(0.3f);

        Boss.Instance.bossAnim.anim.ResetTrigger(Boss.Instance._hashAttack);
        Boss.Instance.bossAnim.anim.SetTrigger(Boss.Instance._hashPhase);

        boss2PhaseVcam.Priority = 11;
        Managers.Sound.Play("Assets/05.Sounds/BGM/Ghost/Boss_Ghost_Two.mp3", Define.Sound.Bgm, 1, 1);

        yield return new WaitForSeconds(0.25f);
        CinemachineCameraShaking.Instance.CameraShake(1.5f, 3f);

        while (Boss.Instance.Base.Hp < Boss.Instance.Base.MaxHp)
        {
            Boss.Instance.Base.Hp += 8;
            yield return endOfFrame;
        }

        yield return new WaitForSeconds(2.5f);

        Vector3 dir = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        GameManager.Instance.Player.GetComponent<Rigidbody2D>().AddRelativeForce(dir * 10f, ForceMode2D.Impulse);
        bossAura.SetActive(true);

        yield return new WaitForSeconds(2f);
        GameManager.Instance.Player.IsInvincibility = true;
        GameManager.Instance.Player.GetComponent<PlayerMovement>().IsControl = true;
        
        Boss.Instance.Base.Hp = Boss.Instance.Base.MaxHp;
        
        isCanUseFinalPattern = true;
        isUsingFinalPattern = false;
        
        boss2PhaseVcam.Priority = 0;
        patternDelay = new WaitForSeconds(1.2f);
        NowPhase = 2;
        playerPCUI.alpha = 1;
        playerPPUI.alpha = 1;

        SetPatternWeight();
        Boss.Instance.bossAnim.overrideController = Boss.Instance.bossAnim.SetSkillAnimation(Boss.Instance.bossAnim.overrideController);

        yield return patternDelay;

        Boss.Instance.isBInvincible = false;
        nowBPhaseChange = false;

        Boss.Instance.Phase2();

    }
    private void SetPanicValue()
    {
        float fillTime = GhostBossUI.Instance.fillTime;
        if (Boss.Instance.isBDead)
            fillTime = 50f;

        if (fillTime > 70f)
        {
            panicMat.SetFloat("_VigIntensity", (fillTime - 70) * 0.01f + 0.3f);
            if (panicValue == Mathf.CeilToInt((fillTime - 70) * 0.1f)) return;

            if (panicValue < 1)
            {
                Managers.Pool.PoolManaging("Assets/10.Effects/ghost/P_AtkUp.prefab", GameManager.Instance.Player.transform);
                Managers.Pool.PoolManaging("Assets/10.Effects/ghost/B_AtkUp.prefab", transform);

                panicMat.SetColor("_Color", new Color(17f, 0, 0.8f));
            }

            panicValue = Mathf.CeilToInt((fillTime - 70) * 0.1f);
            Boss.Instance.dmgMul = Mathf.Pow(2, panicValue - 1) * 0.25f + 1;
            GameManager.Instance.Player.dmgMul = Mathf.Pow(2, panicValue - 1) * 0.5f + 1;
        }
        else if (fillTime < 30f)
        {
            panicMat.SetFloat("_VigIntensity", (0.2f - (fillTime - 30) * 0.005f) + 0.15f);
            if (panicValue == Mathf.FloorToInt(fillTime * 0.1f) - 3) return;

            if (panicValue > -1)
            {
                Managers.Pool.PoolManaging("Assets/10.Effects/ghost/P_DefUp.prefab", GameManager.Instance.Player.transform);
                Managers.Pool.PoolManaging("Assets/10.Effects/ghost/B_DefUp.prefab", transform);

                panicMat.SetColor("_Color", new Color(0, 16.5f, 9.3f));
            }

            panicValue = Mathf.FloorToInt(fillTime * 0.1f) - 3;
            Boss.Instance.dmgMul = (panicValue * 0.25f) + 1;
            GameManager.Instance.Player.dmgMul = (panicValue * 0.25f) + 1;
        }
        else if (panicValue != 0)
        {
            panicValue = 0;
            panicMat.SetFloat("_VigIntensity", 0);
            Boss.Instance.dmgMul = 1;
            GameManager.Instance.Player.dmgMul = 1;
        }
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
                yield return StartCoroutine(bossRangePattern.FloorPatternRectangle());
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
                yield return SCoroutine(Pattern_TP_2(count));
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }
    public override IEnumerator Pattern4(int count = 0) //ºÒ¸´ ÆÐÅÏ
    {
        switch (NowPhase)
        {
            case 1:
                yield return SCoroutine(Pattern_SB(count));
                break;
            case 2:
                yield return SCoroutine(Pattern_SB_2(count));
                break;
        }

        yield return null;
        Boss.Instance.actCoroutine = null;
    }
    public override IEnumerator Pattern5(int count = 0) //ÆÈ¼Ú¾Æ¿À¸£±â
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
