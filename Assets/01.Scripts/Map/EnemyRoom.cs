using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : RoomBase
{
    [SerializeField]
    private GameObject enemySpawnPosObj;

    [SerializeField]
    private Transform[] enemySpawnPos;

    private void Start()
    {
        SetRoomTypeFlag();
        SetEnemySpawnPos();
    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.EnemyRoom;
    }

    private Transform[] SetEnemySpawnPos()
    {
        Debug.Log("SetEnemySpawnPos");
        enemySpawnPos = enemySpawnPosObj.GetComponentsInChildren<Transform>();
        
        return enemySpawnPos;
    }

    // �÷��̾� ���� �� ����
    private void SetEnemy()
    {
        Debug.Log("SetEnemy");
        EnemySpawnManager.Instance.SetKindOfEnemy(mapTypeFlag);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Debug.Log("SpawnEnemies");
        EnemySpawnManager.Instance.SetRandomEnemyCount();
        StartCoroutine(EnemySpawnManager.Instance.SpawnEnemy(SetEnemySpawnPos()));
    }

    protected override bool IsClear()
    {
        // TODO : ���� Ŭ���� �Ǿ����� üũ
        if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave)
            isClear = true;

        return isClear;
    }
}
