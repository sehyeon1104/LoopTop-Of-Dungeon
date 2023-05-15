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
    private List<GameObject> normalEnemyPrefabsList = new List<GameObject>();// = new GameObject[10];

    private GameObject[] normalEnemyPrefabs;

    [field: SerializeField]
    public List<Poolable> curEnemies { private set; get; } = new List<Poolable>();

    private int wave1NormalEnemyCount = 0;
    private int wave2NormalEnemyCount = 0;

    private Transform eliteMonsterSpawnPos;

    private GameObject enemySpawnEffect = null;
    [SerializeField]
    private float spawnTime = 1f;

    private GameObject enemyDeadEffect = null;

    private Door door = null;

    public bool isNextWave { private set; get; } = false;
    private bool isSpawnEliteEnemy = false;

    private WaitForSeconds waitForSpawnTime;
    private WaitForSeconds waitForHalfSpawnTime;


    public AssetLabelReference assetLabel;
    private IList<IResourceLocation> _locations;

    private float enemyCount;


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
        enemyCount = 0;
        SetEnemyInList();
        InvokeRepeating("CheckCurEnemyList", 0f, 5f);
    }

    #region Addressable

    // �̿� ��� : SO�� �� �迭�� ����, �ش� SO�� ��� �ְ� ��巹���� ����. �׸��� ����� �� SO�� �ִ� �� �ҷ�����

    public void GetLocations()
    {
        // ����Ÿ���� ��θ� �����´�.
        // ����̱� ������ �޸𸮿� ������ �ε���� �ʴ´�.
        //Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
        //    (handle) =>
        //    {
        //        _locations = handle.Result;
        //    };

        // TODO : Ư�� ���� �� ���� ���� ��������

    }

    public void SetEnemyInList()
    {
        Debug.Log($"mapTypeFlag : {GameManager.Instance.mapTypeFlag}");

        // TODO : �������� ���� ���� ������ ����Ʈ �Ҵ�

        for (int i = 0; i < 4; ++i)
        {
            // �� Ÿ�� �÷��׿� �´� �� �� ������ �ҷ���
            normalEnemyPrefabsList.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/Normal/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
            Managers.Pool.CreatePool(normalEnemyPrefabsList[i], 5);
        }
    }

    #endregion

    public void SetKindOfEnemy()
    {
        normalEnemyPrefabs = normalEnemyPrefabsList.ToArray();
    }

    public void SetRandomEnemyCount()
    {
        int rand = Random.Range(1, 5);

        switch (rand)
        {
            case 1:
                wave1NormalEnemyCount = 4;
                wave2NormalEnemyCount = 5;
                break;
            case 2:
                wave1NormalEnemyCount = 5;
                wave2NormalEnemyCount = 4;
                break;
            case 3:
                wave1NormalEnemyCount = 3;
                wave2NormalEnemyCount = 6;
                break;
            case 4:
                wave1NormalEnemyCount = 5;
                wave2NormalEnemyCount = 5;
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

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");

        for(int i = 0; i < wave1NormalEnemyCount; ++i)
        {

            // �� ��ȯ ��ġ�� ���� �迭�� ������ ��������
            randPos = Random.Range(1, enemySpawnPos.Length);
            // �ڽ�(��)�� �ִٸ� �ٽ� ����
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            // �� ��ȯ
            // �� ��ȯ ��ġ�� �θ�� ����
            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            enemy.transform.position = enemySpawnPos[randPos].position;
            // ���� ���� ����Ʈ�� �߰�
            curEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }

        yield return new WaitUntil(() => curEnemies.Count <= 0);


        curEnemies.Clear();

        yield return waitForSpawnTime;

        isNextWave = true;

        if (isSpawnEliteEnemy)
        {
            SpawnEliteMonster(eliteMonsterSpawnPos);
        }

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");

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

        isSpawnEliteEnemy = false;
    }

    public void SetEliteMonsterSpawnBool(bool isSpawn, Transform spawnPos)
    {
        isSpawnEliteEnemy = isSpawn;
        eliteMonsterSpawnPos = spawnPos;
    }

    public void SpawnEliteMonster(Transform spawnPos)
    {
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");
        Poolable eliteMonster = Managers.Pool.PoolManaging("Assets/03.Prefabs/Enemy/Ghost/Elite/G_Mob_Elite_01.prefab", spawnPos.position, Quaternion.identity);
        curEnemies.Add(eliteMonster);
        StartCoroutine(ShowEnemySpawnPos(eliteMonster.transform, eliteMonster));
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

    public void CheckCurEnemyList()
    {
        if(curEnemies.Count > 0)
        {
            for(int i = 0; i < curEnemies.Count; ++i)
            {
                if (!curEnemies[i].gameObject.activeSelf)
                {
                    curEnemies.RemoveAt(i);
                }
            }
        }
    }

}
