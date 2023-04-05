using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class GhostSkill : PlayerSkillBase
{

    float cicleRange = 2f;
    float janpanDuration = 5f;
    PlayerSkillData skillData;
    private float hiilaDuration = 5;
    [SerializeField]
    private float attackRange = 1f;
    WaitForSeconds telpoDuration = new WaitForSeconds(0.1f);
    float telpoVelocity = 20f;
    Animator playerAnim;
    float dashtime = 0.1f;
    SpriteRenderer ghostDash;
    // Beam
    [SerializeField]
    private Vector3 beamDir;
    private float beamRot;
    private Vector3 beamPos;
    private Poolable beam;
    private Poolable subBeamLeft;
    private Poolable subBeamRight;
    [Header("솟아오르기 스킬")]
    float armSpeed = 10f;
    private Vector3 joystickDir;
    private void Awake()
    {
        Cashing();
        playerAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BeamPattern(5);
        }
    }
    public override void Attack()
    {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        playerAnim.SetTrigger("Attack");
        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, attackRange);
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].gameObject.CompareTag("Enemy") || enemys[i].gameObject.CompareTag("Boss"))
            {
                CinemachineCameraShaking.Instance.CameraShake();
                enemys[i].GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, gameObject, GameManager.Instance.Player.playerBase.CritChance);

            }
        }
    }
    public override void FirstSkill(int level)
    {
        StartCoroutine(JanpangSkill(level));
    }
    public override void SecondSkill(int level)
    {
        StartCoroutine(HillaSkill(level));
    }
    public override void ThirdSkill(int level)
    {
        BeamPattern(level);
    }
    public override void ForuthSkill(int level)
    {
        StartCoroutine(telpoSkill(level));
    }
    public override void FifthSkill(int level)
    {
        StartCoroutine(ArmSkill(level));
    }
    public override void UltimateSkill()
    {

    }
    public override void DashSkill()
    {
        StartCoroutine(Dash());
    }

    #region 스킬 구현
    IEnumerator JanpangSkill(int level)
    {
        Collider2D[] attachObjs = null;
        float timer = 0;
        float timerA = 0;
        GameObject smoke = Managers.Pool.PoolManaging("10.Effects/player/PlayerSmoke", transform.parent);

        while (timer < janpanDuration)
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, 1.7f);
                for (int i = 0; i < attachObjs.Length; i++)
                {
                    if (attachObjs[i].CompareTag("Enemy") || attachObjs[i].CompareTag("Boss"))
                    {
                        attachObjs[i].GetComponent<IHittable>().OnDamage(1, attachObjs[i].gameObject, playerBase.CritChance);
                    }
                }
                timerA = 0;
            }
            yield return null;
        }
        Managers.Pool.Push(smoke.GetComponent<Poolable>());
    }

    IEnumerator HillaSkill(int level)
    {
        float timer = 0;
        Poolable ghostMob = Managers.Pool.PoolManaging("03.Prefabs/Player/Ghost/GhostMob11", transform.position + new Vector3(Mathf.Cos(Random.Range(0, 360f) * Mathf.Deg2Rad), Mathf.Sin(Random.Range(0, 360f) * Mathf.Deg2Rad), 0) * cicleRange, quaternion.identity);
        while (true)
        {
            if (timer > hiilaDuration)
            {
                Managers.Pool.Push(ghostMob);
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void BeamPattern(int level)
    {

        beamRot = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(beamRot, Vector3.forward);
        print(beamPos);
        beam = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/PlayerBeam.prefab", transform.position, angleAxis);

        switch (level)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                subBeamLeft = Managers.Pool.PoolManaging("10.Effects/ghost/PlayerBeam", beamPos, beam.transform.rotation);
                subBeamLeft.transform.position = beamPos + new Vector3(-joystickDir.y, joystickDir.x);

                subBeamRight = Managers.Pool.PoolManaging("10.Effects/ghost/PlayerBeam", beamPos, beam.transform.rotation);
                subBeamRight.transform.position = beamPos + new Vector3(joystickDir.y, -joystickDir.x);
                break;
        }
    }
    IEnumerator telpoSkill(int level)
    {
        RaycastHit2D[] hit;
        playerMovement.IsMove = false;
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
        hit = Physics2D.BoxCastAll(playerPos, (Vector2)currentPlayerPos, angle, (Vector2)angleAxis.eulerAngles);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy") || hit[i].transform.CompareTag("Boss"))
            {
                hit[i].transform.GetComponent<IHittable>().OnDamage(3, gameObject, 0);
            }
        }
        playerMovement.IsMove = true;

    }
    IEnumerator ArmSkill(int level)
    {
        GameObject[] arm = new GameObject[2];
        arm[0] = Managers.Pool.PoolManaging("Assets/10.Effects/player/Ghost/LeftArm.prefab", transform);
        arm[1] = Managers.Pool.PoolManaging("htArm.prefabfects/player/Ghost/RightArm.prefab",transform);
        arm[0].transform.position += new Vector3(1, 3, 0);
        arm[1].transform.position += new Vector3(-1, 3, 0);
        while (true)
        {
            for (int i = 0; i < arm.Length; i++)
            {
                arm[i].transform.localPosition = new Vector3(0, Mathf.Lerp(arm[i].transform.localPosition.y, 2, Time.deltaTime* armSpeed), 0);
            }
            if (arm[0].transform.localPosition.y < 2.01f && arm[1].transform.localPosition.y <2.01f)
            {
                Managers.Pool.Push(arm[0].GetComponent<Poolable>());
                Managers.Pool.Push(arm[1].GetComponent<Poolable>());
                yield break;
            }
            yield return null;
        }

    }
    IEnumerator Dash()
    {
        float timer = 0;
        float timerA = 0;
        Color dashColor = new Color(1, 1, 1, 0);
        playerMovement.IsMove = false;
        Vector3 playerPos = transform.position;
        playerRigid.velocity = playerMovement.Direction * dashVelocity;
        while (timer < dashtime)
        {
            if (timerA >= dashtime / 5)
            {
                dashColor.a += 0.25f;
                playerSprite.color = dashColor;
                timerA = 0;
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
    }
    #endregion
    private void OnDrawGizmos()
    {

    }


}
