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
    private GameObject dangerMark = null;
    [SerializeField]
    private float spawnTime = 1.5f;

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
        Managers.Pool.CreatePool(dangerMark, 10);
        SetEnemyInList();
    }

    #region Addressable

    // SO�� �� �迭�� ����, �ش� SO�� ��� �ְ� ��巹���� ����. �׸��� ����� �� SO�� �ִ� �� �ҷ�����

    public void GetLocations()
    {
        // ����Ÿ���� ��θ� �����´�.
        // ����̱� ������ �޸𸮿� ������ �ε���� �ʴ´�.
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _locations = handle.Result;
            };

    }

    public void SetEnemyInList()
    {
        Debug.Log(_locations.Count);

        //if(_locations.Count == 0)
        //{
        //    Debug.Log("_locations.Count == 0");
        //}

        for (int i = 0; i < _locations.Count; ++i)
        {
            // �� Ÿ�� �÷��׿� �´� �� �� ������ �ҷ���
            ghostNormalEnemyPrefabs.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
            ghostEliteEnemyPrefabs.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
        }
    }

    #endregion

    public void SetKindOfEnemy(Define.MapTypeFlag mapType)
    {
        // TODO : ���� ���������� ������ ���� �� ���� ����

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
        // TODO : ������..

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

        // Debug.Log("wave1NormalEnemyCount : " + wave1NormalEnemyCount);
        // Debug.Log("wave1EliteEnemyCount : " + wave1EliteEnemyCount);
        // Debug.Log("wave2NormalEnemyCount : " + wave2NormalEnemyCount);
        // Debug.Log("wave2EliteEnemyCount : " + wave2EliteEnemyCount);
    }

    public IEnumerator SpawnEnemy(Transform[] enemySpawnPos)
    {
        // TODO : �� ��ȯ�� ����Ʈ �߰�
        if (door.IsFirst)
        {
            door.IsFirst = false;
        }

        // ������, �׽�Ʈ �ʿ�
        //if(enemySpawnPos.Length == 0)
        //{
        //    GameManager.Instance.Player.playerBase.FragmentAmount = 404;
        //}

        door.CloseDoors();
        
        int randPos = 0;
        isNextWave = false;
        // wave1
        // Debug.Log(enemySpawnPos.Length);
        // Debug.Log("wave 1");
        // GameManager.Instance.Player.playerBase.FragmentAmount = 404;
        for(int i = 0; i < wave1NormalEnemyCount; ++i)
        {

            // �� ��ȯ ��ġ�� ���� �迭�� ������ ��������
            randPos = Random.Range(1, enemySpawnPos.Length);
            // �ڽ�(��)�� �ִٸ� �ٽ� ����
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            Debug.Log("SpawnEnemy");
            // �� ��ȯ
            // �� ��ȯ ��ġ�� �θ�� ����
            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            enemy.transform.position = enemySpawnPos[randPos].position;
            // var enemy = Instantiate(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            // enemy.transform.SetParent(enemySpawnPos[randPos]);
            // ���� ���� ����Ʈ�� �߰�
            curEnemies.Add(enemy);
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
            // var enemy = Instantiate(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            // enemy.transform.SetParent(enemySpawnPos[randPos]);
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

            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            enemy.transform.position = enemySpawnPos[randPos].position;
            //var enemy = Instantiate(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            //enemy.transform.SetParent(enemySpawnPos[randPos]);
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
            //var enemy = Instantiate(eliteEnemyPrefabs[Random.Range(0, eliteEnemyPrefabs.Length)], enemySpawnPos[randPos].position, Quaternion.identity);
            //enemy.transform.SetParent(enemySpawnPos[randPos]);
            enemy.gameObject.SetActive(false);
            curEnemies.Add(enemy);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }
    }

    public IEnumerator ShowEnemySpawnPos(Transform spawnPos, Poolable enemy)
    {
        //var dangerMarkObj = Instantiate(dangerMark, enemySpawnPos.position, Quaternion.identity);

        var dangerMarkObj = Managers.Pool.Pop(dangerMark);
        dangerMarkObj.transform.position = spawnPos.position;
         
        yield return new WaitForSeconds(spawnTime);

        Managers.Pool.Push(dangerMarkObj);
        enemy.gameObject.SetActive(true);

        yield return null;
    }
    public void RemoveEnemyInList(Poolable enemy)
    {
        curEnemies.Remove(enemy);
        Managers.Pool.Push(enemy);
    }

}
