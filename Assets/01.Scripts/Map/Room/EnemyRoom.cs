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
            EnemySpawnManager.Instance.SetEliteMonsterSpawnBool(isMoveAnotherStage, transform);

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

        EnemySpawnManager.Instance.SetEnemyWaveCount();
        StartCoroutine(EnemySpawnManager.Instance.ManagingEnemy(SetEnemySpawnPos()));


        StartCoroutine(CheckClear());
    }

    private IEnumerator CheckClear()
    {
        yield return new WaitUntil(() => EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave);
        IsClear();
    }

    public void InstantiateMoveMapIcon()
    {
        Debug.Log("아이콘 생성");
        if (isMoveAnotherStage)
        {
            portalMapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/PortalMapIcon.prefab");
            portalMapIcon.transform.position = transform.position;
            portalMapIcon.SetActive(false);
        }
    }

    protected override void ShowIcon()
    {
        Debug.Log("ShowIcon");
        if (isMoveAnotherStage)
        {
            portalMapIcon.SetActive(true);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

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
        // TODO : 맵이 클리어 되었는지 체크
        if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave)
            isClear = true;

        if (isClear)
        {
            Door.Instance.OpenDoors();

            if (isMoveAnotherStage)
            {
                // TODO : 아이템 드랍 상자 구현

                StageManager.Instance.AssignMoveNextMapPortal(this);
            }
        }
    }
}
