using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject gasi;
    [SerializeField] private GameObject monster;

    [Space]

    [SerializeField] private GameObject bullet;

    private Transform player;
    private Coroutine attackCoroutine = null;

    private float hp = 0.0f;
    private float speed = 1.5f; 

    private void Awake()
    {
        player = Player.Instance.transform;
        hp = Boss.Instance.Base.Hp;

        StartCoroutine(RandomPattern());
    }
    private void Update()
    {
        MoveToPlayer();
    }
    private void LateUpdate()
    {
        if (Boss.Instance.isBDead)
        {
            StopAllCoroutines();
        }
    }

    private void MoveToPlayer()
    {
        if (attackCoroutine == null)
        {
            Vector2 dir = player.position - transform.position;
            transform.Translate(dir.normalized * Time.deltaTime);
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
                        attackCoroutine = StartCoroutine(Pattern_03(10));
                        break;
                }
            }
            yield return new WaitUntil(() => attackCoroutine == null);

            yield return new WaitForSeconds(5f);

        }
    }

    private IEnumerator Pattern_01(int attackCount)
    {
        for(int i = 0; i< attackCount; i++)
        {
            GameObject clone = Instantiate(warning, player.position, Quaternion.Euler(Vector3.zero));
            yield return new WaitForSeconds(1f);

            GameObject clone2 = Instantiate(gasi, clone.transform);
            
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
        if (hp >= Boss.Instance.Base.MaxHp * 0.4f) yield break;

        int finalCount = 0;
        List<GameObject> mobList = new List<GameObject>();

        for(int i = 0; i < mobCount; i++)
        {
            GameObject clone = Instantiate(monster, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.Euler(Vector3.zero));
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
        hp += finalCount * 10;
        mobList.Clear();

        yield return null;
        attackCoroutine = null;
    }
}
