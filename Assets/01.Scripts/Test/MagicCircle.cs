using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] private GameObject outCircle;
    [SerializeField] private GameObject inCircle;

    private void Awake()
    {
        StartCoroutine(StartAct());
    }

    private IEnumerator StartAct()
    {
        int count = 0;
        while (true)
        {
            outCircle.transform.Rotate(Vector3.forward * 0.25f);
            inCircle.transform.Rotate(-Vector3.forward * 0.5f);

            float outAngle = outCircle.transform.eulerAngles.z;
            float inAngle = inCircle.transform.eulerAngles.z;

            int subAngle = (int)(outAngle - inAngle);

            if (subAngle % 90 == 0)
            {
                count++;
                if(count == 15)
                {
                    yield return new WaitForSeconds(1f);
                    yield break;
                }
            }
            if (subAngle % 45 == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    float addedAngle = (i * 90) + 45;

                    float realOutAngle = outAngle + addedAngle;
                    float realInAngle = inAngle + addedAngle;

                    Vector3 dir = new Vector3(Mathf.Cos(realOutAngle * Mathf.Deg2Rad), Mathf.Sin(realOutAngle * Mathf.Deg2Rad)).normalized;
                    Managers.Pool.PoolManaging("Assets/10.Effects/final/Bullet_White.prefab", transform.position + dir * 3f, Quaternion.AngleAxis(realOutAngle, Vector3.forward));

                    for (int j = -1; j < 2; j += 2)
                    {
                        float finalAngle = j * Random.Range(15f, 30f) + realInAngle;
                        dir = new Vector3(Mathf.Cos(realInAngle * Mathf.Deg2Rad), Mathf.Sin(realInAngle * Mathf.Deg2Rad)).normalized;
                        Managers.Pool.PoolManaging("Assets/10.Effects/final/Bullet_Black.prefab", transform.position + dir * 2f, Quaternion.AngleAxis(finalAngle, Vector3.forward));

                    }
                }

            }    

            yield return new WaitForSeconds(0.01f);
        }
    }
}
