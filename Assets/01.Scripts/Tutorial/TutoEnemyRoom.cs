using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnemyRoom : RoomBase
{
    private List<GameObject> enemyList = new List<GameObject>();

    private TutoEnemySpawn tutoEnemySpawn = null;

    public bool isPlayerEnter { get; private set; } = false;

    protected override void Awake()
    {
        tutoEnemySpawn = FindObjectOfType<TutoEnemySpawn>();

        doors = transform.Find("Door").gameObject;
        doors.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isClear)
            return;

        if (collision.CompareTag("Player") && !isPlayerEnter)
        {
            isPlayerEnter = true;
            tutoEnemySpawn.SpawnEnemy();
            ToggleDoors();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void AddEnemyInList(GameObject enemy)
    {
        enemyList.Add(enemy);
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    public void EnemyDead(GameObject enemy)
    {
        enemyList.Remove(enemy);
        IsClear();
    }

    protected override void IsClear()
    {
        if (isClear)
            return;

        if (enemyList.Count == 0)
        {
            isClear = true;
            ToggleDoors();
            TutorialManager.Instance.ClearTuto();
        }
    }

    public void ReStart()
    {
        if (!isPlayerEnter)
            return;

        isPlayerEnter = false;

        isClear = false;
        foreach(var enemy in enemyList)
        {
            Managers.Pool.Push(enemy.GetComponent<Poolable>());
        }
        enemyList.Clear();
        ToggleDoors();
    }
}
