using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

public class GhostSkill : PlayerSkillBase
{
    [Header("장판스킬")]
    WaitForSeconds jangpanWait2 = new WaitForSeconds(2f);
    WaitForSeconds janpanWait = new WaitForSeconds(0.3f);
    ParticleSystem janpnaPartical;
    float cicleRange = 2f;
    float janpanDuration = 5f;
    [Header("힐라 스킬")]
    List<Poolable> poolMob = new List<Poolable>();
    WaitForSeconds hillaDuration = new WaitForSeconds(10f);
    [SerializeField]
    private float attackRange = 1f;
    WaitForSeconds telpoDuration = new WaitForSeconds(0.1f);
    float telpoVelocity = 50f;
    Animator playerAnim;
    SpriteRenderer ghostDash;

    // Beam
    [SerializeField]
    private Vector3 beamDir;
    private float beamRot;
    private Poolable beam;
    private Poolable subBeamLeft;
    private Poolable subBeamRight;
    [Header("솟아오르기 스킬")]
    float armSpeed = 10f;
    private Vector3 joystickDir;
    [Header("궁극기")]
    [SerializeField] GhostUltSignal ghostUlt;


    private void Awake()
    {
        Cashing();
        janpnaPartical = Managers.Resource.Load<GameObject>("Assets/10.Effects/player/Ghost/PlayerSmoke.prefab").GetComponent<ParticleSystem>();
        playerAnim = GetComponent<Animator>();
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

    #region 스킬 구현
    IEnumerator JanpangSkill(int level)
    {

        Collider2D[] attachObjs = null;
        float timer = 0;
        float timerA = 0;
        Poolable smoke = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerSmoke.prefab", transform.parent);
        smoke.GetComponent<ParticleSystem>().startSize = 2 * level + 2;
        ParticleSystem smokeParticle = smoke.GetComponent<ParticleSystem>();
        yield return janpanWait;
        while (timer < janpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, level / 3.5f * 2 + 0.57f, 1 << enemyLayer);
                for (int i = 0; i < attachObjs.Length; i++)
                {
                    attachObjs[i].GetComponent<IHittable>().OnDamage(1 + level * 2, 0);
                }
                timerA = 0;
            }
            yield return null;
        }
        smokeParticle.loop = false;
        yield return jangpanWait2;
        Managers.Pool.Push(smoke);
        smokeParticle.GetComponent<ParticleSystem>().loop = true;
    }
    IEnumerator Jangpan5Skill()
    {
        Collider2D[] attachObjs = null;
        Collider2D[] attachObj2 = null;
        List<Poolable> smoke = new List<Poolable>();
        float timer = 0;
        float timerA = 0;
        float timerB = 1;
        Poolable playerSmoke = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerSmoke.prefab", transform);
        ParticleSystem smokeParticle = playerSmoke.GetComponent<ParticleSystem>();
        smokeParticle.startSize = 10;
        yield return janpanWait;
        while (timer < janpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            timerB += Time.deltaTime;
            if (timerA > 0.1f)
            {

                attachObj2 = Physics2D.OverlapCircleAll(transform.position, 2.9f, 1 << enemyLayer);
                for (int i = 0; i < smoke.Count; i++)
                {
                    attachObjs = Physics2D.OverlapCircleAll(smoke[i].transform.position, 2.9f, 1 << enemyLayer);
                    for (int j = 0; j < attachObjs.Length; j++)
                    {
                        attachObjs[j].GetComponent<IHittable>().OnDamage(11, 0);
                    }
                }
                for (int i = 0; i < attachObj2.Length; i++)
                {
                    attachObj2[i].GetComponent<IHittable>().OnDamage(11, 0);
                }
                timerA = 0;
            }
            if (timerB > 1f)
            {
                Poolable cloneSmoke = Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform.position, Quaternion.identity);
                smoke.Add(cloneSmoke);
                cloneSmoke.GetComponent<ParticleSystem>().startSize = 10;
                timerB = 0;
            }
            yield return null;
        }
        for (int i = 0; i < smoke.Count; i++)
        {
            smoke[i].GetComponent<ParticleSystem>().loop = false;
        }
        smokeParticle.loop = false;
        yield return jangpanWait2;
        Managers.Pool.Push(playerSmoke);
        for (int i = 0; i < smoke.Count; i++)
        {
            Managers.Pool.Push(smoke[i]);
            smoke[i].GetComponent<ParticleSystem>().loop = true;
        }
        smokeParticle.loop = true;

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
           poolMob.Add(Managers.Pool.PoolManaging("Assets/03.Prefabs/Player/Ghost/GhostEliteMob.prefab", transform.position + new Vector3(Mathf.Cos(Random.Range(0, 360f) * Mathf.Deg2Rad), Mathf.Sin(Random.Range(0, 360f) * Mathf.Deg2Rad), 0) * cicleRange , quaternion.identity));
        }
        yield return hillaDuration;
        for(int i =0; i<poolMob.Count; i++ )
        {
            Managers.Pool.Push(poolMob[i]);
        }
        poolMob.Clear();
    }

    IEnumerator Beam(int level)
    {
        float angle = Mathf.Atan2((transform.up.y - playerMovement.Direction.y), (transform.up.x - playerMovement.Direction.x)) * Mathf.Rad2Deg;
        beamRot = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis;
        print(playerMovement.Direction);
        switch (level)
        {
            case 1:
                angleAxis = Quaternion.AngleAxis(beamRot, transform.forward);
                Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position, angleAxis);
                yield break;
            case 2:
                angleAxis = Quaternion.AngleAxis(beamRot - 15, transform.forward);
                Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, -90) * playerMovement.Direction), angleAxis);
                angleAxis = Quaternion.AngleAxis(beamRot + 15, transform.forward);
                Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, 90) * playerMovement.Direction), angleAxis);
                yield break;
            case 3:
                angleAxis = Quaternion.AngleAxis(beamRot, transform.forward);
                Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position, angleAxis);
                angleAxis = Quaternion.AngleAxis(beamRot - 15, transform.forward);
                Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, -90) * playerMovement.Direction * 2f), angleAxis);
                angleAxis = Quaternion.AngleAxis(beamRot + 15, transform.forward);
                Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + (Quaternion.Euler(0, 0, 90) * playerMovement.Direction * 2f), angleAxis);
                yield break;
            case 4:
                Poolable[] beams = new Poolable[4];
                angleAxis = Quaternion.AngleAxis(beamRot, transform.forward);
                beams[0] = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + Vector3.up, Quaternion.Euler(0, 0, 90));
                angleAxis = Quaternion.AngleAxis(beamRot, transform.forward);
                beams[1] = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + Vector3.down, Quaternion.Euler(0, 0, 270));
                angleAxis = Quaternion.AngleAxis(beamRot - 15, transform.forward);
                beams[2] = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + Vector3.right, Quaternion.Euler(0, 0, 0));
                angleAxis = Quaternion.AngleAxis(beamRot + 15, transform.forward);
                beams[3] = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position + Vector3.left, Quaternion.Euler(0, 0, 180));
                PlayerBeam beam = beams[3].GetComponent<PlayerBeam>();
                
                while (beam.timerA < beam.beamDuration)
                {
                    if (beam.IsReady)
                    {
                        for (int i = 0; i < beams.Length; i++)
                        {
                           beams[i].transform.Rotate(0, 0, 360 / beam.beamDuration * Time.deltaTime);
                        }
                    }
 
                    yield return null;
                }
                yield break;
            case 5:
                angleAxis = Quaternion.AngleAxis(beamRot, transform.forward);
                 Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position, angleAxis);
                yield break;
            default:
                break;
        }
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
        float timer = 0;
        float timerA = 0;
        float flusA = 0;
        playerMovement.IsMove = false;
        player.IsInvincibility = true;
        Vector3 playerPos = transform.position;
        GameObject dashSprite = new GameObject();
        dashSprite.AddComponent<SpriteRenderer>();
        dashSprite.AddComponent<Poolable>();
        dashSprite.GetComponent<SpriteRenderer>().sprite = playerSprite.sprite;
        dashSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Skill";

        dashSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        playerRigid.velocity = playerMovement.Direction * dashVelocity;
        //SpriteRenderer data = playerSprite;

        //Component tempData = dashSprite.AddComponent(data.GetType());

        //foreach (FieldInfo f in data.GetType().GetFields())
        //{
        //    f.SetValue(tempData, f.GetValue(data));

        //}

        while (timer < dashTime)
        {
            if (timerA > 0.02f)
            {
                flusA += 0.2f;
                dashSprite.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;
                Poolable clone = Managers.Pool.Pop(dashSprite, transform.position);
                clone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, flusA);
                cloneList.Add(clone);
            }
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            yield return null;
        }
        Vector3 playerPoss = transform.position - playerPos;
        float angle = Mathf.Atan2(playerPoss.y, playerPoss.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/DashSmoke.prefab", playerPos, angleAxis);
        playerMovement.IsMove = true;
        player.IsInvincibility = false;
        foreach (var c in cloneList)
        {
            Managers.Pool.Push(c);
        }
        cloneList.Clear();
        dashSprite = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 25f);
        Gizmos.DrawWireSphere(transform.position, 1 / 3.5f * 2 + 0.57f);
    }
}
#endregion


