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

    // 이외 방법 : SO에 몹 배열을 선언, 해당 SO에 잡몹 넣고 어드레서블 적용. 그리고 사용할 때 SO에 있는 몹 불러오기

    public void SetEnemyInList()
    {
        Debug.Log($"mapTypeFlag : {GameManager.Instance.mapTypeFlag}");

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

    public void SetRandomEnemyCount()
    {
        int rand = Random.Range(1, 5);

        switch (rand)
        {
            // TODO : 강화 스켈레톤, 팬텀, 매지션 수 조정 ( 노션 몹 수치 테이블에 있음 )
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

    public IEnumerator SpawnEnemy(Transform[] enemySpawnPos)
    {
        if (door.IsFirst)
        {
            door.IsFirst = false;
        }

        door.CloseDoors();
        
        int randPos = 0;
        isNextWave = false;

        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");

        for (int i = 0; i < wave1NormalEnemyCount; ++i)
        {
            // 적 소환 위치를 담은 배열의 끝까지 범위지정
            randPos = Random.Range(1, enemySpawnPos.Length);
            // 자식(몹)이 있다면 다시 랜드
            while (enemySpawnPos[randPos].childCount != 0)
            {
                randPos = Random.Range(1, enemySpawnPos.Length);
            }

            // 몹 소환
            // 적 소환 위치를 부모로 설정
            var enemy = Managers.Pool.Pop(normalEnemyPrefabs[Random.Range(0, normalEnemyPrefabs.Length)], enemySpawnPos[randPos]);
            // 현재 적들 리스트에 추가
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
        //if(enemy == null)
        //{
        //    for(int i = 0; i < curEnemies.Count; ++i)
        //    {
        //        if(curEnemies[i] == null)
        //        {
        //            curEnemies.RemoveAt(i);
        //        }
        //    }
        //    return;
        //}

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
