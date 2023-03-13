using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField]
    private GameObject[] wallGrids;

    private GameObject wallGrid;

    private int randWallGrid;

    [SerializeField]
    private SpawnRoom[] spawnRooms;
    private EnemyRoom[] enemyRooms;
    private Vector3 playerSpawnPos;

    [SerializeField]
    private GameObject MoveNextMapPortal;

    public bool isSetting { private set; get; }

    private void Start()
    {
        isSetting = true;
        StartCoroutine(SetStage());

        // 벽 생성
        SetWallGrid();
        // 방 스포너 받아옴
        spawnRooms = FindObjectsOfType<SpawnRoom>();
        // 시작 방 설정
        SetStartRoom();
        // 방 생성
        InstantiateRooms();

        enemyRooms = FindObjectsOfType<EnemyRoom>();
        SetMoveNextMapRoom();
        // Player.Instance.transform.position = SetPlayerSpawnPos();
    }

    public IEnumerator SetStage()
    {
        yield return new WaitForSeconds(1.5f);
        isSetting = false;
    }

    public void SetStartRoom()
    {
        Debug.Log("SpawnRooms : " + spawnRooms.Length);
        spawnRooms[Random.Range(0, spawnRooms.Length)].IsStartRoom = true;
    }

    public void InstantiateRooms()
    {
        foreach(var room in spawnRooms)
        {
            room.SetAndInstantiateRoom();
        }
    }

    public void SetMoveNextMapRoom()
    {
        int rand = Random.Range(0, enemyRooms.Length);
        enemyRooms[rand].isMoveAnotherStage = true;
    }

    public void SetWallGrid()
    {
        GameObject map = GameObject.FindGameObjectWithTag("Map");
       if(map != null)
        {
            Destroy(map);
            randWallGrid = Random.Range(0, wallGrids.Length);
            wallGrid = wallGrids[randWallGrid];
            Instantiate(wallGrid);
        }
       else if(map == null)
        {

            randWallGrid = Random.Range(0, wallGrids.Length);
            wallGrid = wallGrids[randWallGrid];
            Instantiate(wallGrid);
        }


    }

    public void AssignMoveNextMapPortal(EnemyRoom enemyRoom)
    {
        Instantiate(MoveNextMapPortal, enemyRoom.gameObject.transform.position, Quaternion.identity);
    }
}