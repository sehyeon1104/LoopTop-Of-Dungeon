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

    private int highHpEnemyCount = 0;       // �׶���
    private int highSpeedEnemyCount = 0;    // �̼Ӻ��� ( ex)���� )
    private int longDisEnemyCount = 0;      // ���Ÿ�
    private int normalEnemyCount = 0;       // �⺻

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

    private void Start()
    {
        door = FindObjectOfType<Door>();
        waitForSpawnTime = new WaitForSeconds(spawnTime);
        waitForHalfSpawnTime = new WaitForSeconds(spawnTime * 0.5f);
        enemySpawnEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/EnemySpawnEffect2.prefab");
        enemyDeadEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/EnemyDeadEffect.prefab");
        Managers.Pool.CreatePool(enemySpawnEffect, 10);
        Managers.Pool.CreatePool(enemyDeadEffect, 10);

        SetEnemyInList();
    }

    #region Addressable

    // �̿� ��� : SO�� �� �迭�� ����, �ش� SO�� ��� �ְ� ��巹���� ����. �׸��� ����� �� SO�� �ִ� �� �ҷ�����

    public void SetEnemyInList()
    {
        Debug.Log($"mapTypeFlag : {GameManager.Instance.mapTypeFlag}");

        // TODO : �������� ���� ���� ������ ����Ʈ �Ҵ�

        for (int i = 0; i < 4; ++i)
        {
            // �� Ÿ�� �÷��׿� �´� �� �� ������ �ҷ���
            normalEnemyPrefabsList.Add(Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Enemy/{GameManager.Instance.mapTypeFlag}/Normal/{GameManager.Instance.mapTypeFlag.ToString().Substring(0, 1)}_Mob_0{i + 1}.prefab"));
            Managers.Pool.CreatePool(normalEnemyPrefabsList[i], 6);
        }
    }

    #endregion

    public void SetKindOfEnemy()
    {
        normalEnemyPrefabs = normalEnemyPrefabsList.ToArray();
    }

    public void SetEnemyWaveCount()
    {
        int rand = Random.Range(1, 5);

        switch (rand)
        {
            // TODO : ��ȭ ���̷���, ����, ������ �� ���� ( ��� �� ��ġ ���̺� ���� )
            case 1:
                wave1NormalEnemyCount = 5;
                wave2NormalEnemyCount = 7;
                break;
            case 2:
                wave1NormalEnemyCount = 7;
                wave2NormalEnemyCount = 5;
                break;
            case 3:
                wave1NormalEnemyCount = 4;
                wave2NormalEnemyCount = 8;
                break;
            case 4:
                wave1NormalEnemyCount = 6;
                wave2NormalEnemyCount = 6;
                break;
        }
    }

    int randEnemyCount = 0;
    public void SetEnemyCount(int totalEnemySpawnCount)
    {
        if(totalEnemySpawnCount < 6)
        {
            highHpEnemyCount = 1;
            highSpeedEnemyCount = Random.Range(0, 2);
            longDisEnemyCount = 1;
        }
        else if(totalEnemySpawnCount >= 6)
        {
            randEnemyCount = Random.Range(0, 3);
            switch (randEnemyCount)
            {
                case 0:
                    highHpEnemyCount = 2;
                    highSpeedEnemyCount = 1;
                    longDisEnemyCount = 1;
                    break;
                case 1:
                    highHpEnemyCount = 1;
                    highSpeedEnemyCount = Random.Range(0, 2);
                    longDisEnemyCount = 1;
                    break;
                case 2:
                    highHpEnemyCount = 1;
                    highSpeedEnemyCount = 1;
                    longDisEnemyCount = 2;
                    break;
            }
        }
        normalEnemyCount = totalEnemySpawnCount - highHpEnemyCount - highSpeedEnemyCount - longDisEnemyCount;
    }

    public IEnumerator ManagingEnemy(Transform[] enemySpawnPos)
    {
        if (door.IsFirst)
        {
            door.IsFirst = false;
        }

        door.CloseDoors();
        
        isNextWave = false;

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");

        // TODO : ���� �ڵ� ������ �ø���. ���� ���� �ʹ� ������ 
        SetEnemyCount(wave1NormalEnemyCount);

        SpawnEnemy(enemySpawnPos, normalEnemyCount, Define.MobTypeFlag.Normal);
        SpawnEnemy(enemySpawnPos, highHpEnemyCount, Define.MobTypeFlag.HighHp);
        SpawnEnemy(enemySpawnPos, longDisEnemyCount, Define.MobTypeFlag.LongDis);
        SpawnEnemy(enemySpawnPos, highSpeedEnemyCount, Define.MobTypeFlag.HighSpeed);

        //for (int i = 0; i < wave1NormalEnemyCount; ++i)
        //{
        //    // �� ��ȯ ��ġ�� ���� �迭�� ������ ��������
        //    randPos = Random.Range(1, enemySpawnPos.Length);
        //    // �ڽ�(��)�� �ִٸ� �ٽ� ����
        //    while (enemySpawnPos[randPos].childCount != 0)
        //    {
        //        randPos = Random.Range(1, enemySpawnPos.Length);
        //    }

        //    // �� ��ȯ
        //    // �� ��ȯ ��ġ�� �θ�� ����
        //    var enemy = Managers.Pool.Pop(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos]);
        //    // ���� ���� ����Ʈ�� �߰�
        //    curEnemies.Add(enemy);
        //    enemy.gameObject.SetActive(false);
        //    StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        //}

        yield return new WaitUntil(() => curEnemies.Count <= 0);

        curEnemies.Clear();

        yield return waitForSpawnTime;

        isNextWave = true;

        if (isSpawnEliteEnemy)
        {
            SpawnEliteMonster(eliteMonsterSpawnPos);
        }

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");

        SetEnemyCount(wave2NormalEnemyCount);

        SpawnEnemy(enemySpawnPos, normalEnemyCount, Define.MobTypeFlag.Normal);
        SpawnEnemy(enemySpawnPos, highHpEnemyCount, Define.MobTypeFlag.HighHp);
        SpawnEnemy(enemySpawnPos, longDisEnemyCount, Define.MobTypeFlag.LongDis);
        SpawnEnemy(enemySpawnPos, highSpeedEnemyCount, Define.MobTypeFlag.HighSpeed);

        isSpawnEliteEnemy = false;
    }

    public void SpawnEnemy(Transform[] enemySpawnPos, int enemyCount, Define.MobTypeFlag mobTypeFlag)
    {
        int randPos = 0;
        for (int i = 0; i < enemyCount; ++i)
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
            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[(int)mobTypeFlag], enemySpawnPos[randPos]);
            // ���� ���� ����Ʈ�� �߰�
            curEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
        }
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
        if (enemy == null)
        {
            for (int i = 0; i < curEnemies.Count; ++i)
            {
                if (curEnemies[i] == null)
                {
                    curEnemies.RemoveAt(i);
                }
            }
            return;
        }

        var enemyDeadEffectClone = Managers.Pool.Pop(enemyDeadEffect);
        enemyDeadEffectClone.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y - 0.5f);
        curEnemies.Remove(enemy);

        for (int i = 0; i < curEnemies.Count; ++i)
        {
            if (!curEnemies[i].gameObject.activeSelf)
            {
                curEnemies.Remove(enemy);
            }
        }
    }

}
