using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : RoomBase
{
    [SerializeField]
    private GameObject enemySpawnPosObj;

    [SerializeField]
    private Transform[] enemySpawnPos;

    public bool isSpawnMonster { private set; get; } = false;

    GameObject portalMapIcon;


    private void Start()
    {
        isSpawnMonster = false;
        SetRoomTypeFlag();
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
        EnemySpawnManager.Instance.SetEnemyWaveCount();
        StartCoroutine(EnemySpawnManager.Instance.ManagingEnemy(SetEnemySpawnPos(), transform.parent.position));

        StartCoroutine(CheckClear());
    }

    private IEnumerator CheckClear()
    {
        yield return new WaitUntil(() => EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave);
        IsClear();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            if (!isClear && !isSpawnMonster)
            {
                isSpawnMonster = true;
                SetEnemy();
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isClear)
            {
                base.OnTriggerExit2D(collision);
            }

            if (StageManager.Instance.isSetting)
            {
                return;
            }
        }

        //if (collision.CompareTag("Player"))
        //{
        //    IsClear();
        //}
    }

    protected override void IsClear()
    {
        if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave)
            isClear = true;

        if (isClear)
        {
            ItemManager.Instance.RoomClearRelatedItemEffects?.Invoke();
            StageManager.Instance.ToggleRoomDoor(transform.parent.position);
        }
    }
}
