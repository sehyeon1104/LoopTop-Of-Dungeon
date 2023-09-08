using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnemySpawn : MonoBehaviour
{
    [SerializeField] 
    private Transform[] enemySpawnPos = null;
    private GameObject enemyPrefab = null;
    private int mobSpawnCount;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        enemyPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/TutoMob.prefab");

        mobSpawnCount = enemySpawnPos.Length;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < mobSpawnCount; ++i)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPos[i]);
            TutorialManager.Instance.tutoEnemyRoom.AddEnemyInList(enemy);
        }
    }
}
