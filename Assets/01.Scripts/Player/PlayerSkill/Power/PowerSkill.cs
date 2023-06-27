using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class PowerSkill : PlayerSkillBase
{
    float choppingDmg = 20;
    float chopSize = 1;
    float meteorDmg = 10;
    float rushDmg = 13;
    float rockFallDuration = 2f;
    float rushVelocity = 5;
    int rushMax = 5;
    int rushMin = 1;
    float rushSize = 1;
    float rushDuration = 2f;
    bool isColumn;
    float ColumnDuration = 5f;
    WaitForSeconds waitcolliderPerTimr = new WaitForSeconds(0.1f);
    WaitForSeconds waitRockPush = new WaitForSeconds(2f);
    WaitForSeconds rockFallWait = new WaitForSeconds(0.5f);
    WaitForSeconds rushWait = new WaitForSeconds(0.5f);
    WaitForSeconds ColumnWait = new WaitForSeconds(5f);
    ParticleSystem attackPar;
    private void Awake()
    {
        Cashing();
    }
    protected override void Update()
    {
        base.Update();
    }
    private void Start()
    {
        attackPar = Managers.Resource.Instantiate("Assets/10.Effects/player/P_Attack.prefab", transform).GetComponent<ParticleSystem>();
    }
    protected override void Attack()
    {
        if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || player.playerBase.IsPDead)
            return;
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Player/Power/Attack.wav");

        CinemachineCameraShaking.Instance.CameraShake();
        playerAnim.SetTrigger("Attack");
        attackPar.transform.SetParent(transform);
        attackPar.transform.localPosition = playerSprite.flipX ? Vector3.right : Vector3.left;
        attackPar.Play();
        attackPar.transform.SetParent(null);

        RaycastHit2D[] enemys = Physics2D.BoxCastAll(attackPar.transform.position, Vector2.one, 0, attackPar.transform.localPosition, attackRange / 2, 1 << enemyLayer);
        for (int i = 0; i < enemys.Length; i++)
        {
            CinemachineCameraShaking.Instance.CameraShake(5, 0.3f);
            enemys[i].transform.GetComponent<IHittable>().OnDamage(GameManager.Instance.Player.playerBase.Damage, GameManager.Instance.Player.playerBase.CritChance);
        }
    }



    protected override void FirstSkill(int level)
    {
        if (level == 5)
            StartCoroutine(FiveBottomingOut());
        else
            StartCoroutine(BottomingOut());

    }

    protected override void SecondSkill(int level)
    {
        if (level == 5)
            StartCoroutine(FiveRush());
        else
           StartCoroutine(Rush(level));
    }

    protected override void ThirdSkill(int level)
    {

    }

    protected override void ForuthSkill(int level)
    {

    }

    protected override void FifthSkill(int level)
    {

    }

    protected override void UltimateSkill()
    {

    }
    protected override IEnumerator Dash()
    {
        return base.Dash();
    }


    protected override void ThirdSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 3, 0);
    }

    protected override void ForuthSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 4, 0);
    }

    protected override void FifthSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 5, 0);
    }
    #region 스킬 구현
    IEnumerator BottomingOut()
    {
        CinemachineCameraShaking.Instance.CameraShake(15, 0.2f);
        Poolable choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/BottomingOutEffect.prefab", transform.position, Quaternion.identity);
        choppingObj.GetComponent<Transform>().localScale = Vector2.one * chopSize;
        Collider2D[] enemys = Physics2D.OverlapCircleAll(choppingObj.transform.position, chopSize * 2, 1 << enemyLayer);
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GetComponent<IHittable>().OnDamage(choppingDmg, 0);
        }
        yield return null;
    }
    IEnumerator FiveBottomingOut()
    {

        float timer = 0;
        Vector2 CamSize = new Vector2(7, 10);
        CinemachineCameraShaking.Instance.CameraShake(40, 0.2f);
        Poolable choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/BottomingOutEffectFive.prefab", transform.position, Quaternion.identity);
        VisualEffect rockFall = choppingObj.transform.Find("RockFall").GetComponent<VisualEffect>();
        Vector3 rockFallPos = rockFall.transform.GetChild(0).position;
        choppingObj.GetComponent<Transform>().localScale = Vector2.one * chopSize;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(choppingObj.transform.position, chopSize * 2, 1 << enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<IHittable>().OnDamage(choppingDmg, 0);
        }
        yield return rockFallWait;
        while (timer < rockFallDuration)
        {
            enemies = Physics2D.OverlapBoxAll(rockFallPos, Vector2.one * 40, 0, 1 << enemyLayer);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<IHittable>().OnDamage(meteorDmg);
            }
            yield return waitcolliderPerTimr;
            timer += 0.1f;
        }
        rockFall.Stop();
        yield return waitRockPush;
        Managers.Pool.Push(choppingObj);
    }
    protected override void FirstSkillUpdate(int level)
    {
        if (level == 5)
        {
            chopSize = 2.5f;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 1, 1);
        }
        else
        {

            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 1, 0);
            chopSize = 1 + 0.25f * (level - 1);
        }
    }

    IEnumerator Rush(int level)
    {
        int rushNum = 1;
        int rushCheckNum = 0;
        int num =1;
        playerRigid.velocity = Vector2.zero;
        playerMovement.IsControl = false;
        player.IsInvincibility = true;
        if (level >= 2) 
        {
            if (GameManager.Instance.platForm == Define.PlatForm.PC)
            {
                float chargingTime = 0;
                KeyCode keyBoardButton = playerBase.PlayerSkillNum[0] == 2 ? KeyCode.U : KeyCode.I;
                while (Input.GetKey(keyBoardButton))
                {
                    chargingTime += Time.deltaTime * 2;
                    if(chargingTime > num)
                    {
                        Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/ChargingEffect.prefab", transform.position, Quaternion.identity);
                        num++;
                    }
                    yield return null;
                }
                Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/emitEffect.prefab",transform.position, Quaternion.identity);
                rushNum += Mathf.FloorToInt(chargingTime);
                rushNum = Mathf.Clamp(rushNum, rushMin, rushMax);
            }
        }
        playerRigid.velocity = playerMovement.Direction * rushVelocity;
        while (rushCheckNum < rushNum)
        {
            yield return rushWait;
            rushCheckNum++;
            CinemachineCameraShaking.Instance.CameraShake(5, 0.2f);
            Poolable choppingObj;
            if (level >= 3)
            {
                 choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEnforceEffect.prefab", transform.position, Quaternion.identity);
            }
            else
            {
                 choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEffect.prefab", transform.position, Quaternion.identity);
            }
            choppingObj.GetComponent<Transform>().localScale = Vector3.one * rushSize;
            Collider2D[] enemys = Physics2D.OverlapCircleAll(choppingObj.transform.position, 1.5f * rushSize, 1 << enemyLayer);
            
            for (int i = 0; i < enemys.Length; i++)
            {
                enemys[i].GetComponent<IHittable>().OnDamage(rushDmg, 0);
            }
            
        }
        playerMovement.IsControl = true;
        player.IsInvincibility = false;
        playerRigid.velocity = Vector3.zero;
        yield return null;
    }
    IEnumerator FiveRush()
    {
        while (player.transform.localScale.x < 2)
        {

            player.transform.localScale += Vector3.one * 0.1f;
            yield return null;
        }
    }
    protected override void SecondSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 2, 0);
        if (level == 4)
            rushSize *= 1.5f;

    }

    IEnumerator Jump()
    {
        yield return null;
    }
    IEnumerator Column()
    {
        isColumn = true;
        yield return ColumnWait;
        isColumn = false;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
