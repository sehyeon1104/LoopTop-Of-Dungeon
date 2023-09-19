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

    // �� ��ȯ ����
    private float[] enemySpawnRatioArr = new float[5];

    private float normalEnemyRatio = 0.5f;       // �⺻
    private float highHpEnemyRatio = 0.3f;       // �׶���
    private float highSpeedEnemyRatio = 0.15f;    // �̼Ӻ��� ( ex)���� )
    private float longDisEnemyRatio = 0.05f;      // ���Ÿ�

    // ���� �� ��ȯ ��
    private int spawnCount = 0;
    // �� ����
    private int mobType = 0;
    // ��ȯ ����
    private float spawnRatio = 0f;
    // �ʿ� ��ȯ ��
    private int requireSpawnCount = 0;

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

    private void Start()
    {
        // door = FindObjectOfType<Door>();
        waitForSpawnTime = new WaitForSeconds(spawnTime);
        waitForHalfSpawnTime = new WaitForSeconds(spawnTime * 0.5f);
        enemySpawnEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/EnemySpawnEffect2.prefab");
        enemyDeadEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/EnemyDeadEffect.prefab");
        Managers.Pool.CreatePool(enemySpawnEffect, 10);
        Managers.Pool.CreatePool(enemyDeadEffect, 10);

        enemySpawnRatioArr[0] = normalEnemyRatio;
        enemySpawnRatioArr[1] = highHpEnemyRatio;
        enemySpawnRatioArr[2] = longDisEnemyRatio;
        enemySpawnRatioArr[3] = highSpeedEnemyRatio;
        enemySpawnRatioArr[4] = 0;
        SetEnemyInList();
    }

    #region Addressable

    // �̿� ��� : SO�� �� �迭�� ����, �ش� SO�� ��� �ְ� ��巹���� ����. �׸��� ����� �� SO�� �ִ� �� �ҷ�����

    public void SetEnemyInList()
    {
        //Debug.Log($"mapTypeFlag : {GameManager.Instance.mapTypeFlag}");

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
                wave1NormalEnemyCount = 8;
                wave2NormalEnemyCount = 12;
                break;
            case 2:
                wave1NormalEnemyCount = 12;
                wave2NormalEnemyCount = 8;
                break;
            case 3:
                wave1NormalEnemyCount = 10;
                wave2NormalEnemyCount = 10;
                break;
            case 4:
                wave1NormalEnemyCount = 10;
                wave2NormalEnemyCount = 10;
                break;
        }
    }

    public IEnumerator ManagingEnemy(Transform[] enemySpawnPos, Vector3 vec)
    {
        //if (door.IsFirst)
        //{
        //    door.IsFirst = false;
        //}

        StageManager.Instance.ToggleRoomDoor(vec);

        // door.CloseDoors();
        
        isNextWave = false;

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");

        // TODO : ���� �ڵ� ������ �ø���. ���� ���� �ʹ� ������ 

        SpawnEnemy(enemySpawnPos, wave1NormalEnemyCount);

        yield return new WaitUntil(() => curEnemies.Count <= 0);

        curEnemies.Clear();

        yield return waitForSpawnTime;

        isNextWave = true;

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");

        SpawnEnemy(enemySpawnPos, wave2NormalEnemyCount);

        isSpawnEliteEnemy = false;
    }

    public void SpawnEnemy(Transform[] enemySpawnPos, int enemyCount)
    {
        int randPos = 0;
        Define.MobTypeFlag mobTypeFlag;
        spawnRatio = normalEnemyRatio;

        mobType = 0;
        spawnCount = 0;
        requireSpawnCount = Mathf.RoundToInt(enemyCount * spawnRatio);

        for (int i = 0; i < enemyCount; ++i)
        {
            mobTypeFlag = (Define.MobTypeFlag)mobType;

            // �� ��ȯ ��ġ�� ���� �迭�� ������ ��������
            randPos = Random.Range(1, enemySpawnPos.Length);
            // �ڽ�(��)�� �ִٸ� �ٽ� ����
            int loopCount = 0;
            while (enemySpawnPos[randPos].childCount != 0)
            {
                if (loopCount >= 100)
                    Debug.LogError("Too Many loop!");

                loopCount++;
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            // �� ��ȯ
            // �� ��ȯ ��ġ�� �θ�� ����
            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[(int)mobTypeFlag], enemySpawnPos[randPos]);
            // ���� ���� ����Ʈ�� �߰�
            curEnemies.Add(enemy);
            enemy.gameObject.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[randPos], enemy));
            spawnCount++;

            if (spawnCount == requireSpawnCount)
            {
                mobType++;
                spawnCount = 0;
                spawnRatio = enemySpawnRatioArr[mobType];
                requireSpawnCount = Mathf.RoundToInt(enemyCount * spawnRatio);
            }
        }
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
