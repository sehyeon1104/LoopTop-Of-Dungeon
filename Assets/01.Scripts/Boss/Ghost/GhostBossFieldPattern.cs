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

    WaitForSeconds Delay = new WaitForSeconds(0.25f);

    WaitForSeconds waittime4s = new WaitForSeconds(4f);

    public Coroutine UltPattern { get; set; } = null;

    [SerializeField] BossAnim bossAnim;
    [SerializeField] AnimationClip absorbEnd;
    [SerializeField] AnimationClip finalHitted;
    public static bool isPushAllBubbles { get; set; } = false;

    //팔 솟아오르기 패턴

    private void Start()
    {
        UltPattern =  StartCoroutine(MakeBubble());
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

    //버블생성
    public IEnumerator MakeBubble()
    {
        while (true)
        {
            Vector2 Owntransform = new Vector2(14.5f, 7.5f);

            BubbleRandomSizeX = 0f;
            BubbleRandomSizeY = 0f;

            BubbleRandomSizeX = Random.Range((BubbleSizeX / 2) * -1, BubbleSizeX / 2);
            BubbleRandomSizeY = Random.Range((BubbleSizeY / 2) * -1, BubbleSizeY / 2);

            Vector2 RandomPos = new Vector2(BubbleRandomSizeX, BubbleRandomSizeY);

            Vector2 RealRandomPos = Owntransform + RandomPos;

            if(Random.Range(0,3) <= 1)
                Managers.Pool.PoolManaging("10.Effects/ghost/Bubble", RealRandomPos, Quaternion.identity);
            else
                Managers.Pool.PoolManaging("Assets/10.Effects/ghost/BubbleBlue.prefab", RealRandomPos, Quaternion.identity);

            yield return waittime4s;
        }
    }

    public IEnumerator GhostUltStart()
    {
        bool isCrashed = false;
        Poolable clone = Managers.Pool.PoolManaging("10.Effects/ghost/Absorb", bossAnim.transform.position, Quaternion.identity);
        Boss.Instance.Base.Shield = Boss.Instance.Base.MaxShield;

        bossAnim.overrideController[$"SkillFinal"] = Boss.Instance.bossPattern.Phase_Two_AnimArray[5];
        bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);

        checktime = 0f;
        while (checktime < 20f)
        {
            yield return new WaitForSeconds(0.5f);
            checktime += 0.5f;

            if (Boss.Instance.Base.Shield <= 0)
            {
                Managers.Pool.Push(clone.GetComponent<Poolable>());
                bossAnim.overrideController[$"SkillFinal"] = finalHitted;
                bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
                isCrashed = true;
                yield return new WaitForSeconds(10f);
                Boss.Instance.bossAnim.anim.SetBool("FinalEnd", true);
                yield break;
            }

            if (GhostBossUI.Instance.fillTime > 70f)
            {
                GameManager.Instance.Player.OnDamage(1, 0);
            }

            if(checktime % 1.5f == 0)
            {
                int random = Random.Range(0, 2);
                Poolable bubble = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Bubble&Bullet/BulletGroup.prefab", 
                GameManager.Instance.Player.transform.position, 
                Quaternion.Euler(Vector3.forward * random * 90));

                Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Bubble&Bullet/SafeBulletGroup.prefab", 
                bubble.transform.position, 
                Quaternion.Euler(Vector3.forward * ((random - 1) * 90)));
            }
        }

        if (!isCrashed)
        {
            bossAnim.overrideController[$"SkillFinal"] = absorbEnd;
            bossAnim.anim.SetTrigger(Boss.Instance._hashAttack);
            Managers.Pool.Push(clone.GetComponent<Poolable>());
            Poolable clone1 = Managers.Pool.PoolManaging("10.Effects/ghost/CircleSmoke", bossAnim.transform.position, Quaternion.identity);
            clone1.transform.localScale = new Vector3(10, 10, 0);
            yield return new WaitForSeconds(1f);
            GameManager.Instance.Player.OnDamage(100f, 0);
            Boss.Instance.bossAnim.anim.SetBool("FinalEnd", true);
        }
    }
}   
