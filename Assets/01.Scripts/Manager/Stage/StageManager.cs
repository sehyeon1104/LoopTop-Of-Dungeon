using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    private SetWall setWall = null;
    private SetRoom setRoom = null;

    private int[,] mapArr;
    public int arrSize { private set; get; } = 6;

    private GameObject mapParent = null;

    private void Awake()
    {
        mapArr = new int[arrSize, arrSize];

        dropItemPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/DropItem.prefab");

        setWall = FindObjectOfType<SetWall>();
        setRoom = FindObjectOfType<SetRoom>();

        SetRoomCountDic();
        SetEventRoomCountDic();
    }

    private IEnumerator Start()
    {
        mapParent = new GameObject("Map");

        setWall.StartSetWall(mapParent.transform);
        setRoom.SetRoomInMapArr();

        isSetting = true;
        spawnRooms = FindObjectsOfType<SpawnRoom>();
        shop = FindObjectOfType<ShopRoom>();
        enemyRooms = FindObjectsOfType<EnemyRoom>();
        yield return new WaitUntil(() => enemyRooms.Length > 3);

        StartCoroutine(UIManager.Instance.ShowCurrentStageName());

        //setRoom.DebugTest();
    }

    public int[,] GetMapArr()
    {
        return mapArr;
    }














    // ∏∏£ºË
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

    public ShopRoom shop { get; private set; } = null;

    [SerializeField]
    private List<GameObject> wayMinimapIconList = new List<GameObject>();

    #region LinkedRoom
    // Î∞∞Ïó¥???? ?? Ï¢? ?∞Î? ?ïÏù∏??Î∞∞Ïó¥
    int[] dx = new int[4] { 1, 0, -1, 0 };
    int[] dy = new int[4] { 0, -1, 0, 1 };

    // ?ÑÏû¨ Î∞©Ïùò x, yÏ¢åÌëúÍ∞íÏùÑ Í∞Ä?∏Ïò¨ Î≥Ä??
    int posX = 0;
    int posY = 0;

    Vector3 roomPos;

    // ?∞Í≤∞??Î∞©Ïùò Ï¢åÌëúÍ∞íÏùÑ ?¥ÏùÑ Î≤°ÌÑ∞
    Vector3 originRoomPos;
    Vector3 originWayPos;
    #endregion

    private GameObject dropItemPrefab = null;

    public bool isSetting { private set; get; }

    //private void Awake()
    //{
    //    //wallGrids = new GameObject[4];
    //    //// TODO : WallGrid Î∞??¨ÌÉà ?¥Îìú?àÏÑúÎ∏îÎ°ú Î∂àÎü¨?§Í∏∞
    //    //for (int i = 0; i < 4; ++i)
    //    //{
    //    //    wallGrids[i] = Managers.Resource.Load<GameObject>($"Assets/03.Prefabs/Map_Wall/WallGrid{i + 1}.prefab");
    //    //}
    //}

    private void SetRoomCountDic()
    {
        roomCountDic.Add(Define.RoomTypeFlag.StartRoom, startRoomCount);
        roomCountDic.Add(Define.RoomTypeFlag.EnemyRoom, enemyRoomCount);
        roomCountDic.Add(Define.RoomTypeFlag.EliteMobRoom, eliteMobRoomCount);
        roomCountDic.Add(Define.RoomTypeFlag.EventRoom, eventRoomCount);
    }

    private void SetEventRoomCountDic()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Define.EventRoomTypeFlag)).Length; ++i)
        {
            eventRoomCountDic.Add((Define.EventRoomTypeFlag)i, 1);
        }
    }

    //private IEnumerator Start()
    //{
    //    isSetting = true;
    //    StartCoroutine(SetStage());

    //    SetWallGrid();
    //    InitWay();
    //    spawnRooms = FindObjectsOfType<SpawnRoom>();
    //    SetRoom();
    //    InstantiateRooms();

    //    shop = FindObjectOfType<ShopRoom>();
    //    enemyRooms = FindObjectsOfType<EnemyRoom>();
    //    yield return new WaitUntil(() => enemyRooms.Length > 3);

    //    StartCoroutine(UIManager.Instance.ShowCurrentStageName());
    //}

    //public IEnumerator SetStage()
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    isSetting = false;
    //}

    //public void SetWallGrid()
    //{
    //    randWallGrid = Random.Range(0, wallGrids.Length);
    //    wallGrid = wallGrids[randWallGrid];
    //    var wallGridObj = Instantiate(wallGrid);
    //    var wayMinimapIcon = wallGridObj.transform.Find("WayMinimapIcons");
    //    for(int i = 0; i < wayMinimapIcon.childCount; ++i)
    //    {
    //        wayMinimapIconList.Add(wayMinimapIcon.GetChild(i).gameObject);
    //    }
    //}

    //public void InitWay()
    //{
    //    for(int i = 0; i < wayMinimapIconList.Count; ++i)
    //    {
    //        wayMinimapIconList[i].SetActive(false);
    //    }
    //}

    //private int randRoom = 0;
    //public void SetRoom()
    //{
    //    for(int i = 0; i < spawnRooms.Length; ++i)
    //    {
    //        // Ω√¿€ πÊ∫Œ≈Õ ¿Ã∫•∆Æ πÊ±Ó¡ˆ ∑£¥˝¿∏∑Œ ±º∏≤
    //        randRoom = Random.Range(1, 5);

    //        // ≥≤¿∫ πÊ¿« ∞≥ºˆ∞° 0¿Ã∂Û∏È ReRoll
    //        if (roomCountDic[(Define.RoomTypeFlag)randRoom] == 0){
    //            --i;
    //            continue;
    //        }

    //        // πÊ ≈∏¿‘ ¡ˆ¡§
    //        spawnRooms[i].RoomTypeFlag = (Define.RoomTypeFlag)randRoom;
    //        roomCountDic[(Define.RoomTypeFlag)randRoom]--;
    //    }
    //}

    //public void InstantiateRooms()
    //{
    //    foreach (var room in spawnRooms)
    //    {
    //        room.SetAndInstantiateRoom();
    //    }
    //}

    public void InstantiateChest(Vector3 pos, Define.ChestRating chestRating)
    {
        // TODO : ªÛ¿⁄ º“»Ø
        Debug.Log("ªÛ¿⁄ º“»Ø");
        GameObject chestObj = Managers.Resource.Instantiate("Assets/03.Prefabs/Chest.prefab");
        chestObj.transform.position = pos;
        Chest chest = chestObj.GetComponent<Chest>();
        chest.SetChestRating(chestRating);
    }

    //public void ShowLinkedMapInMinimap(Vector3 pos)
    //{
    //    // wallGridInfo
    //    wallGridInfo = randWallGrid switch
    //    {
    //        0 => MapInfo.WallGrid1,
    //        1 => MapInfo.WallGrid2,
    //        2 => MapInfo.WallGrid3,
    //        3 => MapInfo.WallGrid4,

    //        _ => null
    //    };

    //    if (wallGridInfo == null)
    //    {
    //        Rito.Debug.Log("wallGridInfo is null");
    //        return;
    //    }

    //    // xÍ∞íÍ≥º yÍ∞íÏù¥ ÏµúÎ? 3ÍπåÏ?Îß??òÏò§Í≤åÎÅî ?∏ÌåÖ
    //    posY = ( (int)(pos.x - MapInfo.firstPosX) / (int)MapInfo.xDir ) * 2;
    //    posX = ( (int)(pos.y - MapInfo.firstPosY) / (int)MapInfo.yDir) * 2;

    //    // Î∞∞Ïó¥????x + 1), ??x - 1), Ï¢?y - 1), ??y + 1) Ï§?Í∏∏Ïù¥ ?àÎäîÏßÄ Ï≤¥ÌÅ¨
    //    // ?úÏÑú : ?? Ï¢? ?? ??(Î∞òÏãúÍ≥?
    //    for(int i = 0; i < 4; ++i)
    //    {
    //        // Î∞∞Ïó¥???ÑÏ≤¥ ?¨Í∏∞Î≥¥Îã§ ?¨Í±∞???ëÏùÑÍ≤ΩÏö∞ Î∞∞Ïó¥ Î≤îÏúÑ ?¥ÌÉà
    //        if ((posX + dx[i]) < 0 || posY + dy[i] < 0 || posX + dx[i] > 6 || posY + dy[i] > 6)
    //        {
    //            continue;
    //        }

    //        // wallGrid??x, yÏ¢åÌëúÍ∞Ä Í∏∏ÏùºÍ≤ΩÏö∞
    //        if (wallGridInfo[posX + dx[i], posY + dy[i]] == 2)
    //        {
    //            originRoomPos = new Vector3(((posY / 2) + dy[i]) * MapInfo.xDir + MapInfo.firstPosX, ((posX / 2) + dx[i]) * MapInfo.yDir + MapInfo.firstPosY);
    //            originWayPos = new Vector3((posY + dy[i]) * MapInfo.xDirWay + MapInfo.firstPosXWay, (posX + dx[i]) * MapInfo.yDirWay + MapInfo.firstPosYWay);

    //            for (int j = 0; j < spawnRooms.Length; ++j)
    //            {
    //                roomPos = spawnRooms[j].transform.position;
    //                // x, yÍ∞íÏùÑ ?êÎûò?ÄÎ°??åÎ†§?ìÏ? Í∞íÏù¥ spawnRooms[j]??Ï¢åÌëúÍ∞íÍ≥º Í∞ôÏùÑÍ≤ΩÏö∞
    //                if (roomPos == originRoomPos)
    //                {
    //                    // ÎØ∏ÎãàÎßµÏóê ?ÑÏù¥ÏΩ??úÍ∏∞
    //                    spawnRooms[j].GetSummonedRoom().ShowInMinimap();
    //                    ShowWayMinimapIcon();
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //}

    //public void ShowWayMinimapIcon()
    //{
    //    for(int i = 0; i < wayMinimapIconList.Count; ++i)
    //    {
    //        if(wayMinimapIconList[i].transform.position == originWayPos)
    //        {
    //            if(!wayMinimapIconList[i].activeSelf)
    //            {
    //                wayMinimapIconList[i].SetActive(true);
    //            }
    //            else
    //            {
    //                // Debug.Log("already wayIcon is active!");
    //            }
    //        }
    //    }
    //}
}