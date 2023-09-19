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

    // 몹 소환 비율
    private float[] enemySpawnRatioArr = new float[5];

    private float normalEnemyRatio = 0.5f;       // 기본
    private float highHpEnemyRatio = 0.3f;       // 뚱땡이
    private float highSpeedEnemyRatio = 0.15f;    // 이속빠름 ( ex)팬텀 )
    private float longDisEnemyRatio = 0.05f;      // 원거리

    // 현재 몹 소환 수
    private int spawnCount = 0;
    // 몹 유형
    private int mobType = 0;
    // 소환 비율
    private float spawnRatio = 0f;
    // 필요 소환 수
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

    // 이외 방법 : SO에 몹 배열을 선언, 해당 SO에 잡몹 넣고 어드레서블 적용. 그리고 사용할 때 SO에 있는 몹 불러오기

    public void SetEnemyInList()
    {
        //Debug.Log($"mapTypeFlag : {GameManager.Instance.mapTypeFlag}");

        // TODO : 동적으로 몬스터 개수 가져와 리스트 할당

        for (int i = 0; i < 4; ++i)
        {
            // 맵 타입 플래그에 맞는 몹 몹 프리팹 불러옴
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
            // TODO : 강화 스켈레톤, 팬텀, 매지션 수 조정 ( 노션 몹 수치 테이블에 있음 )
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

        // TODO : 이후 코드 가독성 올리기. 지금 보기 너무 더럽다 

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

            // 적 소환 위치를 담은 배열의 끝까지 범위지정
            randPos = Random.Range(1, enemySpawnPos.Length);
            // 자식(몹)이 있다면 다시 랜드
            int loopCount = 0;
            while (enemySpawnPos[randPos].childCount != 0)
            {
                if (loopCount >= 100)
                    Debug.LogError("Too Many loop!");

                loopCount++;
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            // 몹 소환
            // 적 소환 위치를 부모로 설정
            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[(int)mobTypeFlag], enemySpawnPos[randPos]);
            // 현재 적들 리스트에 추가
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
