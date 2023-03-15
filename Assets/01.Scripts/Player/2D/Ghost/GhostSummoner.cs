using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSummoner : MonoBehaviour
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private float summonDuration = 10f; // 소환 지속시간
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float attackRange = 1.5f;
    [SerializeField]
    private float attackDelay = 1f;

    private float curDelay = 0f;

    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private GameObject nearestEnemy;

    private float dis = 0f;
    private float shortestDis = 10000f;
    private Vector3 dir;

    private bool isTargerting = false;

    private void OnEnable()
    {
        nearestEnemy = null;
        FindNearestEnemy();
    }

    private void Update()
    {
        summonDuration -= Time.deltaTime;
        curDelay += Time.deltaTime;
        if(summonDuration <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FindNearestEnemy()
    {
        nearestEnemy = null;

        if (isTargerting)
        {
            return;
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies == null)
        {
            return;
        }

        foreach(var enemy in enemies)
        {
            dis = Vector2.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position);
            if (dis < shortestDis)
            {
                nearestEnemy = enemy;
                shortestDis = dis;
            }
        }

        if(nearestEnemy == null)
        {
            return;
        }

        isTargerting = true;
        StartCoroutine(MoveToEnemy());
    }

    private IEnumerator MoveToEnemy()
    {
        if(nearestEnemy == null)
        {
            Debug.Log("NearestEnemy is null!");
            yield break;
        }

        StartCoroutine(IELocationTracking());

        while(nearestEnemy != null)
        {
            if (Vector2.Distance(transform.position, nearestEnemy.transform.position) > attackRange)
            {
                transform.Translate(dir * Time.deltaTime * moveSpeed);
            }
            else
            {
                Attack();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator IELocationTracking()    // 위치 추적
    {
        while (true)
        {
            if(nearestEnemy == null)
            {
                yield break;
            }

            dir = (nearestEnemy.transform.position - transform.position).normalized;

            yield return new WaitForEndOfFrame();
        }
    }

    private void Attack()
    {
        if(curDelay > attackDelay)
        {
            curDelay = 0f;
            Debug.Log("Attack");
        }
    }
}
