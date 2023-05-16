using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GhostSkill : PlayerSkillBase
{
    [Header("장판스킬")]
    WaitForSeconds jangpanWait2 = new WaitForSeconds(2f);
    WaitForSeconds janpanWait = new WaitForSeconds(0.3f);
    float jangpanDealinterval = 0;
    float jangpanSize = 0f;
    float jangpanDuration = 0f;
    float jangPanDamage = 0f;
    float jangpanoverlapFloat = 0;
    Poolable smokePoolable = null;
    GameObject smoke = null;
    GameObject fiveSmoke = null;
    Color janpangFitrstColor = new Color(1, 1, 1);
    Color jangpanglastColor = new Color(0.7372551f, 0, 1);
    ParticleSystem smokeParticle = null;
    [Header("힐라 스킬")]
    float cicleRange = 2f;
    List<Poolable> poolMob = new List<Poolable>();
    WaitForSeconds hillaDuration = new WaitForSeconds(10f);
    SpriteRenderer ghostDash;

    [Header("빔 스킬")]
    float beamRotationDuration = 2;
    float rotateBeamAngle = 45f;
    float beamMoveSpeed = 3;
    [SerializeField]
    PlayerBeam playerBeam = null;
    private float beamDmg = 0.5f;
    private Vector3 beamDir;
    private float beamRot;
    private Poolable beam;
    private Poolable subBeamLeft;
    private Poolable subBeamRight;
    private Material beamFiveMat;
    WaitForSeconds beamWait = new WaitForSeconds(1f);
    Texture2D eyeEffect;
    Texture2D reverseEffect;
    [Header("텔레포트 스킬")]
    float telpoDamage = 37;
    float telpoVelocity = 50;
    float telpoDuration = 0.099999f;
    float telpoClawDuration = 1f;
    WaitForFixedUpdate telpWait = new WaitForFixedUpdate();
    WaitForSeconds waitClaw = new WaitForSeconds(0.025f);
    WaitForSeconds waitLastClaw = new WaitForSeconds(0.5f);
    [Header("솟아오르기 스킬")]
    float armDamage = 30f;
    private Vector3 joystickDir;
    WaitForSeconds waitArm = new WaitForSeconds(0.9f);
    WaitForSeconds waitFiveArm = new WaitForSeconds(1f);
    Material mat;
    GameObject boss = null;
    [Header("궁극기")]
    [SerializeField] GhostUltSignal ghostUlt;
    private void Awake()
    {
        Cashing();
        mat = Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Shader Graphs_GhostArm.mat");
        boss = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Player/Ghost/boss_devil_man.prefab");
        playerBeam = Managers.Resource.Load<GameObject>("Assets/10.Effects/player/Ghost/PlayerBeam.prefab").GetComponent<PlayerBeam>();
        smoke = Managers.Resource.Load<GameObject>("Assets/10.Effects/player/Ghost/PlayerSmoke.prefab");
        fiveSmoke = Managers.Resource.Load<GameObject>("Assets/10.Effects/player/Ghost/PlayerFiveSmoke.prefab");
        beamFiveMat = Managers.Resource.Load<Material>("Assets/10.Effects/player/Ghost/EyeEffectMat.mat");
        eyeEffect = Managers.Resource.Load<Texture2D>("Assets/10.Effects/player/Ghost/EyeEffectFinal.png");
        reverseEffect = Managers.Resource.Load<Texture2D>("Assets/10.Effects/player/Ghost/EyeeffectFinalRerverse.png");
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void FirstSkill(int level)
    {
        if (level == 5)
            StartCoroutine(Jangpan5Skill());
        else
            StartCoroutine(JanpangSkill());

    }
    protected override void SecondSkill(int level)
    {
        StartCoroutine(HillaSkill(level));
    }
    protected override void ThirdSkill(int level)
    {
        StartCoroutine(Beam(level));
    }
    protected override void ForuthSkill(int level)
    {
        StartCoroutine(TelpoSkill(level));
    }
    protected override void FifthSkill(int level)
    {
        if (level == 5)
        {
            StartCoroutine(ArmFiveSkill());
        }
        else
        {
            StartCoroutine(ArmSkill(level));
        }
    }
    protected override void UltimateSkill()
    {
        ghostUlt.UltSkillCast();
    }

    #region 스킬 구현

    protected override IEnumerator Dash()
    {
        List<Poolable> dashObjLength = new List<Poolable>();
        int dashNum;
        Vector3 firstPosition = transform.position;
        yield return base.Dash();
        dashNum = Mathf.RoundToInt(Vector3.Magnitude(firstPosition - transform.position));
        Vector3 playerPoss = transform.position - firstPosition;
        float angle = Mathf.Atan2(playerPoss.y, playerPoss.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        for (int i = 0; i < dashNum; i++)
        {
            dashObjLength.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/DashSmoke.prefab", firstPosition + playerPoss.normalized * i, angleAxis));
        }
    }

    IEnumerator JanpangSkill()
    {
        Collider2D[] attachObjs = null;
        float timer = 0;
        float timerA = 0;
        smokePoolable = Managers.Pool.Pop(smoke, transform);
        smokeParticle = smokePoolable.GetComponent<ParticleSystem>();
        smokeParticle.startSize = jangpanSize;
        yield return janpanWait;
        while (timer < jangpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > jangpanDealinterval)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, jangpanoverlapFloat, 1 << enemyLayer);
                for (int i = 0; i < attachObjs.Length; i++)
                {
                    attachObjs[i].GetComponent<IHittable>().OnDamage(jangPanDamage, 0);
                }
                timerA = 0;
            }
            yield return null;
        }
        smokeParticle.loop = false;
        yield return jangpanWait2;
        Managers.Pool.Push(smokePoolable);
        smokeParticle.loop = true;
    }
    IEnumerator Jangpan5Skill()
    {
        Collider2D[] attachObjs = null;
        Collider2D[] attachObj2 = null;
        List<Poolable> smokes = new List<Poolable>();
        float timer = 0;
        float timerA = 0;
        float timerB = 1;
        Poolable playerSmoke = Managers.Pool.Pop(fiveSmoke, transform);
        smokeParticle = playerSmoke.GetComponent<ParticleSystem>();
        smokeParticle.startSize = jangpanSize;
        yield return janpanWait;
        while (timer < jangpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            timerB += Time.deltaTime;
            if (timerA > jangpanDealinterval)
            {

                attachObj2 = Physics2D.OverlapCircleAll(transform.position, jangpanoverlapFloat, 1 << enemyLayer);
                for (int i = 0; i < smokes.Count; i++)
                {
                    attachObjs = Physics2D.OverlapCircleAll(smokes[i].transform.position, jangpanoverlapFloat, 1 << enemyLayer);
                    for (int j = 0; j < attachObjs.Length; j++)
                    {
                        attachObjs[j].GetComponent<IHittable>().OnDamage(jangPanDamage, 0);
                    }
                }
                for (int i = 0; i < attachObj2.Length; i++)
                {
                    attachObj2[i].GetComponent<IHittable>().OnDamage(jangPanDamage, 0);
                }
                timerA = 0;
            }
            if (timerB > 1f)
            {
                Poolable poolSmoke = Managers.Pool.Pop(fiveSmoke, transform.position);
                poolSmoke.GetComponent<ParticleSystem>().startSize = jangpanSize;
                smokes.Add(poolSmoke);
                timerB = 0;
            }
            yield return null;
        }
        for (int i = 0; i < smokes.Count; i++)
        {
            smokes[i].GetComponent<ParticleSystem>().loop = false;
        }
        smokeParticle.loop = false;
        yield return jangpanWait2;
        Managers.Pool.Push(playerSmoke);
        for (int i = 0; i < smokes.Count; i++)
        {
            Managers.Pool.Push(smokes[i]);
            smokes[i].GetComponent<ParticleSystem>().loop = true;
        }
        smokeParticle.loop = true;

    }
    protected override void FirstSkillUpdate(int level)
    {
        if (level == 5)
        {
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 0, 1);
        }
        else
        {
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 0, 0);
        }
        playerBase.PlayerTransformData.skill[0].skillDelay = 8;
        jangpanDuration = 4 + (level - 1) / 2;
        jangpanDealinterval = 0.1f;
        jangpanSize = 2 * level + 2;
        jangPanDamage = 1f;
        jangpanoverlapFloat = level / 3.5f * 2 + 0.57f;

    }

    IEnumerator HillaSkill(int level)
    {
        if (level <= 2)
        {
            for (int i = 0; i < level; i++)
            {
                poolMob.Add(Managers.Pool.PoolManaging("03.Prefabs/Player/Ghost/GhostMob11", transform.position + new Vector3(Mathf.Cos(Random.Range(0, 360f) * Mathf.Deg2Rad), Mathf.Sin(Random.Range(0, 360f) * Mathf.Deg2Rad), 0) * cicleRange, quaternion.identity));
            }
        }
        else if (2 < level && level <= 4)
        {
            for (int i = 0; i < level - 2; i++)
            {
                poolMob.Add(Managers.Pool.PoolManaging("Assets/03.Prefabs/Player/Ghost/GhostMob2.prefab", transform.position + new Vector3(Mathf.Cos(Random.Range(0, 360f) * Mathf.Deg2Rad), Mathf.Sin(Random.Range(0, 360f) * Mathf.Deg2Rad), 0) * cicleRange, quaternion.identity));
            }
        }
        else
        {
            poolMob.Add(Managers.Pool.PoolManaging("Assets/03.Prefabs/Player/Ghost/GhostEliteMob.prefab", transform.position + new Vector3(Mathf.Cos(Random.Range(0, 360f) * Mathf.Deg2Rad), Mathf.Sin(Random.Range(0, 360f) * Mathf.Deg2Rad), 0) * cicleRange, quaternion.identity));
        }
        yield return hillaDuration;
        for (int i = 0; i < poolMob.Count; i++)
        {
            Managers.Pool.Push(poolMob[i]);
        }
        poolMob.Clear();
    }
    protected override void SecondSkillUpdate(int level)
    {
        if (level == 5)
        {
            hillaDuration = new WaitForSeconds(10f);
            playerBase.PlayerTransformData.skill[4].skillDelay = 15;
        }
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 1, 0);
    }
    IEnumerator Beam(int level)
    {

        PlayerBeam playerBeam = null;
        List<Poolable> beamList = new List<Poolable>();

        beamRot = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(beamRot, transform.forward); ;

        if (level == 1)
        {
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position, angleAxis));
            playerBeam = beamList[0].GetComponent<PlayerBeam>();
            playerBeam.damage = beamDmg;
            yield return new WaitUntil(() => playerBeam.IsReady);
        }
        else if (level == 2)
        {
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, -90) * playerMovement.Direction), Quaternion.AngleAxis(beamRot - 15, transform.forward)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, 90) * playerMovement.Direction), Quaternion.AngleAxis(beamRot + 15, transform.forward)));
            playerBeam = beamList[0].GetComponent<PlayerBeam>();
            for (int i = 0; i < beamList.Count; i++)
            {
                beamList[i].GetComponent<PlayerBeam>().damage = beamDmg;
            }
            yield return new WaitUntil(() => playerBeam.IsReady);
        }
        else if (level == 3)
        {
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position, angleAxis));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, -90) * playerMovement.Direction * 2f), Quaternion.AngleAxis(beamRot - 15, transform.forward)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, 90) * playerMovement.Direction * 2f), Quaternion.AngleAxis(beamRot + 15, transform.forward)));
            playerBeam = beamList[0].GetComponent<PlayerBeam>();
            for (int i = 0; i < beamList.Count; i++)
            {
                beamList[i].GetComponent<PlayerBeam>().damage = beamDmg;
            }
            yield return new WaitUntil(() => playerBeam.IsReady);
        }
        else if (level == 4)
        {

            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (beamRot % 90 == 0 ? Vector3.up : new Vector3(-1, 1, 0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, 135)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (beamRot % 90 == 0 ? Vector3.down : new Vector3(1, -1, 0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 270) : Quaternion.Euler(0, 0, 315)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (beamRot % 90 == 0 ? Vector3.right : new Vector3(1, 1, 0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 45)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (beamRot % 90 == 0 ? Vector3.left : new Vector3(-1, -1, 0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.Euler(0, 0, 225)));
            for (int i = 0; i < beamList.Count; i++)
            {
                beamList[i].GetComponent<PlayerBeam>().damage = beamDmg;
            }
            playerBeam = beamList[0].GetComponent<PlayerBeam>();
            yield return new WaitUntil(() => playerBeam.timerA > float.Epsilon);
            while (playerBeam.timerA < playerBeam.beamDuration)
            {
                for (int i = 0; i < beamList.Count; i++)
                {
                    beamList[i].transform.Rotate(new Vector3(0, 0, 180 / playerBeam.beamDuration * Time.deltaTime));
                }
                yield return null;
            }
        }
        else if (level == 5)
        {
            Vector3 currentPostion = transform.position;
            float timer = 0;
            Poolable leftBeam = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", currentPostion, Quaternion.AngleAxis(beamRot + 45, transform.forward));
            Poolable rightBeam = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", currentPostion, Quaternion.AngleAxis(beamRot - 45, transform.forward));
            PlayerBeam playerBeam1 = leftBeam.GetComponent<PlayerBeam>();
            LineRenderer lineRenderer = playerBeam1.GetComponentInChildren<LineRenderer>();
            lineRenderer.sortingOrder++;
            PlayerBeam playerBeam2 = rightBeam.GetComponent<PlayerBeam>();
            leftBeam.GetComponent<PlayerBeam>().damage = beamDmg;
            rightBeam.GetComponent<PlayerBeam>().damage = beamDmg;
            while (timer < beamRotationDuration)
            {
                playerBeam1.timerA = 0;
                playerBeam2.timerA = 0;
                leftBeam.transform.Rotate(new Vector3(0, 0, -45 * Time.deltaTime / beamRotationDuration));
                rightBeam.transform.Rotate(new Vector3(0, 0, 45 * Time.deltaTime / beamRotationDuration));
                timer += Time.deltaTime;
                yield return null;
            }
            Poolable fiveBeam = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/Beam5Effect.prefab", currentPostion, angleAxis);
            playerBeam = fiveBeam.GetComponent<PlayerBeam>();
            ParticleSystem beamParticle = fiveBeam.GetComponent<ParticleSystem>();
            beamParticle.startRotation = -beamRot * Mathf.Deg2Rad;

            playerBeam.enabled = false;
            beamParticle.Pause();
            yield return beamWait;
            beamParticle.Play();
            playerBeam2.beamLight.intensity = 0;
            playerBeam1.beamLight.intensity = 0;
            yield return new WaitUntil(() => beamParticle.time > 0.99f);

            Managers.Pool.Push(leftBeam);
            Managers.Pool.Push(rightBeam);
            beamParticle.Pause();
            playerBeam.enabled = true;
            yield return new WaitUntil(() => playerBeam.timerA > 0);
            CinemachineCameraShaking.Instance.CameraShake(1, 0.25f);
            yield return new WaitUntil(() => !playerBeam.IsReady);
            beamFiveMat.SetTexture("_MainTex", reverseEffect);
            beamParticle.time = 0;
            beamParticle.Play();
            yield return new WaitUntil(() => beamParticle.time > 0.99f);
            lineRenderer.sortingOrder--;
            Managers.Pool.Push(fiveBeam);
            beamFiveMat.SetTexture("_MainTex", eyeEffect);
        }

        yield return new WaitUntil(() => !playerBeam.IsReady);
        for (int i = 0; i < beamList.Count; i++)
        {
            Managers.Pool.Push(beamList[i]);
        }
    }

    protected override void ThirdSkillUpdate(int level)
    {
        if (level == 5)
        {
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 2, 1);
        }
        else
        {
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 2, 0);
        }
        beamDmg = level + 2;
    }
    IEnumerator TelpoSkill(int level)
    {
        float timer = 0;
        RaycastHit2D[] hit;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        Vector3 playerPos = transform.position;
        Vector3 changePos = transform.position;
        playerRigid.velocity = telpoVelocity * playerMovement.Direction;

        float angle = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Ghost/G_Claw.mp3");
        while (timer < telpoDuration)
        {
            timer += Time.fixedDeltaTime;
            //print(MathF.Sqrt(Vector2.SqrMagnitude(transform.position - changePos)));
            if (Vector2.SqrMagnitude(transform.position - changePos) > (2 * 2) - 0.001f)
            {
                Poolable telpoEffect;
                if (level == 5)
                {
                    telpoEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/TelpoFiveEffect.prefab", changePos, angleAxis);
                }
                else
                {
                    telpoEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/TelpoEffect.prefab", changePos, angleAxis);
                }
                VisualEffect[] effects = telpoEffect.GetComponentsInChildren<VisualEffect>();
                for (int i = 0; i < effects.Length; i++)
                {
                    effects[i].Play();
                }
                changePos = transform.position;
            }
            yield return telpWait;
        }
        playerRigid.velocity = Vector3.zero;
        hit = Physics2D.BoxCastAll(playerPos, new Vector2(2, 1), 0, transform.position - playerPos, Vector2.Distance(transform.position, playerPos), 1 << enemyLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            hit[i].transform.GetComponent<IHittable>().OnDamage(telpoDamage, 0);
        }
        if (level == 5)
        {

            Collider2D[] hitEnemies;
            float timerA = 0;
            while (timerA < telpoClawDuration)
            {
                Poolable clawEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerClaw.prefab", transform.position, angleAxis);
                clawEffect.transform.localScale = new Vector3(Random.Range(2, 5f), Random.Range(2, 5f), 1);
                clawEffect.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                clawEffect.transform.GetComponent<VisualEffect>().Play();
                hitEnemies = Physics2D.OverlapCircleAll(transform.position, 5, 1 << enemyLayer);
                for (int i = 0; i < hitEnemies.Length; i++)
                {
                    hitEnemies[i].transform.GetComponent<IHittable>().OnDamage(2, 0);
                }
                timerA += 0.025f;
                yield return waitClaw;
            }
            yield return waitLastClaw;
            for (int i = -1; i < 6; i++)
            {
                Poolable a = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerClaw 1.prefab", transform.position, Quaternion.Euler(0, 0, 45));
                a.transform.localScale = new Vector3(7, 4, 1);
                a.transform.rotation = Quaternion.Euler(0, 0, 30 * i);
                a.GetComponent<VisualEffect>().Play();
            }

            hitEnemies = Physics2D.OverlapCircleAll(transform.position, 7, 1 << enemyLayer);
            for (int i = 0; i < hitEnemies.Length; i++)
            {
                hitEnemies[i].transform.GetComponent<IHittable>().OnDamage(30, 0);
            }
        }
        playerMovement.IsMove = true;
        player.IsInvincibility = false;
    }
    protected override void ForuthSkillUpdate(int level)
    {
        telpoDamage = level + 37;
        if (level == 5)
        {
            telpoVelocity = 100;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 3, 1);
            return;
        }
        else if (level == 4)
        {
            telpoVelocity = 85;
        }
        else if (level == 3)
        {
            telpoVelocity = 75;
        }
        else
        {
            telpoVelocity = 50;
        }
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 3, 0);
    }
    IEnumerator ArmSkill(int level)
    {
        RaycastHit2D[] hitEnemies;
        List<Poolable> arms = new List<Poolable>();
        if (level == 1)
        {
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/LeftArm.prefab", transform.position + (Vector3)playerMovement.Direction * 3 + Vector3.up * 1.5f, Quaternion.identity));
        }
        if (level == 2)
        {
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/LeftArm.prefab", transform.position + Vector3.left * 3 + Vector3.up * 1.5f, Quaternion.Euler(0, 180, 0)));
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightArm.prefab", transform.position + Vector3.right * 3 + Vector3.up * 1.5f, Quaternion.identity));
        }
        if (level == 3)
        {   
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightArm.prefab", transform.position + Vector3.right * 3 , Quaternion.identity));
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightArm.prefab", transform.position + Vector3.left * 3 , Quaternion.Euler(0, 180, 0)));
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/LeftArm.prefab", transform.position + Vector3.up * 3.5f , Quaternion.Euler(0, 180, 0)));
        }
        if(level == 4)
        {
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightArm.prefab", transform.position + Vector3.right * 3 + Vector3.up, Quaternion.identity));
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightArm.prefab", transform.position + Vector3.left * 3 + Vector3.up, Quaternion.Euler(0, 180, 0)));
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/LeftArm.prefab", transform.position + Vector3.up * 3 + Vector3.left*2, Quaternion.Euler(0, 180, 0)));
            arms.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightArm.prefab", transform.position + Vector3.right *2 + Vector3.up *3, Quaternion.identity));
        }
        mat.SetFloat("_StepValue", arms[0].transform.position.y - 2);
        for (int i = 0; i < arms.Count; i++)
        {

            hitEnemies = Physics2D.BoxCastAll(arms[i].transform.position + Vector3.down * 2, new Vector2(2, 1f), 0,Vector2.up, 5,1 << enemyLayer);
            for (int j = 0; j < hitEnemies.Length; j++)
            {
                hitEnemies[j].transform.GetComponent<IHittable>().OnDamage(armDamage, 0);
            }
        }
        yield return waitArm;
        for(int i = 0; i < arms.Count; i++)
        {

        Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/ArnFinishEffect.prefab", arms[i].transform.position + Vector3.down * 2, Quaternion.identity);
        }
        yield return null;
    }
    IEnumerator ArmFiveSkill()
    {

        RaycastHit2D[] hitEnemies;
        Poolable bossSprite = Managers.Pool.Pop(boss, transform.position + Vector3.up * 3);
        Animator animator = bossSprite.GetComponentInChildren<Animator>();
        SpriteRenderer[] bossSprites = bossSprite.transform.GetComponentsInChildren<SpriteRenderer>();
        Color fadeColor = new Color(1, 1, 1, 0);
        animator.SetTrigger("Start");
        while (fadeColor.a < 1)
        {
            for (int i = 0; i < bossSprites.Length; i++)
            {
                bossSprites[i].color = fadeColor;
            }

            fadeColor.a += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.67);
        Poolable leftArm = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightFiveArm.prefab", bossSprite.transform.position + Vector3.left * 4 + Vector3.down * 5, Quaternion.Euler(0, 180, 0));
        Poolable rightArm = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightFiveArm.prefab", bossSprite.transform.position + Vector3.right * 4 + Vector3.down * 5, Quaternion.identity);
        Poolable leftTopArm = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightFiveArm.prefab", bossSprite.transform.position + Vector3.left * 4 + Vector3.down * 3, Quaternion.Euler(0, 0, 270));
        leftTopArm.transform.Find("ArmOutEffect").GetComponent<ParticleSystem>().startRotation = 0;
        Poolable rightTopArm = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightFiveArm.prefab", bossSprite.transform.position + Vector3.right * 4 + Vector3.down, Quaternion.Euler(0, 0, 90));
        rightTopArm.transform.Find("ArmOutEffect").GetComponent<ParticleSystem>().startRotation = -180 * Mathf.Deg2Rad;
        hitEnemies = Physics2D.BoxCastAll(leftArm.transform.position - Vector3.down, new Vector2(4, 1), 0, Vector2.up, 10, 1 << enemyLayer);
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            hitEnemies[i].transform.GetComponent<IHittable>().OnDamage(armDamage, 0);
        }
        hitEnemies = Physics2D.BoxCastAll(rightArm.transform.position - Vector3.down, new Vector2(4, 1), 0, Vector2.up, 10, 1 << enemyLayer);
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            hitEnemies[i].transform.GetComponent<IHittable>().OnDamage(armDamage, 0);
        }
        hitEnemies = Physics2D.BoxCastAll(rightArm.transform.position - Vector3.left, new Vector2(1, 4), 0, Vector2.right, 10, 1 << enemyLayer);
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            hitEnemies[i].transform.GetComponent<IHittable>().OnDamage(armDamage, 0);
        }
        hitEnemies = Physics2D.BoxCastAll(rightArm.transform.position - Vector3.right, new Vector2(1, 4), 0, Vector2.left, 10, 1 << enemyLayer);
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            hitEnemies[i].transform.GetComponent<IHittable>().OnDamage(armDamage, 0);
        }
        mat.SetFloat("_StepValue", leftArm.transform.position.y - 1);
        yield return waitFiveArm;
        while (fadeColor.a > 0)
        {
            for (int i = 0; i < bossSprites.Length; i++)
            {
                bossSprites[i].color = fadeColor;
            }

            fadeColor.a -= Time.deltaTime;
            yield return null;
        }
        leftTopArm.transform.Find("ArmOutEffect").GetComponent<ParticleSystem>().startRotation = 270 * Mathf.Deg2Rad;
        rightTopArm.transform.Find("ArmOutEffect").GetComponent<ParticleSystem>().startRotation = 270 * Mathf.Deg2Rad;
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Hide.prefab", bossSprite.transform.position + Vector3.up * 2, Quaternion.identity);
        Managers.Pool.Push(bossSprite);
    }
    protected override void FifthSkillUpdate(int level)
    {
        armDamage = 29 + level;
        if (level == 5)
        {
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 4, 1);
        }
        else
        {
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 4, 0);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(2, 5));
    }





}
#endregion


