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
    public bool isSpawnMonster { private set; get; } = false;

    private void Start()
    {
        isSpawnMonster = false;
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
            // Debug.Log("SetEnemySpawnPos");
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
        EnemySpawnManager.Instance.SetKindOfEnemy();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        // Debug.Log("SpawnEnemies");
        //if (isMoveAnotherStage)
        //{
        //    EnemySpawnManager.Instance.SetEliteMonsterSpawnBool(isMoveAnotherStage, transform);
        //}

        EnemySpawnManager.Instance.SetRandomEnemyCount();
        StartCoroutine(EnemySpawnManager.Instance.SpawnEnemy(SetEnemySpawnPos()));


        StartCoroutine(CheckClear());
    }

    private IEnumerator CheckClear()
    {
        yield return new WaitUntil(() => EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave);
        IsClear();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (StageManager.Instance.isSetting)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            if (!isClear && !isSpawnMonster)
            {
                isSpawnMonster = true;
                Door.Instance.CloseDoors();
                SetEnemy();
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
        }
    }

    protected override void IsClear()
    {
        // TODO : 맵이 클리어 되었는지 체크
        if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave)
            isClear = true;

        if (isClear)
        {
            Door.Instance.OpenDoors();

            if (isMoveAnotherStage)
            {
                StageManager.Instance.AssignMoveNextMapPortal(this);
            }
        }
    }
}
