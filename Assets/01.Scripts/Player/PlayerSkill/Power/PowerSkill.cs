using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PowerSkill : PlayerSkillBase
{
    public Gradient trailColor;
    TrailRenderer trailRenderer;
    float trailWidth = 5;
    float fireCheckDuration = 0.1f;
    float fireDuration = 0;
    float choppingDmg = 20;
    float chopSize = 1;
    float meteorDmg = 10;
    float rushAttackDmg = 13;
    float rushDmg = 3;
    float rockFallDuration = 2f;
    float rushVelocity = 15f;
    int rushMax = 5;
    int rushMin = 1;
    float rushDuration = 1f;
    float rushSize = 1;
    float rushWait = 0.2f;
    float jumpHeight = 5f;
    float jumpWidth = 10f;
    float jumpSpeed = 2f;
    float jumpAttackRange = 2f;
    float jumpAttackDmg = 20f;
    bool isClick = false;
    float catchTime = 4f;
    bool isColumn;
    float ColumnDuration = 5f;
    WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
    WaitForSeconds waitcolliderPerTimr = new WaitForSeconds(0.1f);
    WaitForSeconds waitRockPush = new WaitForSeconds(2f);
    WaitForSeconds rockFallWait = new WaitForSeconds(0.5f);
    WaitForSeconds ColumnWait = new WaitForSeconds(5f);
    WaitForSeconds waitAttack = new WaitForSeconds(0.5f);
    ParticleSystem attackPar;
    //
    float jumpDownScaleMultiply = 1;
    float shockWaveTime = 4f;
    Coroutine shockWaveCoroutine;
    IEnumerator catchSkill;
    readonly int hashThrow = Animator.StringToHash("Throw");
    readonly int hashCatch = Animator.StringToHash("Catch");
    Animator bossArmAni = null;
    Poolable bossArm = null;
    Material material;
    List<Transform> catchEnemies = new List<Transform>();
    float power = 20;
    static int _waveDistanceFromCenter = Shader.PropertyToID("_waveDistanceFromCenter");
   [SerializeField] GameObject columnEffect;
    private void Awake()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        material = Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Player/Shader Graphs_ShockWaveScreen.mat");
           Cashing();
    }
    protected override void Update()
    {
        base.Update();
    }
    private void Start()
    {
        catchSkill = CatchSkill();
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
        if (level == 5)
            StartCoroutine(FiveJumpDown());
        else
            StartCoroutine(Jumpdown(level));
    }
    protected override void ForuthSkill(int level)
    {
     
        if (isClick)
        {
            StopCoroutine(catchSkill);
            StartCoroutine(Throw());
        }
        else
            StartCoroutine(catchSkill);

    }

    protected override void FifthSkill(int level)
    {
        StartCoroutine(Column());
    }

    protected override void UltimateSkill()
    {

    }
    protected override IEnumerator Dash()
    {
        return base.Dash();
    }



    protected override void ForuthSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 4, 0);
    }

    protected override void FifthSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 5, 0);
    }
    #region ½ºÅ³ ±¸Çö
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

        player.IsInvincibility = true;
        float angle = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, transform.forward);
        Poolable rushEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEffect 1.prefab", transform);
        rushEffect.transform.rotation = angleAxis;
        while (rushCheckNum < rushNum)
        {
            playerRigid.velocity = playerMovement.Direction * rushVelocity;
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
        Dictionary<Poolable, float> tonados = new Dictionary<Poolable, float>();
        Collider2D[] playerCollider;
        float timer = 0;
        float checkTime = 0;
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

        while (num <= 3)
        {
            playerRigid.velocity = playerMovement.Direction * rushVelocity;
            if (timer >= 0.5f)
            {
                tonados.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RuehFiveEffect.prefab", transform.position, Quaternion.identity), 5);
                if (fireDuration == 0)
                    fireDuration = tonados.First().Key.GetComponent<VisualEffect>().GetFloat("Duration");
                KeyValuePair<Poolable, float> keyValue = tonados.First();
                StartCoroutine(FireTotenedo(keyValue.Key, keyValue.Value));
                tonados.Remove(keyValue.Key);
                timer = 0;
                checkTime = 0;
                num++;
            }
            if (checkTime < timer)
            {
                playerCollider = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 4, 0, 1 << enemyLayer);
                for (int i = 0; i < playerCollider.Length; i++)
                    playerCollider[i].GetComponent<IHittable>().OnDamage(rushDmg);
                checkTime += fireCheckDuration;
            }
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
            if (checkTime < timer)
            {
                radius += fireCheckDuration / fireDuration * 2;
                attachObjs = Physics2D.OverlapCircleAll(tonados.transform.position, radius, 1 << enemyLayer);
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
            rushSize = 1.5f;

    }

    IEnumerator Jumpdown(int level)
    {
        float trailWith;
        Collider2D[] enemies;
        Vector2[] dots = new Vector2[4];
        Vector2 currentPos = transform.position;
        float lerpValue = 0;
        //µÚÀÏ¶© + ¾Õ¿¤¶© -
        Vector2 playerDirection = new Vector2(playerMovement.Direction.y, Mathf.Abs(playerMovement.Direction.x));
        if ((playerMovement.Direction.x < 1 && playerMovement.Direction.x > 0))
            playerDirection = new Vector2(playerMovement.Direction.y * -1, Mathf.Abs(playerMovement.Direction.x));
        dots[0] = currentPos;
        dots[1] = currentPos + playerDirection * jumpHeight;
        dots[2] = dots[1] + playerMovement.Direction * jumpWidth;
        dots[3] = dots[0] + playerMovement.Direction * jumpWidth;
        playerMovement.IsMove = false;
        playerMovement.IsControl = false;
        Vector3 currentPlayerScale = transform.localScale;
        float multiPlyValue = 1;
        trailRenderer.startWidth = trailWidth;
        trailRenderer.colorGradient = trailColor;
        trailRenderer.enabled = true;
        trailRenderer.material = Managers.Resource.Load<Material>("Assets/10.Effects/player/Power/TrailMat.mat");
        ParticleSystem a = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/Flame_sides.prefab", transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        float value = 0;
        if (playerMovement.Direction.x != 0 && playerMovement.Direction.y != 0)
            value = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) - 90 * Mathf.Deg2Rad;
        else
            value = playerMovement.Direction.y < 0 ? 180 * Mathf.Deg2Rad : 0;

        a.startRotation = value;

        while (lerpValue < 1)
        {
            if (lerpValue > 0.5)
            {
                multiPlyValue = 1.3f;
            }
            if (lerpValue < 0.5)
            {
                multiPlyValue = 0.7f;
            }
            trailRenderer.widthMultiplier = multiPlyValue * trailWidth;
            lerpValue += Time.deltaTime * jumpSpeed * multiPlyValue;
            lerpValue = Mathf.Clamp(lerpValue, 0, 1);
            transform.localScale = currentPlayerScale * (Mathf.Sin(lerpValue * Mathf.PI) + 1);
            Vector2 FSegment = Vector2.Lerp(Vector2.Lerp(Vector2.Lerp(dots[0], dots[1], lerpValue), Vector2.Lerp(dots[1], dots[2], lerpValue), lerpValue),
                                            Vector2.Lerp(Vector2.Lerp(dots[1], dots[2], lerpValue), Vector2.Lerp(dots[2], dots[3], lerpValue), lerpValue), lerpValue);
            playerRigid.MovePosition(FSegment);
            yield return fixedWait;
        }
        trailRenderer.enabled = false;
        enemies = Physics2D.OverlapCircleAll(transform.position, jumpAttackRange * jumpDownScaleMultiply, 1 << enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<IHittable>().OnDamage(jumpAttackDmg);
        }
        Poolable jumpDown = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/JumpDown.prefab", transform.position, Quaternion.identity);
        jumpDown.transform.localScale = Vector3.one * jumpDownScaleMultiply;
        CinemachineCameraShaking.Instance.CameraShake(30, 0.3f);
        playerMovement.IsMove = true;
        playerMovement.IsControl = true;
        yield return null;
    }

    IEnumerator FiveJumpDown()
    {
        float trailWith;
        Collider2D[] enemies;
        Vector2[] dots = new Vector2[4];
        Vector2 currentPos = transform.position;
        float lerpValue = 0;
        //µÚÀÏ¶© + ¾Õ¿¤¶© -
        Vector2 playerDirection = new Vector2(playerMovement.Direction.y, Mathf.Abs(playerMovement.Direction.x));
        if ((playerMovement.Direction.x < 1 && playerMovement.Direction.x > 0))
            playerDirection = new Vector2(playerMovement.Direction.y * -1, Mathf.Abs(playerMovement.Direction.x));
        dots[0] = currentPos;
        dots[1] = currentPos + playerDirection * jumpHeight;
        dots[2] = dots[1] + playerMovement.Direction * jumpWidth;
        dots[3] = dots[0] + playerMovement.Direction * jumpWidth;
        playerMovement.IsMove = false;
        playerMovement.IsControl = false;
        Vector3 currentPlayerScale = transform.localScale;
        float multiPlyValue = 1;
        trailRenderer.enabled = true;
        trailRenderer.colorGradient = trailColor;
        trailRenderer.material = Managers.Resource.Load<Material>("Assets/10.Effects/player/Power/TrailMat2.mat");
        ParticleSystem a = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/Flame_sides.prefab", transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        float value = 0;
        if (playerMovement.Direction.x != 0 && playerMovement.Direction.y != 0)
            value = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) - 90 * Mathf.Deg2Rad;
        else
            value = playerMovement.Direction.y < 0 ? 180 * Mathf.Deg2Rad : 0;

        a.startRotation = value;

        trailRenderer.startWidth = trailWidth * 2;
        while (lerpValue < 1)
        {
            if (lerpValue > 0.5)
            {
                multiPlyValue = 1.3f;
            }
            if (lerpValue < 0.5)
            {
                multiPlyValue = 0.7f;
            }
            trailRenderer.widthMultiplier = multiPlyValue * trailWidth;
            lerpValue += Time.fixedDeltaTime * jumpSpeed * multiPlyValue;
            lerpValue = Mathf.Clamp(lerpValue, 0, 1);
            transform.localScale = currentPlayerScale * (Mathf.Sin(lerpValue * Mathf.PI) + 1) * 2;
            Vector2 FSegment = Vector2.Lerp(Vector2.Lerp(Vector2.Lerp(dots[0], dots[1], lerpValue), Vector2.Lerp(dots[1], dots[2], lerpValue), lerpValue),
                                            Vector2.Lerp(Vector2.Lerp(dots[1], dots[2], lerpValue), Vector2.Lerp(dots[2], dots[3], lerpValue), lerpValue), lerpValue);
            playerRigid.MovePosition(FSegment);
            yield return fixedWait;
        }
        Poolable shockWave = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/ShockWave.prefab", transform.position, Quaternion.identity);
        StartCoroutine(ShockWaveAction(shockWave, 0.1f, 1));
        transform.localScale = currentPlayerScale;
        trailRenderer.enabled = false;
        enemies = Physics2D.OverlapCircleAll(transform.position, jumpAttackRange, 1 << enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<IHittable>().OnDamage(jumpAttackDmg);
        }
        Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FiveJumpDown.prefab", transform.position, Quaternion.identity);
        CinemachineCameraShaking.Instance.CameraShake(30, 0.3f);
        playerMovement.IsMove = true;
        playerMovement.IsControl = true;
        yield return null;
        yield return null;
    }
    IEnumerator ShockWaveAction(Poolable shockWave, float startPos, float endPos)
    {
        material.SetFloat(_waveDistanceFromCenter, startPos);

        float lerpedAmount = 0f;

        float elpsedTime = 0f;
        while (lerpedAmount < endPos - 0.01)
        {
            elpsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(startPos, endPos, (elpsedTime / shockWaveTime));
            material.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            yield return null;
        }
        Managers.Pool.Push(shockWave);
    }
    protected override void ThirdSkillUpdate(int level)
    {
        if (level != 5)
        {
            if (level == 2)
                jumpDownScaleMultiply = 1.5f;
            UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 3, 0);
        }
        else        
           UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 3, 1);

    }
    IEnumerator CatchSkill()
    {
        KeyCode getKey = playerBase.PlayerSkillNum[0] == 4 ? KeyCode.U : KeyCode.I;
        float timer = 0;
        bossArm = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/BossArm.prefab", transform);
        bossArm.transform.position += (Vector3)playerMovement.Direction;
        Transform hand = bossArm.transform.Find("BossArm/Hand");
        bossArmAni = bossArm.transform.GetComponentInChildren<Animator>();
        Quaternion quaternion = MathClass.VectorToQuaternion(playerMovement.Direction, transform, 45);
        bossArm.transform.rotation = quaternion;
        RaycastHit2D[] enemies = Physics2D.RaycastAll(transform.position, playerMovement.Direction, 4, 1 << enemyLayer);
        bossArmAni.SetTrigger(hashCatch);

       
        if (enemies.Length == 0)
        {
            yield return waitAttack;
            Managers.Pool.Push(bossArm);
            yield break;
        }

        isClick = true;
        UIManager.Instance.SkillCoolCalculation(playerBase.PlayerTransformData.skill[4].skillDelay,PlayerSkill.Instance.skillIndex[0] == 4 ? 0 : 1);    
        for (int i = 0; i < enemies.Length; i++)
        {
            Transform enemy = enemies[i].transform;    
            catchEnemies.Add(enemy);
            enemy.GetComponent<EnemyDefault>().isControl = false;
        }
        while (timer < catchTime)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].transform.position = hand.position;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].transform.GetComponent<EnemyDefault>().isControl = true;
        isClick = false;
        
    }
    IEnumerator Throw()
    {
        bossArmAni.SetTrigger(hashThrow);
        Vector2 force = new Vector2(Random.Range(playerMovement.Direction.y * -1, playerMovement.Direction.y), Random.Range(playerMovement.Direction.x * -1, playerMovement.Direction.x)) * Random.Range(power - 10, power + 10);
        for (int i=0; i<catchEnemies.Count; i++)
        {
          
            catchEnemies[i].GetComponent<Rigidbody2D>().AddForce(playerMovement.Direction * Random.Range(power - 10, power + 10) + force, ForceMode2D.Impulse);
            catchEnemies[i].GetComponent<IHittable>().OnDamage(20, 0);
            catchEnemies[i].GetComponent<EnemyDefault>().isControl = true;
        }
        yield return waitAttack;
        isClick = false;
        Managers.Pool.Push(bossArm);
    }
    IEnumerator Column()
    {
        
        isColumn = true;
        float timer = 0;
        columnEffect.SetActive(true);
        while (timer < ColumnDuration)
        {

            timer += Time.deltaTime;
            yield return null;
        }
        columnEffect.SetActive(false);
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
