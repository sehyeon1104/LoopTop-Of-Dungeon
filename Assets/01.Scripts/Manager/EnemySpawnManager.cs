using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoSingleton<EnemySpawnManager>
{
    [Header("Ghost_Field_Enemy")]
    [SerializeField]
    private GameObject[] ghostNormalEnemyPrefabs;
    [SerializeField]
    private GameObject[] ghostEliteEnemyPrefabs;

    private GameObject[] normalEnemyPrefabs;
    private GameObject[] eliteEnemyPrefabs;

    [field: SerializeField]
    public List<GameObject> curEnemies { private set; get; } = new List<GameObject>();

    private int wave1NormalEnemyCount = 0;
    private int wave1EliteEnemyCount = 0;
    private int wave2NormalEnemyCount = 0;
    private int wave2EliteEnemyCount = 0;

    [SerializeField]
    private GameObject dangerMark = null;
    [SerializeField]
    private float spawnTime = 1.5f;

    public bool isNextWave { private set; get; } = false;

    public void SetKindOfEnemy(Define.MapTypeFlag mapType)
    {
        // TODO : 현재 스테이지의 종류에 따라 적 종류 설정

        switch (mapType)
        {
            case Define.MapTypeFlag.Ghost:
                normalEnemyPrefabs = ghostNormalEnemyPrefabs;
                eliteEnemyPrefabs = ghostEliteEnemyPrefabs;
                break;
            case Define.MapTypeFlag.LavaSlime:
                break;
            case Define.MapTypeFlag.Electricity:
                break;
            case Define.MapTypeFlag.Werewolf:
                break;
            case Define.MapTypeFlag.Lizard:
                break;
        }
    }

    public void SetRandomEnemyCount()
    {
        // TODO : 가독성..

        Debug.Log("SetRandomEnemyCount");

        int rand = Random.Range(1, 5);

        switch (rand)
        {
            case 1:
                wave1NormalEnemyCount = 7;
                wave1EliteEnemyCount = 0;
                wave2NormalEnemyCount = 8;
                wave2EliteEnemyCount = 0;
                break;
            case 2:
                wave1NormalEnemyCount = 8;
                wave1EliteEnemyCount = 0;
                wave2NormalEnemyCount = 1;
                wave2EliteEnemyCount = 5;
                break;
            case 3:
                wave1NormalEnemyCount = 1;
                wave1EliteEnemyCount = 5;
                wave2NormalEnemyCount = 1;
                wave2EliteEnemyCount = 5;
                break;
            case 4:
                wave1NormalEnemyCount = 1;
                wave1EliteEnemyCount = 5;
                wave2NormalEnemyCount = 2;
                wave2EliteEnemyCount = 2;
                break;
        }

        Debug.Log("wave1NormalEnemyCount : " + wave1NormalEnemyCount);
        Debug.Log("wave1EliteEnemyCount : " + wave1EliteEnemyCount);
        Debug.Log("wave2NormalEnemyCount : " + wave2NormalEnemyCount);
        Debug.Log("wave2EliteEnemyCount : " + wave2EliteEnemyCount);
    }

    public IEnumerator SpawnEnemy(Transform[] enemySpawnPos)
    {
        int randPos = 0;
        isNextWave = false;
        // wave1
        Debug.Log(enemySpawnPos.Length);
        Debug.Log("wave 1");
        for(int i = 0; i < wave1NormalEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Instantiate(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            enemy.transform.SetParent(enemySpawnPos[randPos]);
            curEnemies.Add(enemy);
        }
        for(int i = 0; i < wave1EliteEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Instantiate(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            curEnemies.Add(enemy);
        }

        yield return new WaitUntil(() => curEnemies.Count <= 0);

        curEnemies.Clear();

        // wave2
        Debug.Log("wave 2");
        isNextWave = true;
        StartCoroutine(ShowEnemySpawnPos(enemySpawnPos));
        yield return new WaitForSeconds(spawnTime);

        for (int i = 0; i < wave2NormalEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Instantiate(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            enemy.transform.SetParent(enemySpawnPos[randPos]);
            curEnemies.Add(enemy);
        }
        for (int i = 0; i < wave2EliteEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Instantiate(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            curEnemies.Add(enemy);
        }
    }

    public IEnumerator ShowEnemySpawnPos(Transform[] enemySpawnPos)
    {
        List<GameObject> dangerMarks = new List<GameObject>();

        foreach(var enemySpawnPosItem in enemySpawnPos)
        {
            var dangerMarkObj = Instantiate(dangerMark, enemySpawnPosItem.position, Quaternion.identity);
            dangerMarks.Add(dangerMarkObj);
        }

        yield return new WaitForSeconds(spawnTime);

        foreach(var dangerMarkItem in dangerMarks)
        {
            Destroy(dangerMarkItem);
        }

        yield break;
    }
    public void RemoveEnemyInList(GameObject enemy)
    {
        curEnemies.Remove(enemy);
    }

}
