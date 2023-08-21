using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public SetWall setWall { private set; get; } = null;
    public SetRoad setRoad { private set; get; } = null;
    private SetRoom setRoom = null;

    private int[,] mapArr;
    public int arrSize { private set; get; } = 6;

    private GameObject mapParent = null;

    public Dictionary<Vector3, GameObject> wallDic { private set; get; } = new Dictionary<Vector3, GameObject>();

    [SerializeField]
    private SpawnRoom[] spawnRooms;
    [SerializeField]
    private EnemyRoom[] enemyRooms;

    private int startRoomCount = 1;
    private int enemyRoomCount = 7;
    private int eliteMobRoomCount = 1;
    private int eventRoomCount = 3;

    private Dictionary<Define.RoomTypeFlag, int> roomCountDic = new Dictionary<Define.RoomTypeFlag, int>();
    public Dictionary<Define.EventRoomTypeFlag, int> eventRoomCountDic = new Dictionary<Define.EventRoomTypeFlag, int>();

    public ShopRoom shop { get; private set; } = null;

    private GameObject dropItemPrefab = null;

    public bool isSetting { private set; get; }

    private void Awake()
    {
        mapArr = new int[arrSize, arrSize];

        dropItemPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/DropItem.prefab");

        setWall = FindObjectOfType<SetWall>();
        setRoom = FindObjectOfType<SetRoom>();
        setRoad = FindObjectOfType<SetRoad>();

        SetRoomCountDic();
        SetEventRoomCountDic();

        mapParent = new GameObject("Map");

        StartCoroutine(SetStage());
        setWall.StartSetWall(mapParent.transform);
        setRoom.SetRoomInMapArr();
        setRoad.StartSetRoad(mapParent.transform);

        isSetting = true;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => 
        setWall.isSetupComplete 
        && setRoom.isSetupComplete 
        && setRoom.isSetupComplete);

        spawnRooms = FindObjectsOfType<SpawnRoom>();
        shop = FindObjectOfType<ShopRoom>();
        enemyRooms = FindObjectsOfType<EnemyRoom>();
        yield return new WaitUntil(() => enemyRooms.Length > 3);

        StartCoroutine(UIManager.Instance.ShowCurrentStageName());

    }

    public IEnumerator SetStage()
    {
        yield return new WaitForSeconds(1.5f);
        isSetting = false;
    }

    public int[,] GetMapArr()
    {
        return mapArr;
    }

    public void AddWallDicKey(Vector3 vec, GameObject obj)
    {
        wallDic.Add(vec, obj);
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
        for (int i = 0; i < System.Enum.GetValues(typeof(Define.EventRoomTypeFlag)).Length; ++i)
        {
            eventRoomCountDic.Add((Define.EventRoomTypeFlag)i, 1);
        }
    }

    public void InstantiateChest(Vector3 pos, Define.ChestRating chestRating)
    {
        GameObject chestObj = Managers.Resource.Instantiate("Assets/03.Prefabs/Chest.prefab");
        chestObj.transform.position = pos;
        Chest chest = chestObj.GetComponent<Chest>();
        chest.SetChestRating(chestRating);
    }

    public void ToggleRoomDoor(Vector3 vec)
    {
        wallDic[new Vector3(vec.x / 28f, -vec.y / 28f)].GetComponentInChildren<RoomBase>().ToggleDoors();
    }
}