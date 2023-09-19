using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;
using Cinemachine;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using static Cinemachine.DocumentationSortingAttribute;
using Unity.Burst.Intrinsics;

public class PowerSkill : PlayerSkillBase
{
    [SerializeField]
    AnimationCurve animationSpeed;
    CinemachineVirtualCamera cineMachine;
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
    float jumpWidth = 2f;
    float jumpSpeed = 2f;
    float jumpAttackRange = 2f;
    float jumpAttackDmg = 20f;
    float tornadoSpeed = 9f;
    float tornadoDuration = 10f;
    bool isClick = false;
    bool isClicked = false;
    float catchTime = 4f;
  public bool isColumn;
    bool isColumning = true;
    float columnDetective = 5f;
    int columnLevel = 1;
    WaitForSeconds columnDuration = new WaitForSeconds(5f);
    public AnimationCurve jumpValue;
    WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
    WaitForSeconds waitcolliderPerTimr = new WaitForSeconds(0.1f);
    WaitForSeconds waitRockPush = new WaitForSeconds(2f);
    WaitForSeconds rockFallWait = new WaitForSeconds(0.5f);
    WaitForSeconds ColumnWait = new WaitForSeconds(5f);
    WaitForSeconds waitAttack = new WaitForSeconds(0.5f);
    ParticleSystem attackPar;
    WaitForSeconds columningWait = new WaitForSeconds(0.4f);
    WaitForSeconds columnExplosionWait = new WaitForSeconds(0.1f);
    //
    float jumpDownScaleMultiply = 1;
    float shockWaveTime = 4f;
    readonly int hashThrow = Animator.StringToHash("Throw");
    readonly int hashCatch = Animator.StringToHash("Catch");
    Animator bossArmAni = null;
    Poolable bossArm = null;
    Material jumpDownMat;
    Material material;
    AnimationCurve fiveJumpDownCurve;
    float power = 20;
    static int _waveDistanceFromCenter = Shader.PropertyToID("_waveDistanceFromCenter");
    List<Transform> enemiesTrans = new List<Transform>();
    bool[] boolGroup = new bool[50];
    private void Awake()
    {
    
        jumpDownMat = Managers.Resource.Load<Material>("Assets/10.Effects/player/Power/TrailMat.mat");
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        material = Managers.Resource.Load<Material>("Assets/12.ShaderGraph/Player/Shader Graphs_ShockWaveScreen.mat");
        attackPar = Managers.Resource.Instantiate("Assets/10.Effects/player/P_Attack.prefab", transform).GetComponent<ParticleSystem>();
        cineMachine = FindObjectOfType<CinemachineVirtualCamera>();
        Cashing();
        Init();
    }
    void Init()
    {
        for (int i = 0; i < 50; i++)
        {
            boolGroup[i] = true;
        }
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack()
    {
        
        if (isColumning && isColumn)
        {
            isColumning = false;
            StartCoroutine(ColumnAttack());
        }
        else
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
        StartCoroutine(FireBall(level));
    }

    protected override void FifthSkill(int level)
    {
        StartCoroutine(Column(level));
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
        int rushCheckNum = 0;
        float rushCheckTime = 0.7f;
        float num = 1;
        Collider2D[] playerCollider;
        playerRigid.velocity = Vector2.zero;
        playerMovement.IsControl = false;
        if (level >= 2)
        {
            if (GameManager.Instance.platForm == Define.PlatForm.PC)
            {
                KeyCode keyBoardButton = playerBase.PlayerSkillNum[0] == 2 ? KeySetting.keys[KeyAction.SKILL1] : KeySetting.keys[KeyAction.SKILL2];
                while (Input.GetKey(keyBoardButton))
                {
                    checkTime += Time.deltaTime;
                    if (checkTime > rushCheckTime)
                    {
                        Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/ChargingEffect.prefab", transform.position, Quaternion.identity);
                        num++;
                        checkTime = 0;
                    }
                    yield return null;
                }
                if (num > 1)
                {
                    Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/emitEffect.prefab", transform.position, Quaternion.identity);
                }
                num = Mathf.Clamp(num, rushMin, rushMax);
            }
        }

        player.IsInvincibility = true;
        float angle = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90, transform.forward);
        Poolable rushEffect = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/RushEffect 2.prefab", transform);
        rushEffect.transform.rotation = angleAxis;
        while (rushCheckNum < num)
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
        Vector3 scale = Vector3.zero;
        float trailWith;
        Collider2D[] enemies;
        Vector2[] dots = new Vector2[4];
        Vector2 currentPos = transform.position;
        VisualEffect tornadoEffect = null;
        float lerpValue = 0;
        Vector2 beforePos = Vector2.zero;
        Poolable emitJumpDown;
        float multiPlyValue = 1;
        float value = 0;
        float TornadoTimer = 0;
        float lensValue = cineMachine.m_Lens.OrthographicSize;
        jumpWidth = 2f;
        //µÚÀÏ¶© + ¾Õ¿¤¶© -
        Vector2 playerDirection = new Vector2(playerMovement.Direction.y, Mathf.Abs(playerMovement.Direction.x));
        if ((playerMovement.Direction.x < 1 && playerMovement.Direction.x > 0))
            playerDirection = new Vector2(playerMovement.Direction.y * -1, Mathf.Abs(playerMovement.Direction.x));
        float timer = 0;
        float timerA = 0;
        Vector3 currentPlayerScale = transform.localScale;
        KeyCode keyBoardButton = playerBase.PlayerSkillNum[0] == 3 ? KeySetting.keys[KeyAction.SKILL1] : KeySetting.keys[KeyAction.SKILL2];
        playerMovement.IsControl = false;
        playerRigid.velocity = Vector2.zero;
        Vector2 normailzedVec = playerMovement.Direction;
        if (playerMovement.Direction.x != 0 && playerMovement.Direction.y != 0)
            normailzedVec = new Vector2(playerMovement.Direction.x / MathF.Abs(playerMovement.Direction.x), playerMovement.Direction.y / MathF.Abs(playerMovement.Direction.y));
        //cineMachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(normailzedVec.x * 8f, normailzedVec.y * 6,-10);   
        Poolable charging = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/JumpDownCharging.prefab", transform.position, Quaternion.identity);
        Transform trans = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/Expectedrange.prefab", transform.position, Quaternion.identity).transform;
        trans.localScale = Vector3.one * jumpDownScaleMultiply;
        VisualEffect expectedRande = trans.GetComponent<VisualEffect>();
        while (Input.GetKey(keyBoardButton))
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 1)
            {
                timerA = 0;
                expectedRande.Reinit();
            }
            jumpWidth += 4 * Time.deltaTime;
            trans.position = currentPos + playerMovement.Direction * jumpWidth;
            cineMachine.m_Lens.OrthographicSize += Time.deltaTime * 2; // 3ÃÊ Â÷Â¡ÇßÀ» ¶§ 6ÀÌ ´Ã¾î³²
            if (timer > 3f)
                break;
            yield return null;
        }
        float subtractValue = cineMachine.m_Lens.OrthographicSize - lensValue;
        timer = 0;
        Managers.Pool.Push(charging);
        dots[0] = currentPos;
        dots[1] = currentPos + playerDirection * jumpHeight;
        dots[2] = dots[1] + playerMovement.Direction * jumpWidth;
        dots[3] = dots[0] + playerMovement.Direction * jumpWidth;

        ParticleSystem a = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/Flame_sides.prefab", transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        trailRenderer.material = jumpDownMat;
        trailRenderer.enabled = true;
        //trailRenderer.colorGradient = trailColor;
        if (playerMovement.Direction.x != 0 && playerMovement.Direction.y != 0)
            value = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) - 90 * Mathf.Deg2Rad;
        else
            value = playerMovement.Direction.y < 0 ? 180 * Mathf.Deg2Rad : 0;

        player.IsInvincibility = true;
        Vector3 direction = playerMovement.Direction;
        a.startRotation = value;
        while (lerpValue < 1)
        {
            multiPlyValue = jumpValue.Evaluate(lerpValue);
            lerpValue += Time.fixedDeltaTime * jumpSpeed * multiPlyValue;
            lerpValue = Mathf.Clamp(lerpValue, 0, 1);
            print(multiPlyValue);
            transform.localScale = currentPlayerScale * (Mathf.Sin(lerpValue * Mathf.PI) + 1);
            beforePos = Vector2.Lerp(Vector2.Lerp(Vector2.Lerp(dots[0], dots[1], lerpValue), Vector2.Lerp(dots[1], dots[2], lerpValue), lerpValue),
                                           Vector2.Lerp(Vector2.Lerp(dots[1], dots[2], lerpValue), Vector2.Lerp(dots[2], dots[3], lerpValue), lerpValue), lerpValue);
            playerRigid.MovePosition(beforePos);
            cineMachine.m_Lens.OrthographicSize -= (Time.fixedDeltaTime * jumpSpeed * multiPlyValue * subtractValue);
            yield return fixedWait;
        }

        cineMachine.m_Lens.OrthographicSize = 6;
        playerRigid.velocity = Vector2.zero;
        playerMovement.IsControl = true;
        trailRenderer.enabled = false;
        player.IsInvincibility = false;
        //cineMachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.back;
        enemies = Physics2D.OverlapCircleAll(transform.position, jumpAttackRange * jumpDownScaleMultiply, 1 << enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<IHittable>().OnDamage(jumpAttackDmg);
        }
        Poolable jumpDown = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/JumpDown.prefab", transform.position, Quaternion.identity);
        jumpDown.transform.localScale = Vector3.one * jumpDownScaleMultiply;
        CinemachineCameraShaking.Instance.CameraShake(30, 0.3f);
        if (level == 3)
        {
            emitJumpDown = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/JumpDownThree.prefab", transform);
            while (TornadoTimer < tornadoDuration)
            {
                scale = Vector3.one * TornadoTimer;
                if (scale.x > 1 || scale.y > 1)
                    scale = Vector3.one;
                emitJumpDown.transform.localScale = scale;
                emitJumpDown.transform.Rotate(Vector3.back * 180 * Time.fixedDeltaTime);
                TornadoTimer += Time.fixedDeltaTime;
                yield return fixedWait;
            }
        }
        else if (level == 4)
        {
            Collider2D[] attachenemies;
            emitJumpDown = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/JumpDownEmit.prefab", transform);
            while (TornadoTimer < tornadoDuration)
            {

                scale = Vector3.one * TornadoTimer;
                if (scale.x > 1 || scale.y > 1)
                    scale = Vector3.one;
                emitJumpDown.transform.localScale = scale;
                attachenemies = Physics2D.OverlapCircleAll(transform.position, 4.5f, 1 << enemyLayer);
                for (int i = 0; i < attachenemies.Length; i++)
                {
                    if (Vector2.SqrMagnitude(attachenemies[i].transform.position - transform.position) > 3.5 * 3.5)
                    {
                        attachenemies[i].GetComponent<IHittable>().OnDamage(playerBase.Attack);
                    }
                }
                TornadoTimer += Time.fixedDeltaTime;
                yield return fixedWait;
            }
        }
        yield return null;
    }
    IEnumerator FiveJumpDown()
    {

        float trailWith;
        Collider2D[] enemies;
        Vector2[] dots = new Vector2[4];
        Vector2 currentPos = transform.position;
        float lerpValue = 0;
        jumpWidth = 10;
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
        trailRenderer.material = jumpDownMat;
        ParticleSystem a = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/Flame_sides.prefab", transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        float value = 0;
        if (playerMovement.Direction.x != 0 && playerMovement.Direction.y != 0)
            value = Mathf.Atan2(playerMovement.Direction.y, playerMovement.Direction.x) - 90 * Mathf.Deg2Rad;
        else
            value = playerMovement.Direction.y < 0 ? 180 * Mathf.Deg2Rad : 0;

        a.startRotation = value;
        player.IsInvincibility = true;
        trailRenderer.startWidth = trailWidth * 2;
        while (lerpValue < 1)
        {
            multiPlyValue = jumpValue.Evaluate(lerpValue);
            lerpValue += Time.fixedDeltaTime * jumpSpeed * multiPlyValue;
            lerpValue = Mathf.Clamp(lerpValue, 0, 1);
            transform.localScale = currentPlayerScale * (Mathf.Sin(lerpValue * Mathf.PI) + 1) * 2;
            Vector2 FSegment = Vector2.Lerp(Vector2.Lerp(Vector2.Lerp(dots[0], dots[1], lerpValue), Vector2.Lerp(dots[1], dots[2], lerpValue), lerpValue),
                                            Vector2.Lerp(Vector2.Lerp(dots[1], dots[2], lerpValue), Vector2.Lerp(dots[2], dots[3], lerpValue), lerpValue), lerpValue);
            playerRigid.MovePosition(FSegment);
            yield return fixedWait;
        }
        player.IsInvincibility = false;
        Poolable shockWave = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/ShockWave.prefab", transform.position, Quaternion.identity);
        StartCoroutine(ShockWaveAction(shockWave, 0.1f, 1));
        enemies = Physics2D.OverlapCircleAll(transform.position, jumpAttackRange * jumpDownScaleMultiply, 1 << enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<IHittable>().OnDamage(jumpAttackDmg);
        }
        transform.localScale = currentPlayerScale;
        trailRenderer.enabled = false;
        CinemachineCameraShaking.Instance.CameraShake(30, 0.3f);
        playerMovement.IsMove = true;
        playerMovement.IsControl = true;
        Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FiveJumpDown.prefab", transform.position, Quaternion.identity);
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
    IEnumerator FireBall(int level)
    {
        List<Poolable> ballList = new List<Poolable>();
        List<Vector2> vectorList = new List<Vector2>(); 
        Collider2D[] attachEnemies;
        float timer = 0;
        Vector3 playerPos = transform.position;
        Quaternion angleAxis = MathClass.VectorToQuaternion(playerMovement.Direction, transform);
        Vector2 direction = playerMovement.Direction;
        if (level == 1)
        {
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, 0) * direction, Quaternion.identity));
            vectorList.Add((ballList[0].transform.position - playerPos).normalized );
        }
        else if (level == 2)
        {
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0,0,15) * direction, Quaternion.identity));
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, -15) * direction,Quaternion.identity));
            vectorList.Add((ballList[0].transform.position - playerPos).normalized);
            vectorList.Add((ballList[1].transform.position - playerPos).normalized);
        }
        else if (level == 3)
        {
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, -15) * direction, Quaternion.identity));
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, 0) * direction , Quaternion.identity));
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, 15) * direction, Quaternion.identity));
            vectorList.Add((ballList[0].transform.position - playerPos).normalized);
            vectorList.Add((ballList[1].transform.position - playerPos).normalized);
            vectorList.Add((ballList[2].transform.position - playerPos).normalized);
        }
        else if (level == 4)
        {
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, -15)  * direction/*+ (beamRot % 90 == 0 ? Vector3.up : new Vector3(-1, 1, 0))*/, Quaternion.identity));
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos+ Quaternion.Euler(0, 0, -30) * direction/*+ (beamRot % 90 == 0 ? Vector3.down : new Vector3(1, -1, 0))*/, Quaternion.identity));
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, 15) * direction/*+ (beamRot % 90 == 0 ? Vector3.right : new Vector3(1, 1, 0))*/, Quaternion.identity));
            ballList.Add(Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/FireBall.prefab", playerPos + Quaternion.Euler(0, 0, 30) * direction/* + (beamRot % 90 == 0 ? Vector3.left : new Vector3(-1, -1, 0))*/, Quaternion.identity));
            vectorList.Add((ballList[0].transform.position - playerPos).normalized);
            vectorList.Add((ballList[1].transform.position - playerPos).normalized);
            vectorList.Add((ballList[2].transform.position - playerPos).normalized);
            vectorList.Add((ballList[3].transform.position - playerPos).normalized);
        }
        while(timer < 10)
        {
            timer += Time.fixedDeltaTime;
            
            for (int i = 0; i < ballList.Count; i++)
            {
                ballList[i].transform.Translate(vectorList[i] * Time.fixedDeltaTime * 4 * animationSpeed.Evaluate(timer/10));  
                attachEnemies = Physics2D.OverlapCircleAll(ballList[i].transform.position, 1.5f, 1 << enemyLayer);
                for(int j=0; j<attachEnemies.Length; j++)
                {
                    attachEnemies[j].GetComponent<IHittable>().OnDamage(playerBase.Attack + level *2);
                }
                print(vectorList[i]);
            }
            yield return fixedWait;
        }
    }
    //IEnumerator Throw()
    //{ 
    //    isClicked = true;
    //    bossArmAni.SetTrigger(hashThrow);
    //    //Vector2 force = new Vector2(Random.Range(playerMovement.Direction.y * -1, playerMovement.Direction.y), Random.Range(playerMovement.Direction.x * -1, playerMovement.Direction.x)) * Random.Range(power - 10, power + 10);
    //    for (int i = 0; i < enemiesTrans.Count; i++)
    //    {
    //        enemiesTrans[i].GetComponent<Rigidbody2D>().AddForce(playerMovement.Direction * power/*Random.Range(power - 50, power + 50)*/, ForceMode2D.Impulse);
    //        print(enemiesTrans[i].transform.GetComponent<EnemyDefault>().IsControl);
    //    }
    //    yield return waitAttack;
    //    for (int i = 0; i < enemiesTrans.Count; i++)
    //    {
    //        print(enemiesTrans[i].transform.GetComponent<EnemyDefault>().IsControl);
    //        enemiesTrans[i].GetComponent<EnemyDefault>().IsControl = true;
    //        //enemiesTrans[i].transform.GetComponent<Collider2D>().isTrigger = boolGroup[i];
    //    }
    //    isClick = false;
    //    isClicked = false;
    //    enemiesTrans.Clear();
    //    Managers.Pool.Push(bossArm);
    //}
    IEnumerator Column(int level)
    {
        
       Poolable trail = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/ColumnTrail.prefab", transform);
        if(level >= 5)
        {
            trail.transform.localScale = Vector2.one * 1.5f;
        }
        isColumn = true;
        yield return columnDuration;
        isColumn = false;
    }
    IEnumerator ColumnAttack()
    {
        RaycastHit2D[] attachEnemies;
       Poolable Columns =null;
        if (Physics2D.OverlapCircle(transform.position, columnDetective, 1 << enemyLayer))
        {
            float minDistance;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, columnDetective, 1 << enemyLayer);
            List<Collider2D> enemiesList = enemies.ToList();
            int index = 0;
            minDistance = Vector2.Distance(transform.position, enemies[0].transform.position);
            if(columnLevel >=5)
            {
                Columns = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/column.prefab", enemiesList[index].transform.position, Quaternion.identity);
                Columns.transform.localScale = Vector2.one * 1.5f;
                attachEnemies = Physics2D.RaycastAll(enemiesList[index].transform.position, Vector2.up, 3, 1 << enemyLayer);
                for (int a = 0; a < attachEnemies.Length; a++)
                {
                    attachEnemies[a].transform.GetComponent<IHittable>().OnDamage(playerBase.Attack * 1.2f + columnLevel * 5, 0);
                }
                StartCoroutine(ColumnExplosion(Columns));
            }
            else
            {

            if (columnLevel > enemiesList.Count)
            {
                columnLevel = enemiesList.Count;
            }
            for (int i = 0; i < columnLevel; i++)
            {
                for (int j = 0; j < enemiesList.Count; j++)
                {
                    if (minDistance * minDistance > Vector2.SqrMagnitude(enemiesList[j].transform.position - transform.position))
                    {
                        minDistance = Mathf.Sqrt(Vector2.SqrMagnitude(enemiesList[j].transform.position - transform.position));
                        index = j;
                    }
                }

                Columns = Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/column.prefab", enemiesList[index].transform.position, Quaternion.identity);
                attachEnemies = Physics2D.RaycastAll(enemiesList[index].transform.position, Vector2.up,3,1<<enemyLayer);
                for (int a =0; a<attachEnemies.Length; a++)
                {
                    attachEnemies[a].transform.GetComponent<IHittable>().OnDamage(playerBase.Attack * 1.2f + columnLevel * 5, 0);
                }
                enemiesList.RemoveAt(index);
                minDistance = Vector2.Distance(transform.position, enemies[0].transform.position);
            }
            }
            yield return columningWait;
        isColumning = true;
        }
        else
        {
            isColumning = true;
            yield break;
        }
        
        yield return null;
    }
    IEnumerator ColumnExplosion(Poolable column)
    {
        yield return columnExplosionWait;
        Managers.Pool.PoolManaging("Assets/10.Effects/player/Power/ColumnExplo.prefab", column.transform.position, Quaternion.identity);
        Collider2D[] attachEnemies = Physics2D.OverlapCircleAll(column.transform.position, 3, 1 << enemyLayer);
        for (int i = 0; i < attachEnemies.Length; i++)
        {
            attachEnemies[i].GetComponent<IHittable>().OnDamage(playerBase.Attack +3);
        }
        yield return null;
    }
    protected override void FifthSkillUpdate(int level)
    {
        UIManager.Instance.SetSkillIcon(playerBase.PlayerTransformData, 0, 5, 0);
        columnLevel = level;
    }
    #endregion  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }


}
