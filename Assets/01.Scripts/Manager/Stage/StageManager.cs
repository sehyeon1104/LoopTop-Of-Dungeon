using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField]
    private GameObject[] wallGrids;


    private GameObject wallGrid;

    private int randWallGrid;

    private EnemyRoom[] enemyRooms;
    private Transform playerSpawnPos;

    [SerializeField]
    private GameObject MoveNextMapPortal;

    private void Start()
    {
        enemyRooms = FindObjectsOfType<EnemyRoom>();
        SetWallGrid();
        SetMoveNextMapRoom();
    }

    public Transform SetPlayerSpawnPos()
    {
        foreach(var enemyRoomItem in enemyRooms)
        {
            if(enemyRoomItem.GetRoomTypeFlag == Define.RoomTypeFlag.StartRoom)
            {
                playerSpawnPos = enemyRoomItem.transform;
            }
        }

        return playerSpawnPos;
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