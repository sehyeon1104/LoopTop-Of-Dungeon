using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GhostSkill : PlayerSkillBase
{
    [Header("ÀåÆÇ½ºÅ³")]
    WaitForSeconds jangpanWait2 = new WaitForSeconds(2f);
    WaitForSeconds janpanWait = new WaitForSeconds(0.3f);
    float jangpanDealinterval = 0;
    float jangpanSize = 0f;
    float jangpanDuration = 0f;
    float jangPanDamage = 0f;
    float jangpanoverlapFloat = 0;
    Poolable smokePoolable = null;
    GameObject smoke = null;
    ParticleSystem smokeParticle = null;
    [Header("Èú¶ó ½ºÅ³")]
    float cicleRange = 2f;
    List<Poolable> poolMob = new List<Poolable>();
    WaitForSeconds hillaDuration = new WaitForSeconds(10f);
    [SerializeField]
    private float attackRange = 1f;
    WaitForSeconds telpoDuration = new WaitForSeconds(0.1f);
    float telpoVelocity = 50f;
    Animator playerAnim;
    SpriteRenderer ghostDash;

    [Header("ºö ½ºÅ³")]
    float beamRotationDuration = 4;
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
    [Header("¼Ú¾Æ¿À¸£±â ½ºÅ³")]
    float armSpeed = 10f;
    private Vector3 joystickDir;
    [Header("±Ã±Ø±â")]
    [SerializeField] GhostUltSignal ghostUlt;
    private void Awake()
    {
        Cashing();
        playerBeam = FindObjectOfType<PlayerBeam>();    
        playerAnim = GetComponent<Animator>();
        smoke = Managers.Resource.Load<GameObject>("Assets/10.Effects/player/Ghost/PlayerSmoke.prefab");
    }
    private void Start()
    {
    }
    protected override void Attack()
    {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            return;


        playerAnim.SetTrigger("Attack");
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss"))
            {
                CinemachineCameraShaking.Instance.CameraShake();
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, GameManager.Instance.Player.playerBase.CritChance);

            }
        }
    }
    protected override void FirstSkill(int level)
    {
        if (level == 5)
            StartCoroutine(Jangpan5Skill());
        else
            StartCoroutine(JanpangSkill(level));

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
        StartCoroutine(ArmSkill(level));
    }
    protected override void UltimateSkill()
    {
        ghostUlt.UltSkillCast();
    }

    protected override void DashSkill()
    {
        StartCoroutine(Dash());
    }

    #region ½ºÅ³ ±¸Çö
    IEnumerator JanpangSkill(int level)
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
        Poolable playerSmoke = Managers.Pool.Pop(smoke, transform);
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
                Poolable poolSmoke = Managers.Pool.Pop(smoke, transform.position);
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
        if(level == 5)
        {
            hillaDuration = new WaitForSeconds(10f);
            playerBase.PlayerTransformData.skill[4].skillDelay = 15;
        }
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
            yield return new WaitUntil(() => playerBeam.IsReady);
        }
        else if (level == 2)
        {
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, -90) * playerMovement.Direction), Quaternion.AngleAxis(beamRot - 15, transform.forward)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, 90) * playerMovement.Direction), Quaternion.AngleAxis(beamRot + 15, transform.forward)));
            playerBeam = beamList[0].GetComponent<PlayerBeam>();
            yield return new WaitUntil(() => playerBeam.IsReady);
        }
        else if (level == 3)
        {
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position, angleAxis));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, -90) * playerMovement.Direction * 2f), Quaternion.AngleAxis(beamRot - 15, transform.forward)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, 90) * playerMovement.Direction * 2f), Quaternion.AngleAxis(beamRot + 15, transform.forward)));
            playerBeam = beamList[0].GetComponent<PlayerBeam>();
            yield return new WaitUntil(() => playerBeam.IsReady);
        }
        else if (level == 4)
        {

            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position +( beamRot % 90 == 0 ? Vector3.up : new Vector3(-1,1,0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, 135)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (beamRot % 90 == 0 ? Vector3.down : new Vector3(1, -1, 0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 270) : Quaternion.Euler(0, 0, 315)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (beamRot % 90 == 0 ? Vector3.right : new Vector3(1, 1, 0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 45)));
            beamList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (beamRot % 90 == 0 ? Vector3.left : new Vector3(-1, -1, 0)), beamRot % 90 == 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.Euler(0, 0, 225)));

            playerBeam = beamList[0].GetComponent<PlayerBeam>();

            while (playerBeam.timerA < playerBeam.beamDuration)
            {
                if (playerBeam.IsReady)
                {
                    for (int i = 0; i < beamList.Count; i++)
                    {
                        beamList[i].transform.Rotate(new Vector3(0, 0, 180 / playerBeam.beamDuration * Time.deltaTime));
                    }
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
            PlayerBeam playerBeam2 = rightBeam.GetComponent<PlayerBeam>();
            while (timer < beamRotationDuration)
            {
                playerBeam1.timerA = 0;
                playerBeam2.timerA = 0;
                leftBeam.transform.Rotate(new Vector3(0, 0, -45 * Time.deltaTime / beamRotationDuration));
                rightBeam.transform.Rotate(new Vector3(0, 0, 45  * Time.deltaTime / beamRotationDuration));
                timer += Time.deltaTime;
                yield return null;
            }
            Poolable fiveBeam = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/Beam5Effect.prefab", currentPostion, angleAxis);
            playerBeam = fiveBeam.GetComponent<PlayerBeam>();
            ParticleSystem beamParticle = fiveBeam.GetComponent<ParticleSystem>();
            playerBeam.enabled = false;
            playerBeam.beamDuration = 2f;
            yield return null;
            beamParticle.Pause();
            yield return new WaitForSeconds(1f);
            Managers.Pool.Push(leftBeam);
            Managers.Pool.Push(rightBeam);
            beamParticle.Play();
            while (playerBeam.beamDuration > playerBeam.timerA)
            {
                if (beamParticle.time > 0.99f)
                {
                    beamParticle.Pause();
                    playerBeam.enabled = true;
                }
                yield return null;
            }
        }

        yield return new WaitUntil(() => !playerBeam.IsReady);
       
        for (int i = 0; i < beamList.Count; i++)
        {
            Managers.Pool.Push(beamList[i]);
        }
    }
    
    protected override void ThirdSkillUpdate(int level)
    {
        playerBeam.damage = level + 1;
    }
    IEnumerator TelpoSkill(int level)
    {
        RaycastHit2D[] hit;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        Vector3 playerPos = transform.position;
        playerRigid.velocity = telpoVelocity * playerMovement.Direction;
        yield return telpoDuration;
        Vector3 currentPlayerPos = transform.position - playerPos;
        float angle = Mathf.Atan2(currentPlayerPos.y, currentPlayerPos.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        var telpoEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/TelpoEffect.prefab", playerPos, angleAxis);
        VisualEffect[] effects = telpoEffect.GetComponentsInChildren<VisualEffect>();
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].Play();
        }
        hit = Physics2D.BoxCastAll(playerPos, (Vector2)currentPlayerPos, angle, (Vector2)angleAxis.eulerAngles, 0, 1 << enemyLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            hit[i].transform.GetComponent<IHittable>().OnDamage(3, 0);
        }
        playerMovement.IsMove = true;
        player.IsInvincibility = false;
    }
    IEnumerator ArmSkill(int level)
    {
        Poolable[] arm = new Poolable[2];
        arm[0] = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/LeftArm.prefab", transform.position, Quaternion.Euler(0, 180, 0));
        arm[0].transform.localPosition += new Vector3(-3, 0, 0);
        arm[1] = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/RightArm.prefab", transform.position, Quaternion.identity);
        arm[1].transform.localPosition += new Vector3(3, 0, 0);
        yield return null;
    }

    IEnumerator Dash()
    {
        List<Poolable> dashObjLength = new List<Poolable>();
        float timer = 0;
        float alphaValue = 0;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        float distance = 0;
        int dashNum;
        Vector3 firstPosition = transform.position;
        Vector3 changePosition = transform.position;
        playerRigid.velocity = playerMovement.Direction * dashVelocity;
        SpriteRenderer currentPlayerSprite = GetComponent<SpriteRenderer>();
        dashCloneColor = dashSprite.color;
        dashCloneColor.a = 0;
        while (timer < dashTime)
        {
            timer += Time.fixedDeltaTime;
            alphaValue = timer / dashTime;
            distance = Vector2.SqrMagnitude(transform.position - changePosition);
            if (distance > instanceClonePerVelocity * instanceClonePerVelocity)
            {
                changePosition = transform.position;
                Poolable dashPool = Managers.Pool.Pop(dashObj, transform.position);
                cloneList.Add(dashPool);
                SpriteRenderer dashPoolSprite = dashPool.GetComponent<SpriteRenderer>();
                dashPoolSprite.sprite = currentPlayerSprite.sprite;
                dashPoolSprite.flipX = currentPlayerSprite.flipX;
                print(dashPoolSprite.sprite.name);
                dashCloneColor.a = alphaValue;
                dashPoolSprite.color = dashCloneColor;

            }
            yield return new WaitForFixedUpdate();
        }
        dashNum = Mathf.RoundToInt(Vector3.Magnitude(firstPosition - transform.position));
        Vector3 playerPoss = transform.position - firstPosition;
        float angle = Mathf.Atan2(playerPoss.y, playerPoss.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        for (int i = 0; i < dashNum; i++)
        {
            dashObjLength.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/DashSmoke.prefab", firstPosition + playerPoss.normalized * i, angleAxis));
            print(Quaternion.Euler(0, 0, angleAxis.eulerAngles.z) * transform.forward);
        }
        playerMovement.IsMove = true;
        player.IsInvincibility = false; 
        foreach (var c in cloneList)
        {
            Managers.Pool.Push(c);
        }
        cloneList.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(18, 10));
        Gizmos.DrawWireSphere(transform.position, 25f);
        Gizmos.DrawWireSphere(transform.position, 1 / 3.5f * 2 + 0.57f);
    }

  


    protected override void ForuthSkillUpdate(int level)
    {

    }

    protected override void FifthSkillUpdate(int level)
    {

    }
}
#endregion


