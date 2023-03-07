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

    [SerializeField]
    private List<GameObject> curEnemies = new List<GameObject>();

    private int wave1NormalEnemyCount = 0;
    private int wave1EliteEnemyCount = 0;
    private int wave2NormalEnemyCount = 0;
    private int wave2EliteEnemyCount = 0;

    public void SetKindOfEnemy(Define.MapTypeFlag mapType)
    {
        // TODO : 현재 스테이지의 종류에 따라 적 종류 설정

        // 임시
        normalEnemyPrefabs = ghostNormalEnemyPrefabs;
        eliteEnemyPrefabs = ghostEliteEnemyPrefabs;
    }

    public void SetRandomEnemyCount()
    {
        // TODO : 가독성..

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
    }

    public IEnumerator SpawnEnemy()
    {
        // wave1
        for(int i = 0; i < wave1NormalEnemyCount; ++i)
        {
            // TODO : 소환 좌표 설정
            var enemy = Instantiate(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)]);
            curEnemies.Add(enemy);
        }
        for(int i = 0; i < wave1EliteEnemyCount; ++i)
        {
            // TODO : 소환 좌표 설정
            var enemy = Instantiate(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)]);
            curEnemies.Add(enemy);
        }

        yield return new WaitUntil(() => curEnemies.Count <= 0);

        curEnemies.Clear();

        // wave2
        for (int i = 0; i < wave2NormalEnemyCount; ++i)
        {
            // TODO : 소환 좌표 설정
            var enemy = Instantiate(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)]);
            curEnemies.Add(enemy);
        }
        for (int i = 0; i < wave2EliteEnemyCount; ++i)
        {
            // TODO : 소환 좌표 설정
            var enemy = Instantiate(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)]);
            curEnemies.Add(enemy);
        }
    }

    public void EraseEnemyInList(GameObject enemy)
    {
        curEnemies.Remove(enemy);
    }

}
