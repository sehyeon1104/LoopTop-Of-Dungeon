using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoEnemySpawn : MonoBehaviour
{
    [SerializeField] 
    private Transform[] enemySpawnPos = null;
    private GameObject enemyPrefab = null;
    private int mobSpawnCount;

    private GameObject enemySpawnEffect = null;

    private WaitForSeconds waitForSpawnTime;
    private WaitForSeconds waitForHalfSpawnTime;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        enemyPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/TutoMob.prefab");

        mobSpawnCount = enemySpawnPos.Length;

        enemySpawnEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/EnemySpawnEffect2.prefab");
        Managers.Pool.CreatePool(enemySpawnEffect, 10);
    }

    public void SpawnEnemy()
    {
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Mob/Mob_Spawn.wav");
        for (int i = 0; i < mobSpawnCount; ++i)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPos[i]);
            TutorialManager.Instance.tutoEnemyRoom.AddEnemyInList(enemy);
            enemy.gameObject.SetActive(false);
            StartCoroutine(ShowEnemySpawnPos(enemySpawnPos[i], enemy.GetComponent<Poolable>()));
        }
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
}
