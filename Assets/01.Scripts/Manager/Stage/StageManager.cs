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

    private int startRoomCount = 1;
    private int enemyRoomCount = 7;
    private int eliteMobRoomCount = 1;
    private int eventRoomCount = 3;

    private Dictionary<Define.RoomTypeFlag, int> roomCountDic = new Dictionary<Define.RoomTypeFlag, int>();
    public Dictionary<Define.EventRoomTypeFlag, int> eventRoomCountDic = new Dictionary<Define.EventRoomTypeFlag, int>();

    [SerializeField]
    private List<GameObject> wayMinimapIconList = new List<GameObject>();

    #region LinkedRoom
    // 배열???? ?? �? ?��? ?�인??배열
    int[] dx = new int[4] { 1, 0, -1, 0 };
    int[] dy = new int[4] { 0, -1, 0, 1 };

    // ?�재 방의 x, y좌표값을 가?�올 변??
    int posX = 0;
    int posY = 0;

    Vector3 roomPos;

    // ?�결??방의 좌표값을 ?�을 벡터
    Vector3 originRoomPos;
    Vector3 originWayPos;
    #endregion

    private GameObject dropItemPrefab = null;

    public bool isSetting { private set; get; }

    private void Awake()
    {
        wallGrids = new GameObject[4];
        // TODO : WallGrid �??�탈 ?�드?�서블로 불러?�기
        for(int i = 0; i < 4; ++i)
        {
            wallGrids[i] = Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Map_Wall/WallGrid{i + 1}.prefab");
        }

        dropItemPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/DropItem.prefab");

        SetRoomCountDic();
        SetEventRoomCountDic();
    }

    private void SetRoomCountDic()
    {
        roomCountDic.Add(Define.RoomTypeFlag.StartRoom, startRoomCount);
        roomCountDic.Add(Define.RoomTypeFlag.EnemyRoom, enemyRoomCount);
        roomCountDic.Add(Define.RoomTypeFlag.EliteMobRoom, eliteMobRoomCount);
        roomCountDic.Add(Define.RoomTypeFlag.EventRoom, eventRoomCount);
    }

    private void SetEventRoomCountDic()
    {
        for(int i = 0; i < System.Enum.GetValues(typeof(Define.EventRoomTypeFlag)).Length; ++i)
        {
            eventRoomCountDic.Add((Define.EventRoomTypeFlag)i, 1);
        }
    }

    private IEnumerator Start()
    {
        isSetting = true;
        StartCoroutine(SetStage());

        SetWallGrid();
        InitWay();
        spawnRooms = FindObjectsOfType<SpawnRoom>();
        SetRoom();
        InstantiateRooms();

        enemyRooms = FindObjectsOfType<EnemyRoom>();
        yield return new WaitUntil(() => enemyRooms.Length > 3);

        StartCoroutine(UIManager.Instance.ShowCurrentStageName());
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
    public void SetRoom()
    {
        for(int i = 0; i < spawnRooms.Length; ++i)
        {
            // ���� ����� �̺�Ʈ ����� �������� ����
            randRoom = Random.Range(1, 5);

            // ���� ���� ������ 0�̶�� ReRoll
            if (roomCountDic[(Define.RoomTypeFlag)randRoom] == 0){
                --i;
                continue;
            }
            
            // �� Ÿ�� ����
            spawnRooms[i].RoomTypeFlag = (Define.RoomTypeFlag)randRoom;
            roomCountDic[(Define.RoomTypeFlag)randRoom]--;
        }
    }

    public void InstantiateRooms()
    {
        foreach(var room in spawnRooms)
        {
            room.SetAndInstantiateRoom();
        }
    }

    public void InstantiateDropItem(Vector3 pos)
    {
        // TODO : ���ڸ� ���� ������ ���

        Managers.Pool.Pop(dropItemPrefab, pos);
    }

    public void ShowLinkedMapInMinimap(Vector3 pos)
    {
        // wallGridInfo
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

        // x값과 y값이 최�? 3까�?�??�오게끔 ?�팅
        posY = ( (int)(pos.x - MapInfo.firstPosX) / (int)MapInfo.xDir ) * 2;
        posX = ( (int)(pos.y - MapInfo.firstPosY) / (int)MapInfo.yDir) * 2;

        // 배열????x + 1), ??x - 1), �?y - 1), ??y + 1) �?길이 ?�는지 체크
        // ?�서 : ?? �? ?? ??(반시�?
        for(int i = 0; i < 4; ++i)
        {
            // 배열???�체 ?�기보다 ?�거???�을경우 배열 범위 ?�탈
            if ((posX + dx[i]) < 0 || posY + dy[i] < 0 || posX + dx[i] > 6 || posY + dy[i] > 6)
            {
                continue;
            }

            // wallGrid??x, y좌표가 길일경우
            if (wallGridInfo[posX + dx[i], posY + dy[i]] == 2)
            {
                originRoomPos = new Vector3(((posY / 2) + dy[i]) * MapInfo.xDir + MapInfo.firstPosX, ((posX / 2) + dx[i]) * MapInfo.yDir + MapInfo.firstPosY);
                originWayPos = new Vector3((posY + dy[i]) * MapInfo.xDirWay + MapInfo.firstPosXWay, (posX + dx[i]) * MapInfo.yDirWay + MapInfo.firstPosYWay);

                for (int j = 0; j < spawnRooms.Length; ++j)
                {
                    roomPos = spawnRooms[j].transform.position;
                    // x, y값을 ?�래?��??�려?��? 값이 spawnRooms[j]??좌표값과 같을경우
                    if (roomPos == originRoomPos)
                    {
                        // 미니맵에 ?�이�??�기
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