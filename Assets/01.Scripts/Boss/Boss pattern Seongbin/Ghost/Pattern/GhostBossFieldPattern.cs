using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

    private Material[] setMat = new Material[3];

    WaitForSeconds Delay = new WaitForSeconds(0.4f);

    WaitForSeconds Delay1 = new WaitForSeconds(4f);

    Coroutine UltPattern = null;

    //ÆÈ ¼Ú¾Æ¿À¸£±â ÆÐÅÏ

    private void Start()
    {
        StartCoroutine(GhostBossUltPattern());
    }
    public IEnumerator GhostBossArmPattern()
    {
        Vector2 Owntransform = transform.position;
        while(time < 25)
        {
            ArmRandomSizeX = 0f;
            ArmRandomSizeY = 0f;

            ArmRandomSizeX = Random.Range((ArmSizeX / 2) * -1, ArmSizeX / 2);
            ArmRandomSizeY = Random.Range((ArmSizeY / 2) * -1, ArmSizeY / 2);

            Vector2 RandomPos = new Vector2(ArmRandomSizeX, ArmRandomSizeY);

            Vector2 RealRandomPos = Owntransform + RandomPos;

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

    //±Ã±Ø±â ÆÐÅÏ
    public IEnumerator GhostBossUltPattern()
    {


        if(BossUI.fillTime < 40 || BossUI.fillTime > 60)
        {
            //GameManager.Instance.Player.OnDamage(12, gameObject, 0);
        }
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

            yield return Delay1;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size1);
    }
}   
