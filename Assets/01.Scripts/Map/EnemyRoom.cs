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
        // TODO : 맵이 클리어 되었는지 체크
        return isClear;
    }
}
