using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostSkill : PlayerSkillBase
{
    float cicleRange = 2f;
    float janpanDuration = 5f;
    PlayerSkillData skillData;
    GameObject ghostMob;
    private float hiilaDuration = 5;
    [SerializeField]
    private float beamDistanceFromPlayer = 1f;
    private Vector3 beamDir;
    private float beamRot;
    private Vector3 beamPos;
    [SerializeField]
    private float attackRange = 1f;
    Animator playerAnim;
    WaitForSeconds dashTime = new WaitForSeconds(0.1f);
    private void Awake()
    {
        Cashing();
        playerAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BeamPattern(1);
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

    }
    public override void ForuthSkill(int level)
    {

    }
    public override void FifthSkill(int level)
    {

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
                    if (attachObjs[i].CompareTag("Enemy")||attachObjs[i].CompareTag("Boss"))
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
       Poolable ghostMob =  Managers.Pool.PoolManaging("03.Prefabs/Player/Ghost/GhostMob11", transform.position +  new Vector3(Mathf.Cos(Random.Range(0,360f)*Mathf.Deg2Rad), Mathf.Sin(Random.Range(0,360f) * Mathf.Deg2Rad),0) * cicleRange, quaternion.identity);
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
        beamPos = (Vector2)transform.position + new Vector2(playerMovement.joystick.Horizontal, playerMovement.joystick.Vertical).normalized;
        Poolable beam = Managers.Pool.PoolManaging("10.Effects/ghost/PlayerBeam", beamPos, Quaternion.identity);
        SetBeamRotation(beamPos);
        beam.transform.Rotate(new Vector3(0, 0, beamRot));
        //Managers.Pool.PoolManaging("Assets/10.Effects/ghost/PlayerBeam.prefab", (Vector2)transform.position + new Vector2(playerMovement.joystick.Horizontal, playerMovement.joystick.Vertical).normalized * beamDistanceFromPlayer, Quaternion.identity);
    }

    public void SetBeamRotation(Vector3 pos)
    {
        beamDir = pos - transform.position;
        beamRot = Mathf.Atan2(beamDir.y, beamDir.x) * Mathf.Rad2Deg;
    }
    public IEnumerator Dash()
    {
         playerMovement.IsMove = false;
         playerRigid.velocity = playerMovement.Direction * dashVelocity;
         yield return dashTime;
         playerMovement.IsMove = true;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

   
}
