using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
// using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

public class GhostBossFieldPattern : MonoBehaviour
{
    public Vector2 size1;

    private int time = 0;

    private int Randomtime = 0;

    private float ArmSizeX = 35.1f;

    private float ArmSizeY = 23f;

    private float BubbleSizeX = 35.1f;

    private float BubbleSizeY = 20.5f;

    private float ArmRandomSizeX = 0f;

    private float ArmRandomSizeY = 0f;

    private float BubbleRandomSizeX = 0f;

    private float BubbleRandomSizeY = 0f;

    float checktime = 0f;

    private Material[] setMat = new Material[3];

    WaitForSeconds Delay = new WaitForSeconds(0.4f);

    WaitForSeconds waittime4s = new WaitForSeconds(4f);
    
    WaitForSeconds waittime2dot5s = new WaitForSeconds(2.5f);

    WaitForSeconds waittime10s = new WaitForSeconds(10f);

    public Coroutine UltPattern { get; set; } = null;

    [SerializeField] BossAnim bossAnim;
    [SerializeField] AnimationClip absorbEnd;
    [SerializeField] AnimationClip finalHitted;
    public static bool isPushAllBubbles { get; set; } = false;

    //팔 솟아오르기 패턴

    private void Start()
    {
        UltPattern =  StartCoroutine(GhostBossUltPattern());
    }
    public IEnumerator GhostBossArmPattern()
    {
        Vector2 Owntransform = transform.position;
        time = 0;
        while(time < 25)
        {
            ArmRandomSizeX = 0f;
            ArmRandomSizeY = 0f;

            ArmRandomSizeX = Random.Range((ArmSizeX / 2) * -1, ArmSizeX / 2);
            ArmRandomSizeY = Random.Range((ArmSizeY / 2) * -1, ArmSizeY / 2);

            Vector2 RandomPos = new Vector2(ArmRandomSizeX, ArmRandomSizeY);

            Vector2 RealRandomPos = Owntransform + RandomPos;

            Managers.Pool.PoolManaging("10.Effects/ghost/BossArmRangeCircle", RealRandomPos, Quaternion.identity);
            Poolable clone = Managers.Pool.PoolManaging("10.Effects/ghost/GhostBossArmPatternAnim", RealRandomPos, Quaternion.identity);

            for(int i = 0; i < clone.GetComponentsInChildren<Renderer>().Length; i++)
            {
                setMat[i] = clone.GetComponentsInChildren<Renderer>()[i].material;
            }
            foreach (var mat in setMat)
            {
                mat.SetFloat("_StepValue", RealRandomPos.y);
            }

            time++;

            yield return Delay;
        }
    }

    //궁극기 패턴
    public IEnumerator GhostBossUltPattern()
    {
        yield return new WaitUntil(() => Boss.Instance.bossPattern.NowPhase == 2);
        while (true)
        {
            Randomtime = Random.Range(0, 5);

            Vector2 Owntransform = transform.position;

            BubbleRandomSizeX = 0f;
            BubbleRandomSizeY = 0f;

            BubbleRandomSizeX = Random.Range((BubbleSizeX / 2) * -1, BubbleSizeX / 2);
            BubbleRandomSizeY = Random.Range((BubbleSizeY / 2) * -1, BubbleSizeY / 2);

            Vector2 RandomPos = new Vector2(BubbleRandomSizeX, BubbleRandomSizeY);

            Vector2 RealRandomPos = Owntransform + RandomPos;

            Managers.Pool.PoolManaging("10.Effects/ghost/Bubble", RealRandomPos, Quaternion.identity);

            yield return waittime4s;
        }
    }

    public IEnumerator GhostUltStart()
    {
        Poolable clone = Managers.Pool.PoolManaging("10.Effects/ghost/Absorb", bossAnim.transform.position, Quaternion.identity);
        Boss.Instance.Base.Shield = Boss.Instance.Base.MaxShield;

        isPushAllBubbles = true; //필드 내 버블 다 사라짐
        StopCoroutine(UltPattern); // 버블 생성 중지

        //애니메이션 넣기 (흡수 시작 애니메이션 시전) 
        bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
        yield return waittime2dot5s; 

        if (BossUI.fillTime < 30f || BossUI.fillTime > 70f)
        {
            bossAnim.overrideController[$"SkillFinal"] = absorbEnd;
            bossAnim.anim.ResetTrigger(Boss.Instance._hashAttack);
            Poolable clone1 = Managers.Pool.PoolManaging("10.Effects/ghost/Smoke", bossAnim.transform.position, Quaternion.identity);
            clone1.transform.localScale = new Vector3(10, 10, 0);
            yield return new WaitForSeconds(1f);
            GameManager.Instance.Player.OnDamage(12f, 0);
        }

        checktime = 0f;
        //애니메이션 넣기 (흡수 대기 애니메이션 시전)
        while (checktime < 10f)
        {
            if(Boss.Instance.Base.Shield <= 0)
            {
                Managers.Pool.Push(clone.GetComponent<Poolable>());
                bossAnim.overrideController[$"SkillFinal"] = finalHitted;
                bossAnim.anim.ResetTrigger(Boss.Instance._hashAttack);
                yield return new WaitForSeconds(6f);
                yield break;
            }
            yield return null;
            checktime += Time.deltaTime;
        }

        yield return waittime10s;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size1);
    }
}   
