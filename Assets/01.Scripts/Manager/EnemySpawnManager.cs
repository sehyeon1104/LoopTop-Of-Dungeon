using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public class EnemySpawnManager : MonoSingleton<EnemySpawnManager>
{
    [Header("Ghost_Field_Enemy")]
    [SerializeField]
    private List<GameObject> ghostNormalEnemyPrefabs = new List<GameObject>();// = new GameObject[10];
    [SerializeField]
    private List<GameObject> ghostEliteEnemyPrefabs = new List<GameObject>();// = new GameObject[10];

    private GameObject[] normalEnemyPrefabs;
    private GameObject[] eliteEnemyPrefabs;

    [field: SerializeField]
    public List<Poolable> curEnemies { private set; get; } = new List<Poolable>();

    private int wave1NormalEnemyCount = 0;
    private int wave1EliteEnemyCount = 0;
    private int wave2NormalEnemyCount = 0;
    private int wave2EliteEnemyCount = 0;

    [SerializeField]
    private GameObject enemySpawnEffect = null;
    [SerializeField]
    private float spawnTime = 0.8f;

    private Door door = null;

    public bool isNextWave { private set; get; } = false;

    public AssetLabelReference assetLabel;
    private IList<IResourceLocation> _locations;

    private void Awake()
    {
        GetLocations();
    }

    private void Start()
    {
        door = FindObjectOfType<Door>();
        enemySpawnEffect = Managers.Resource.Load<GameObject>("Assets/10.Effects/ghost/SummonEffect.prefab");
        Managers.Pool.CreatePool(enemySpawnEffect, 10);
        SetEnemyInList();
    }

    #region Addressable

    // 이외 방법 : SO에 몹 배열을 선언, 해당 SO에 잡몹 넣고 어드레서블 적용. 그리고 사용할 때 SO에 있는 몹 불러오기

    public void GetLocations()
    {
        // 빌드타겟의 경로를 가져온다.
        // 경로이기 때문에 메모리에 에셋이 로드되진 않는다.
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _locations = handle.Result;
            };

    }

    public void SetEnemyInList()
    {
        for (int i = 0; i < _locations.Count; ++i)
        {
            // 맵 타입 플래그에 맞는 몹 몹 프리팹 불러옴
            ghostNormalEnemyPrefabs.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
            ghostEliteEnemyPrefabs.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
            Managers.Pool.CreatePool(ghostNormalEnemyPrefabs[i], 5);
        }

    }

    #endregion

    public void SetKindOfEnemy(Define.MapTypeFlag mapType)
    {
        // TODO : 현재 스테이지의 종류에 따라 적 종류 설정

        switch (mapType)
        {
            case Define.MapTypeFlag.Ghost:
                normalEnemyPrefabs = ghostNormalEnemyPrefabs.ToArray();
                eliteEnemyPrefabs = ghostEliteEnemyPrefabs.ToArray();
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

    public void SetMonsterPrefabInMonsterList()
    {

    }

    public void SetRandomEnemyCount()
    {
        // TODO : 가독성..

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

        // Debug.Log("wave1NormalEnemyCount : " + wave1NormalEnemyCount);
        // Debug.Log("wave1EliteEnemyCount : " + wave1EliteEnemyCount);
        // Debug.Log("wave2NormalEnemyCount : " + wave2NormalEnemyCount);
        // Debug.Log("wave2EliteEnemyCount : " + wave2EliteEnemyCount);
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
            // 적 소환 위치를 부모로 설정
            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            enemy.transform.position = enemySpawnPos[randPos].position;
            // 현재 적들 리스트에 추가
            curEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }
        for(int i = 0; i < wave1EliteEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Managers.Pool.Pop(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            enemy.transform.position = enemySpawnPos[randPos].position;
            curEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }

        yield return new WaitUntil(() => curEnemies.Count <= 0);


        curEnemies.Clear();

        yield return new WaitForSeconds(spawnTime);

        isNextWave = true;

        for (int i = 0; i < wave2NormalEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            enemy.transform.position = enemySpawnPos[randPos].position;
            curEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }
        for (int i = 0; i < wave2EliteEnemyCount; ++i)
        {
            randPos = Random.Range(1, enemySpawnPos.Length);
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            var enemy = Managers.Pool.Pop(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            enemy.transform.position = enemySpawnPos[randPos].position;
            enemy.gameObject.SetActive(false);
            curEnemies.Add(enemy);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }
    }

    public void StartNextWave()
    {
        isNextWave = true;
    }

    public IEnumerator ShowEnemySpawnPos(Transform spawnPos, Poolable enemy)
    {
        var effect = Managers.Pool.Pop(enemySpawnEffect);
        effect.transform.position = spawnPos.position;

        enemy.gameObject.SetActive(true);

        yield return new WaitForSeconds(spawnTime);

        Managers.Pool.Push(effect);

        yield return null;
    }
    public void RemoveEnemyInList(Poolable enemy)
    {
        if(enemy == null)
        {
            for(int i = 0; i < curEnemies.Count; ++i)
            {
                if(curEnemies[i] == null)
                {
                    curEnemies.RemoveAt(i);
                }
            }
            return;
        }
        curEnemies.Remove(enemy);
        Managers.Pool.Push(enemy);
    }

}
