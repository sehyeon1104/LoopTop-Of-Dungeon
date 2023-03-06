using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : RoomBase
{
    private void Start()
    {
        SetEnemy();
    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.EnemyRoom;
    }

    private void SetEnemy()
    {
        EnemySpawnManager.Instance.SetKindOfEnemy(mapTypeFlag);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        EnemySpawnManager.Instance.SetRandomEnemyCount();
        EnemySpawnManager.Instance.SpawnEnemy();
    }

    protected override bool IsClear()
    {
        // TODO : ���� Ŭ���� �Ǿ����� üũ
        return isClear;
    }
}
