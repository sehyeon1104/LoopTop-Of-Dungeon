using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : RoomBase
{
    [SerializeField]
    private GameObject enemySpawnPosObj;

    [SerializeField]
    private Transform[] enemySpawnPos;

    public bool isMoveAnotherStage = false;

    private void Start()
    {
        SetRoomTypeFlag();
        SetEnemySpawnPos();
    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.EnemyRoom;
    }

    public Define.RoomTypeFlag GetRoomTypeFlag => roomTypeFlag;

    private Transform[] SetEnemySpawnPos()
    {
        if(roomTypeFlag == Define.RoomTypeFlag.EnemyRoom)
        {
            Debug.Log("SetEnemySpawnPos");
            enemySpawnPos = enemySpawnPosObj.GetComponentsInChildren<Transform>();

            return enemySpawnPos;
        }
        else
        {
            Debug.LogWarning("RoomTypeFlag is Not EnemyRoom!");
            return null;
        }
    }

    // 플레이어 입장 시 실행
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (StageManager.Instance.isSetting)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Enter) isClear : " + isClear);
            if (!isClear)
            {
                Door.Instance.EnableDoors();
                SetEnemy();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isClear && Door.Instance.isEnableDoor && !StageManager.Instance.isSetting)
            {
                Door.Instance.DisableDoors();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (StageManager.Instance.isSetting)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            IsClear();
            Debug.Log("Exit) isClear : " + isClear);
        }
    }

    protected override bool IsClear()
    {
        // TODO : 맵이 클리어 되었는지 체크
        if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave)
            isClear = true;

        if(isClear && isMoveAnotherStage)
        {
            StageManager.Instance.AssignMoveNextMapPortal(this);
        }

        return isClear;
    }
}
