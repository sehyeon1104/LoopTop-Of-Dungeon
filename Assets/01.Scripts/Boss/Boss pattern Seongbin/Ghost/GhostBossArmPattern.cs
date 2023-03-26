using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GhostBossArmPattern : MonoBehaviour
{
    public Vector2 size1;

    private int time = 0;

    private float SizeX = 35.1f;

    private float SizeY = 23f;

    private float RandomSizeX = 0f;

    private float RandomSizeY = 0f;

    private Material[] setMat = new Material[3];

    WaitForSeconds Delay = new WaitForSeconds(0.4f);

    private void Start()
    {
        StartCoroutine(AAA());
    }

    public IEnumerator AAA()
    {
        Vector2 Owntransform = transform.position;
        while(time < 25)
        {
            RandomSizeX = 0f;
            RandomSizeY = 0f;

            RandomSizeX = Random.Range((SizeX / 2) * -1, SizeX / 2);
            RandomSizeY = Random.Range((SizeY / 2) * -1, SizeY / 2);

            Vector2 RandomPos = new Vector2(RandomSizeX, RandomSizeY);

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size1);
    }
}   
