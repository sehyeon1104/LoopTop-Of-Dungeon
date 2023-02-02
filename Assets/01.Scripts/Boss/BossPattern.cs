using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject gasi;

    [Space]

    [SerializeField] private GameObject bullet;

    //private Transform player;
    private Coroutine attackCoroutine = null;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(RandomPattern());
    }

    private void LateUpdate()
    {
        if (Boss.Instance.isDead)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator RandomPattern()
    {
        while(true)
        {
            if (attackCoroutine == null)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        attackCoroutine = StartCoroutine(Pattern_01(Random.Range(3,5)));
                        break;
                    case 1:
                        attackCoroutine = StartCoroutine(Pattern_02(Random.Range(25, 30)));
                        break;
                    case 2:
                        attackCoroutine = StartCoroutine(Pattern_03(Random.Range(10, 15)));
                        break;
                }

                yield return new WaitForSeconds(2f);
            }
            yield return null;

        }
    }

    private IEnumerator Pattern_01(int attackCount)
    {
        GameObject clone = null;
        GameObject clone2 = null;
        for(int i = 0; i< attackCount; i++)
        {
            clone = Instantiate(warning, Player.Instance.transform.position, Quaternion.Euler(Vector3.zero));
            yield return new WaitForSeconds(1f);

            clone2 = Instantiate(gasi, clone.transform);
            
            clone2.transform.SetParent(null);
            Destroy(clone);

            yield return new WaitForSeconds(0.4f);
            Destroy(clone2);
        }

        yield return null;
        attackCoroutine = null;
    }

    private IEnumerator Pattern_02(int attackCount)
    {
        float angle = 360 / (attackCount * 0.89f);
        
        for(int i = 0; i < attackCount; i++)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(Vector3.forward * angle * i));
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
        attackCoroutine = null;
    }

    private IEnumerator Pattern_03(int mobCount)
    {
        Debug.Log(Boss.Instance.Base.Hp);
        int finalCount = 0;
        List<GameObject> mobList = new List<GameObject>();

        for(int i = 0; i < mobCount; i++)
        {
            GameObject clone = Instantiate(gasi, new Vector2(Random.Range(0, 10), Random.Range(0, 10)), Quaternion.Euler(Vector3.zero));
            mobList.Add(clone);
        }

        yield return new WaitForSeconds(10f);

        foreach(var mob in mobList)
        {
            if(mob != null)
            {
                finalCount++;
                Destroy(mob);
            }

        }
        Boss.Instance.Base.Hp += finalCount * 10;
        mobList.Clear();

        yield return null;
        Debug.Log(Boss.Instance.Base.Hp);
        attackCoroutine = null;
    }
}
