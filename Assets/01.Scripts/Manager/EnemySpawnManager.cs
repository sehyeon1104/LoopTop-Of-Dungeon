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

    private GameObject enemySpawnEffect = null;
    [SerializeField]
    private float spawnTime = 1f;

    private GameObject enemyDeadEffect = null;

    private Door door = null;

    public bool isNextWave { private set; get; } = false;

    private WaitForSeconds waitForSpawnTime;
    private WaitForSeconds waitForHalfSpawnTime;

    public AssetLabelReference assetLabel;
    private IList<IResourceLocation> _locations;


    private void Awake()
    {
        GetLocations();
    }

    private void Start()
    {
        door = FindObjectOfType<Door>();
        waitForSpawnTime = new WaitForSeconds(spawnTime);
        waitForHalfSpawnTime = new WaitForSeconds(spawnTime * 0.5f);
        enemySpawnEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/EnemySpawnEffect2.prefab");
        enemyDeadEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/EnemyDeadEffect.prefab");
        Managers.Pool.CreatePool(enemySpawnEffect, 10);
        SetEnemyInList();
    }

    #region Addressable

    // �̿� ��� : SO�� �� �迭�� ����, �ش� SO�� ��� �ְ� ��巹���� ����. �׸��� ����� �� SO�� �ִ� �� �ҷ�����

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
        Debug.Log($"mapTypeFlag : {GameManager.Instance.mapTypeFlag}");

        for (int i = 0; i < _locations.Count; ++i)
        {
            // �� Ÿ�� �÷��׿� �´� �� �� ������ �ҷ���
            ghostNormalEnemyPrefabs.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
            ghostEliteEnemyPrefabs.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
            Managers.Pool.CreatePool(ghostNormalEnemyPrefabs[i], 5);
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
    }

    public IEnumerator SpawnEnemy(Transform[] enemySpawnPos)
    {
        // TODO : �� ��ȯ�� ����Ʈ �߰�
        if (door.IsFirst)
        {
            door.IsFirst = false;
        }

        door.CloseDoors();
        
        int randPos = 0;
        isNextWave = false;

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
            // ���� ���� ����Ʈ�� �߰�
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

        yield return waitForSpawnTime;

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
        effect.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y - enemy.transform.localScale.y / 2, 0);

        yield return waitForHalfSpawnTime;

        enemy.gameObject.SetActive(true);

        yield return waitForHalfSpawnTime;

        Managers.Pool.Push(effect);
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
        var enemyDeadEffectClone = Managers.Pool.Pop(enemyDeadEffect);
        enemyDeadEffectClone.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y - 0.5f);
        curEnemies.Remove(enemy);
        Managers.Pool.Push(enemy);
    }

}
