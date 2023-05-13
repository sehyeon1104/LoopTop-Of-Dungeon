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

    [SerializeField]
    private GameObject MoveNextMapPortal;

    public bool isSetting { private set; get; }

    private void Awake()
    {
        wallGrids = new GameObject[4];
        // TODO : WallGrid �� ��Ż ��巹����� �ҷ�����
        for(int i = 0; i < 4; ++i)
        {
            wallGrids[i] = Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Map_Wall/WallGrid{i + 1}.prefab");
        }

        MoveNextMapPortal = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Maps/Magic_Circle_Move.prefab");
    }

    private IEnumerator Start()
    {
        isSetting = true;
        StartCoroutine(SetStage());

        // �� ����
        SetWallGrid();
        // �� ������ �޾ƿ�
        spawnRooms = FindObjectsOfType<SpawnRoom>();
        // ���� �� ����
        SetStartRoomNShopRoom();
        // �� ����
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
        GameObject portalMapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/PortalMapIcon.prefab");
        portalMapIcon.transform.position = enemyRooms[rand].transform.position;
    }

    public void SetWallGrid()
    {
        //GameObject[] map = GameObject.FindGameObjectsWithTag("Map");
        //if(map != null)
        //{
        //    for(int i = 0; i < map.Length; ++i)
        //    {
        //        Destroy(map[i]);
        //    }
        //}

        randWallGrid = Random.Range(0, wallGrids.Length);
        wallGrid = wallGrids[randWallGrid];
        Instantiate(wallGrid);

    }

    public void AssignMoveNextMapPortal(EnemyRoom enemyRoom)
    {
        Instantiate(MoveNextMapPortal, enemyRoom.gameObject.transform.position, Quaternion.identity);
    }
}