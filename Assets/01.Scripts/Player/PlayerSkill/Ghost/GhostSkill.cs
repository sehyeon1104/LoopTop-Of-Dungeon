using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using static Cinemachine.DocumentationSortingAttribute;
using Random = UnityEngine.Random;

public class GhostSkill : PlayerSkillBase
{
    [Header("���ǽ�ų")]
    WaitForSeconds janpanWait = new WaitForSeconds(0.3f);
    ParticleSystem janpnaPartical;
    float cicleRange = 2f;
    float janpanDuration = 5f;
    [Header("���� ��ų")]
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
    [Header("�ھƿ����� ��ų")]
    float armSpeed = 10f;
    private Vector3 joystickDir;
    [Header("�ñر�")]
    [SerializeField] GhostUltSignal ghostUlt;

    private void Awake()
    {
        Cashing();
        janpnaPartical = Managers.Resource.Load<GameObject>("Assets/10.Effects/player/PlayerSmoke.prefab").transform.Find("smoke").GetComponent<ParticleSystem>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BeamPattern(5);
        }
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
        BeamPattern(level);
    }
    protected override void ForuthSkill(int level)
    {
        StartCoroutine(telpoSkill(level));
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

    #region ��ų ����
    IEnumerator JanpangSkill(int level)
    {
        janpnaPartical.startSize = 2 * level +2;
        Collider2D[] attachObjs = null;
        float timer = 0;
        float timerA = 0;
        Poolable smoke = Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform.parent);
        yield return janpanWait;
        while (timer < janpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position,level/3.5f*2 + 0.57f, 1 << enemyLayer);
                for (int i = 0; i < attachObjs.Length; i++)
                {
                    attachObjs[i].GetComponent<IHittable>().OnDamage(1, 0);
                }
                timerA = 0;
            }
            yield return null;
        }
        Managers.Pool.Push(smoke.GetComponent<Poolable>());
    }
    IEnumerator Jangpan5Skill()
    {
        Collider2D[] attachObjs = null;
        Collider2D[] attachObj2 = null;
        List<Poolable> smoke = new List<Poolable>();
        float timer = 0;
        float timerA = 0;
        float timerB = 0;
        smoke.Add(Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform.position, Quaternion.identity));
        Poolable playerSmoke = Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform);
        yield return janpanWait; 
        while (timer < janpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            timerB += Time.deltaTime;
            if (timerA > 0.1f)
            {

                attachObj2 = Physics2D.OverlapCircleAll(transform.position, 1.7f, 1 << enemyLayer);
                for (int i = 0; i < smoke.Count; i++)
                {
                    attachObjs = Physics2D.OverlapCircleAll(smoke[i].transform.position, 1.7f, 1 << enemyLayer);
                    for (int j = 0; j < attachObjs.Length; j++)
                    {
                        attachObjs[j].GetComponent<IHittable>().OnDamage(1, 0);
                    }
                }
                for (int i = 0; i < attachObj2.Length; i++)
                {
                    attachObj2[i].GetComponent<IHittable>().OnDamage(1, 0);
                }
                timerA = 0;
            }
            if (timerB > 1f)
            {
                smoke.Add(Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform.position, Quaternion.identity));
                timerB = 0;
            }
            yield return null;
        }
        Managers.Pool.Push(playerSmoke);
        for (int i = 0; i < smoke.Count; i++)
        {
            Managers.Pool.Push(smoke[i]);
        }
    }
    IEnumerator HillaSkill(int level)
    {

        for (int i = 0; i < level; i++)
        {
            poolMob.Add(Managers.Pool.PoolManaging("03.Prefabs/Player/Ghost/GhostMob11", transform.position + new Vector3(Mathf.Cos(Random.Range(0, 360f) * Mathf.Deg2Rad), Mathf.Sin(Random.Range(0, 360f) * Mathf.Deg2Rad), 0) * cicleRange, quaternion.identity));
        }
        yield return hillaDuration;
        for (int i = 0; i < poolMob.Count; i++)
        {
            Managers.Pool.Push(poolMob[i]);
        }
        poolMob.Clear();
    }

    public void BeamPattern(int level)
    {
        beamRot = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(beamRot, Vector3.forward);
        beam = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/PlayerBeam.prefab", transform.position, angleAxis);

    }
    IEnumerator telpoSkill(int level)
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
        hit = Physics2D.BoxCastAll(playerPos, (Vector2)currentPlayerPos, angle, (Vector2)angleAxis.eulerAngles, Vector2.Distance(currentPlayerPos, transform.position), enemyLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            print("ss");
            if (hit[i].transform.CompareTag("Enemy") || hit[i].transform.CompareTag("Boss"))
            {
                hit[i].transform.GetComponent<IHittable>().OnDamage(3, 0);
            }
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


