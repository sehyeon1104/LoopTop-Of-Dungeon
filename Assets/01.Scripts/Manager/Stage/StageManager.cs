using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField]
    private GameObject[] wallGrids;

    private GameObject wallGrid;

    private int[,] wallGridInfo;

    private int randWallGrid;

    [SerializeField]
    private SpawnRoom[] spawnRooms;
    private EnemyRoom[] enemyRooms;

    [SerializeField]
    private GameObject[] roadMinimapIcon;

    [SerializeField]
    private GameObject MoveNextMapPortal;

    private GameObject dropItemPrefab = null;

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
        dropItemPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/DropItem.prefab");
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

    public void InstantiateDropItem(Vector3 pos)
    {
        // TODO : ������ ��� �ִϸ��̼� �߰�

        Managers.Pool.Pop(dropItemPrefab, pos);
    }

    public void ShowLinkedMapInMinimap(Vector3 pos)
    {
        Debug.Log("ShowLinkedMapInMinimap");
        // wallGrid�� ������ ������
        wallGridInfo = randWallGrid switch
        {
            0 => MapInfo.WallGrid1,
            1 => MapInfo.WallGrid2,
            2 => MapInfo.WallGrid3,
            3 => MapInfo.WallGrid4,

            _ => null
        };

        if (wallGridInfo == null)
        {
            Debug.Log("wallGridInfo is null");
            return;
        }

        //Debug.Log($"wallGrid{randWallGrid}");

        //for(int i = 0; i < 7; ++i)
        //{
        //    for(int j = 0; j < 7; ++j)
        //    {
        //        Debug.Log($"({i}, {j}) : {wallGridInfo[i, j]}");
        //    }
        //    Debug.Log(",");
        //}

        // x���� y���� �ִ� 3������ �����Բ� ����
        int y = ( (int)(pos.x - MapInfo.firstPosX) / (int)MapInfo.xDir ) * 2;
        int x = ( (int)(pos.y - MapInfo.firstPosY) / (int)MapInfo.yDir) * 2;
        Debug.Log($"x : {x}");
        Debug.Log($"y : {y}");

        int[] dx = new int[4]{ 1, 0, -1, 0 };
        int[] dy = new int[4]{ 0, -1, 0, 1 };

        // �迭�� ��(x + 1), ��(x - 1), ��(y - 1), ��(y + 1) �� ���� �ִ��� üũ
        for(int i = 0; i < 4; ++i)
        {
            Debug.Log($"{i}��° : ({x + dx[i]}, {y + dy[i]})");
            if ((x + dx[i]) < 0 || y + dy[i] < 0 || x + dx[i] > 6 || y + dy[i] > 6)
            {
                Debug.Log("�迭 ���� �ʰ�");
                continue;
            }

            if (wallGridInfo[x + dx[i], y + dy[i]] == 2)
            {
                // TODO : ����� �� �ҷ�����
                Debug.Log("��");
                for(int j = 0; j < spawnRooms.Length; ++j)
                {
                    Vector3 roomPos = spawnRooms[j].transform.position;
                    if (roomPos == new Vector3(((y / 2) + dy[i]) * MapInfo.xDir + MapInfo.firstPosX,  ((x / 2) + dx[i]) * MapInfo.yDir + MapInfo.firstPosY))
                    {
                        spawnRooms[j].GetSummonedRoom().ShowInMinimap();
                        break;
                    }
                }
                // spawnRooms[((x / 2) * 4) + (y / 2)].GetSummonedRoom().ShowInMinimap();
            }
            else if(wallGridInfo[(x / 2) + dx[i], (y / 2) + dy[i]] == 0)
            {
                Debug.Log("�� ����");
            }

            Debug.Log($"{i}��° ��");
        }
    }

    public void ShowRoadMinimapIcon()
    {

    }
}