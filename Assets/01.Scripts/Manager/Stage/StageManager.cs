using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private IEnumerator Start()
    {
        isSetting = true;
        StartCoroutine(SetStage());

        // 벽 생성
        SetWallGrid();
        // 방 스포너 받아옴
        spawnRooms = FindObjectsOfType<SpawnRoom>();
        // 시작 방 설정
        SetStartRoomNShopRoom();
        // 방 생성
        InstantiateRooms();

        enemyRooms = FindObjectsOfType<EnemyRoom>();
        yield return new WaitUntil(() => enemyRooms.Length > 9);
        SetMoveNextMapRoom();

        StartCoroutine(UIManager.Instance.ShowCurrentStageName());
    }

    public IEnumerator SetStage()
    {
        yield return new WaitForSeconds(1.5f);
        isSetting = false;
    }

    private int randRoom = 0;
    public void SetStartRoomNShopRoom()
    {
        // Debug.Log("SpawnRooms : " + spawnRooms.Length);
        spawnRooms[Random.Range(0, spawnRooms.Length)].IsStartRoom = true;
        randRoom = Random.Range(0, spawnRooms.Length);
        if (spawnRooms[randRoom].IsStartRoom)
        {
            while (spawnRooms[randRoom].IsStartRoom)
            {
                randRoom = Random.Range(0, spawnRooms.Length);
            }
        }
        spawnRooms[randRoom].IsShopRoom = true;
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