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

    private int startRoomNum = 0;

    [SerializeField]
    private List<GameObject> wayMinimapIconList = new List<GameObject>();

    #region LinkedRoom
    // 배열의 상, 하, 좌, 우를 확인할 배열
    int[] dx = new int[4] { 1, 0, -1, 0 };
    int[] dy = new int[4] { 0, -1, 0, 1 };

    // 현재 방의 x, y좌표값을 가져올 변수
    int posX = 0;
    int posY = 0;

    Vector3 roomPos;

    // 연결된 방의 좌표값을 담을 벡터
    Vector3 originRoomPos;
    Vector3 originWayPos;
    #endregion

    [SerializeField]
    private GameObject MoveNextMapPortal;

    private GameObject dropItemPrefab = null;

    public bool isSetting { private set; get; }

    private void Awake()
    {
        wallGrids = new GameObject[4];
        // TODO : WallGrid 및 포탈 어드레서블로 불러오기
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

        // 벽 생성
        SetWallGrid();
        // 길 초기화
        InitWay();
        // 방 스포너 받아옴
        spawnRooms = FindObjectsOfType<SpawnRoom>();
        // 시작 방 설정
        SetStartRoomNShopRoom();
        // 방 생성
        InstantiateRooms();

        enemyRooms = FindObjectsOfType<EnemyRoom>();
        yield return new WaitUntil(() => enemyRooms.Length > 9);
        // 포탈방 지정
        SetMoveNextMapRoom();

        StartCoroutine(UIManager.Instance.ShowCurrentStageName());
        spawnRooms[startRoomNum].GetSummonedRoom().CheckLinkedRoom();
    }

    public IEnumerator SetStage()
    {
        yield return new WaitForSeconds(1.5f);
        isSetting = false;
    }
    public void SetWallGrid()
    {
        randWallGrid = Random.Range(0, wallGrids.Length);
        wallGrid = wallGrids[randWallGrid];
        var wallGridObj = Instantiate(wallGrid);
        var wayMinimapIcon = wallGridObj.transform.Find("WayMinimapIcons");
        for(int i = 0; i < wayMinimapIcon.childCount; ++i)
        {
            wayMinimapIconList.Add(wayMinimapIcon.GetChild(i).gameObject);
        }
    }

    public void InitWay()
    {
        for(int i = 0; i < wayMinimapIconList.Count; ++i)
        {
            wayMinimapIconList[i].SetActive(false);
        }
    }

    private int randRoom = 0;
    public void SetStartRoomNShopRoom()
    {
        // Debug.Log("SpawnRooms : " + spawnRooms.Length);
        startRoomNum = Random.Range(0, spawnRooms.Length);
        spawnRooms[startRoomNum].IsStartRoom = true;
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
        Debug.Log("포탈방 지정");
        int rand = Random.Range(0, enemyRooms.Length);
        enemyRooms[rand].isMoveAnotherStage = true;
        enemyRooms[rand].InstantiateMoveMapIcon();
    }

    public void AssignMoveNextMapPortal(EnemyRoom enemyRoom)
    {
        Instantiate(MoveNextMapPortal, enemyRoom.gameObject.transform.position, Quaternion.identity);
    }

    public void InstantiateDropItem(Vector3 pos)
    {
        // TODO : 아이템 드랍 애니메이션 추가

        Managers.Pool.Pop(dropItemPrefab, pos);
    }

    public void ShowLinkedMapInMinimap(Vector3 pos)
    {
        // wallGrid의 정보를 가져옴
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
            Rito.Debug.Log("wallGridInfo is null");
            return;
        }

        // x값과 y값이 최대 3까지만 나오게끔 세팅
        posY = ( (int)(pos.x - MapInfo.firstPosX) / (int)MapInfo.xDir ) * 2;
        posX = ( (int)(pos.y - MapInfo.firstPosY) / (int)MapInfo.yDir) * 2;

        // 배열의 상(x + 1), 하(x - 1), 좌(y - 1), 우(y + 1) 중 길이 있는지 체크
        // 순서 : 상, 좌, 하, 우 (반시계)
        for(int i = 0; i < 4; ++i)
        {
            // 배열의 전체 크기보다 크거나 작을경우 배열 범위 이탈
            if ((posX + dx[i]) < 0 || posY + dy[i] < 0 || posX + dx[i] > 6 || posY + dy[i] > 6)
            {
                continue;
            }

            // wallGrid의 x, y좌표가 길일경우
            if (wallGridInfo[posX + dx[i], posY + dy[i]] == 2)
            {
                originRoomPos = new Vector3(((posY / 2) + dy[i]) * MapInfo.xDir + MapInfo.firstPosX, ((posX / 2) + dx[i]) * MapInfo.yDir + MapInfo.firstPosY);
                originWayPos = new Vector3((posY + dy[i]) * MapInfo.xDirWay + MapInfo.firstPosXWay, (posX + dx[i]) * MapInfo.yDirWay + MapInfo.firstPosYWay);

                for (int j = 0; j < spawnRooms.Length; ++j)
                {
                    roomPos = spawnRooms[j].transform.position;
                    // x, y값을 원래대로 돌려놓은 값이 spawnRooms[j]의 좌표값과 같을경우
                    if (roomPos == originRoomPos)
                    {
                        // 미니맵에 아이콘 표기
                        spawnRooms[j].GetSummonedRoom().ShowInMinimap();
                        ShowWayMinimapIcon();
                        break;
                    }
                }
            }
        }
    }

    public void ShowWayMinimapIcon()
    {
        for(int i = 0; i < wayMinimapIconList.Count; ++i)
        {
            if(wayMinimapIconList[i].transform.position == originWayPos)
            {
                if(!wayMinimapIconList[i].activeSelf)
                {
                    wayMinimapIconList[i].SetActive(true);
                }
                else
                {
                    Debug.Log("already wayIcon is active!");
                }
            }
        }
    }
}