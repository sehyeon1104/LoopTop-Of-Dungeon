using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class PowerSkill : PlayerSkillBase
{
    float fireCheckDuration = 0.1f;
    float fireDuration = 0;
    float choppingDmg = 20;
    float chopSize = 1;
    float meteorDmg = 10;
    float rushAttackDmg = 13;
    float rushDmg = 1;
    float rockFallDuration = 2f;
    float rushVelocity = 15f;
    int rushMax = 5;
    int rushMin = 1;
    float rushSize = 1;
    float rushDuration = 1f;
    bool isColumn;
    float rushWait = 0.2f;
    float ColumnDuration = 5f;
    WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
    WaitForSeconds waitcolliderPerTimr = new WaitForSeconds(0.1f);
    WaitForSeconds waitRockPush = new WaitForSeconds(2f);
    WaitForSeconds rockFallWait = new WaitForSeconds(0.5f);
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
        playerAnim.SetTrigger("Attack");
        playerRigid.velocity = Vector2.zero;
        playerMovement.IsControl = false;
        playerMovement.IsMove = false;
        CinemachineCameraShaking.Instance.CameraShake(15, 0.2f);
        Poolable choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/BottomingOutEffect.prefab", transform.position, Quaternion.identity);
        choppingObj.GetComponent<Transform>().localScale = Vector2.one * chopSize;
        Collider2D[] enemys = Physics2D.OverlapCircleAll(choppingObj.transform.position, chopSize * 2, 1 << enemyLayer);
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GetComponent<IHittable>().OnDamage(choppingDmg, 0);
        }
        yield return new WaitUntil(() => playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        playerMovement.IsControl = true;
        playerMovement.IsMove = true;
    }
    IEnumerator FiveBottomingOut()
    {
        float timer = 0;
        Vector2 CamSize = new Vector2(7, 10);
        playerAnim.SetTrigger("Attack");
        playerRigid.velocity = Vector2.zero;
        playerMovement.IsMove = false;
        playerMovement.IsControl = false;
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
        yield return new WaitUntil(() => playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        playerMovement.IsControl = true;
        playerMovement.IsMove = true;
        yield return rockFallWait;
        while (timer < rockFallDuration)
        {
            enemies = Physics2D.OverlapBoxAll(rockFallPos, Vector2.one * 30, 0, 1 << enemyLayer);
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
        float checkTime = 0;
        float timer = 0;
        int rushNum = 1;
        int rushCheckNum = 0;
        int num = 1;
        Collider2D[] playerCollider;
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
                    if (chargingTime > num)
                    {
                        Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/ChargingEffect.prefab", transform.position, Quaternion.identity);
                        num++;
                    }
                    yield return null;
                }
                if (num > 1)
                {

                    Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/emitEffect.prefab", transform.position, Quaternion.identity);
                }
                rushNum += Mathf.FloorToInt(chargingTime);
                rushNum = Mathf.Clamp(rushNum, rushMin, rushMax);
            }
        }
        float angle = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, transform.forward);
        Poolable rushEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEffect 1.prefab", transform);
        rushEffect.transform.rotation = angleAxis;
        playerRigid.velocity = playerMovement.Direction * rushVelocity;
        while (rushCheckNum < rushNum)
        {
            if (timer > rushWait)
            {

                rushCheckNum++;
                playerAnim.SetTrigger("Attack");
                Poolable choppingObj;
                if (level >= 3)
                {
                    choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEnforceEffect.prefab", transform.position, Quaternion.identity);
                    CinemachineCameraShaking.Instance.CameraShake(20, 0.1f);
                }
                else
                {
                    choppingObj = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEffect.prefab", transform.position, Quaternion.identity);
                    CinemachineCameraShaking.Instance.CameraShake(10, 0.1f);
                }
                choppingObj.GetComponent<Transform>().localScale = Vector3.one * rushSize;

                Collider2D[] enemys = Physics2D.OverlapCircleAll(choppingObj.transform.position, 1.5f * rushSize, 1 << enemyLayer);
                for (int i = 0; i < enemys.Length; i++)
                    enemys[i].GetComponent<IHittable>().OnDamage(rushAttackDmg);
                timer = 0;
            }
            if (timer > checkTime)
            {
                checkTime += 0.1f;
                playerCollider = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 2, 0, 1 << enemyLayer);
                for (int i = 0; i < playerCollider.Length; i++)
                    playerCollider[i].GetComponent<IHittable>().OnDamage(rushDmg);
            }
            timer += Time.fixedDeltaTime;
            yield return fixedWait;
        }
        Managers.Pool.Push(rushEffect);
        playerMovement.IsControl = true;
        player.IsInvincibility = false;
        playerRigid.velocity = Vector3.zero;
        yield return null;
    }
    IEnumerator FiveRush()
    {
        int num = 0;
        Dictionary<Poolable,float> tonados = new Dictionary<Poolable,float>();  
        Collider2D[] playerCollider;
        float timer = 0;
        playerRigid.velocity = Vector2.zero;
        playerMovement.IsControl = false;
        player.IsInvincibility = true;
        float angle = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion rotationValue = Quaternion.AngleAxis(angle - 90, transform.forward);
        while (player.transform.localScale.x <= 2)
        {
            player.transform.localScale += Vector3.one * Time.deltaTime * 2;
            yield return null;
        }
        Poolable rushEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEffect 1.prefab", transform);
        rushEffect.transform.localScale *= 2;
        rushEffect.transform.rotation = rotationValue;
        playerRigid.velocity = playerMovement.Direction * rushVelocity;

        while (num <= 3)
        {

            if (timer >= 0.5f)
            {
                tonados.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RuehFiveEffect.prefab", transform.position, Quaternion.identity),5);
            if (fireDuration == 0)
                fireDuration = tonados.First().Key.GetComponent<VisualEffect>().GetFloat("Duration");
                KeyValuePair<Poolable,float> keyValue = tonados.First();
                StartCoroutine(FireTotenedo(keyValue.Key, keyValue.Value));
                tonados.Remove(keyValue.Key);
                timer = 0;
                num++;
            }

            playerCollider = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 4, 0, 1 << enemyLayer);
                for (int i = 0; i < playerCollider.Length; i++)
                    playerCollider[i].GetComponent<IHittable>().OnDamage(rushDmg);
           
            timer += Time.fixedDeltaTime;
            yield return fixedWait;
        }
        rushEffect.gameObject.SetActive(false);
        playerRigid.velocity = Vector2.zero;
        playerMovement.IsControl = true;
        player.IsInvincibility = false;
        while (player.transform.localScale.x >= 1)
        {
            player.transform.localScale -= Vector3.one * Time.deltaTime * 2;
            yield return null;
        }
        Managers.Pool.Push(rushEffect);

    }
    IEnumerator FireTotenedo(Poolable tonados, float radius)
    {
        float checkTime = 0;
        float timer = 0;
        Collider2D[] attachObjs;
        while (timer < fireDuration)
        {
            if(checkTime < timer)
            {   
                radius += fireCheckDuration / fireDuration * 2;
                attachObjs = Physics2D.OverlapCircleAll(tonados.transform.position, radius , 1 << enemyLayer);
                    for (int j = 0; j < attachObjs.Length; j++)
                        attachObjs[j].GetComponent<IHittable>().OnDamage(rushAttackDmg);
                checkTime = timer + fireCheckDuration;
            }
            timer += Time.deltaTime;
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
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
