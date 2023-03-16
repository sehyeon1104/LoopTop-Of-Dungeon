using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySpawnManager : MonoSingleton<EnemySpawnManager>
{
    [Header("Ghost_Field_Enemy")]
    [SerializeField]
    private GameObject[] ghostNormalEnemyPrefabs;// = new GameObject[10];
    [SerializeField]
    private GameObject[] ghostEliteEnemyPrefabs;// = new GameObject[10];

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

    private Door door = null;

    public bool isNextWave { private set; get; } = false;
    private void Start()
    {
        SetMonsterPrefabInMonsterArray();
        door = FindObjectOfType<Door>();
        Managers.Pool.CreatePool(dangerMark, 10);
    }

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

    public void SetMonsterPrefabInMonsterArray()
    {
        // Debug.Log(Directory.GetFiles($"Assets/03.Prefabs/Enemy/Ghost").Length / 2);
        ghostNormalEnemyPrefabs = new GameObject[Directory.GetFiles($"Assets/03.Prefabs/Enemy/Ghost").Length / 2];
        ghostEliteEnemyPrefabs = new GameObject[Directory.GetFiles($"Assets/03.Prefabs/Enemy/Ghost").Length / 2];
        for (int i = 1; i <= Directory.GetFiles($"Assets/03.Prefabs/Enemy/Ghost").Length / 2; i++)
        {
            ghostNormalEnemyPrefabs[i - 1] = (Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/Ghost/G_Mob_0{i}.prefab"));
            ghostEliteEnemyPrefabs[i - 1] = (Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/Ghost/G_Mob_0{i}.prefab"));
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
                wave1NormalEnemyCount = 4;
                wave1EliteEnemyCount = 0;
                wave2NormalEnemyCount = 5;
                wave2EliteEnemyCount = 0;
                break;
            case 2:
                wave1NormalEnemyCount = 5;
                wave1EliteEnemyCount = 0;
                wave2NormalEnemyCount = 1;
                wave2EliteEnemyCount = 3;
                break;
            case 3:
                wave1NormalEnemyCount = 1;
                wave1EliteEnemyCount = 2;
                wave2NormalEnemyCount = 1;
                wave2EliteEnemyCount = 2;
                break;
            case 4:
                wave1NormalEnemyCount = 1;
                wave1EliteEnemyCount = 3;
                wave2NormalEnemyCount = 2;
                wave2EliteEnemyCount = 1;
                break;
        }

        Debug.Log("wave1NormalEnemyCount : " + wave1NormalEnemyCount);
        Debug.Log("wave1EliteEnemyCount : " + wave1EliteEnemyCount);
        Debug.Log("wave2NormalEnemyCount : " + wave2NormalEnemyCount);
        Debug.Log("wave2EliteEnemyCount : " + wave2EliteEnemyCount);
    }

    public IEnumerator SpawnEnemy(Transform[] enemySpawnPos)
    {
        // TODO : 적 소환시 이펙트 추가
        if (door.IsFirst)
        {
            door.IsFirst = false;
        }

        door.CloseDoors();
        
        int randPos = 0;
        isNextWave = false;
        // wave1
        Debug.Log(enemySpawnPos.Length);
        Debug.Log("wave 1");
        for(int i = 0; i < wave1NormalEnemyCount; ++i)
        {
            // 적 소환 위치를 담은 배열의 끝까지 범위지정
            randPos = Random.Range(1, enemySpawnPos.Length);
            // 자식(몹)이 있다면 다시 랜드
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            Debug.Log("SpawnEnemy");
            // 몹 소환
            var enemy = Instantiate(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            // 적 소환 위치를 부모로 설정
            enemy.transform.SetParent(enemySpawnPos[randPos]);
            // 현재 적들 리스트에 추가
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
            enemy.transform.SetParent(enemySpawnPos[randPos]);
            curEnemies.Add(enemy);
        }

        yield return new WaitUntil(() => curEnemies.Count <= 0);

        curEnemies.Clear();

        // wave2
        Debug.Log("wave 2");
        yield return new WaitForSeconds(spawnTime);
        isNextWave = true;

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
            enemy.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }
        for (int i = 0; i < wave2EliteEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Instantiate(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            enemy.transform.SetParent(enemySpawnPos[randPos]);
            enemy.SetActive(false);
            curEnemies.Add(enemy);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }
    }

    public IEnumerator ShowEnemySpawnPos(Transform spawnPos, GameObject enemy)
    {
        //var dangerMarkObj = Instantiate(dangerMark, enemySpawnPos.position, Quaternion.identity);

        var dangerMarkObj = Managers.Pool.Pop(dangerMark);
        dangerMarkObj.transform.position = spawnPos.position;
         
        yield return new WaitForSeconds(spawnTime);

        Managers.Pool.Push(dangerMarkObj);
        enemy.SetActive(true);

        yield return null;
    }
    public void RemoveEnemyInList(GameObject enemy)
    {
        curEnemies.Remove(enemy);
    }

}
