using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 디버깅
using TMPro;

public class SetMap : MonoBehaviour
{
    // 디버깅용
    public GameObject room = null;
    public List<GameObject> rooms = new List<GameObject>();

    // 남은 방 개수
    int remainingRoomCount = 12;

    // 상, 하, 좌, 우
    private int[] dx = new int[4] { 0, 0, -1, 1 };
    private int[] dy = new int[4] { -1, 1, 0, 0 };

    private int[,] mapArr;
    private int arrSize = 6;

    private Queue<Vector2> roomPosQueue = new Queue<Vector2>();

    private void Awake()
    {
        mapArr = new int[arrSize, arrSize];
    }

    private void Start()
    {
        Init();

        SetMapForm();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Init();

            SetMapForm();
        }
    }

    private void Init()
    {
        foreach (var roomObj in rooms)
        {
            Destroy(roomObj);
        }
        rooms.Clear();

        for (int i = 0; i < arrSize; ++i)
        {
            for (int j = 0; j < arrSize; ++j)
            {
                if (mapArr[i, j] != 0)
                    mapArr[i, j] = 0;
            }
        }

        remainingRoomCount = 12;
    }

    private void SetMapForm()
    {
        // 시작방 (정중앙) 생성
        mapArr[3, 3] = (int)Define.RoomTypeFlag.StartRoom;
        GameObject Room = Instantiate(room, new Vector3(3 - 3, 3 - 3), Quaternion.identity);
        Room.GetComponent<SpriteRenderer>().color = Color.yellow;
        rooms.Add(Room);
        remainingRoomCount--;

        AddMap(new Vector2(3, 3), 2);
        while (roomPosQueue.Count != 0)
        {
            AddMap(roomPosQueue.Dequeue());
        }
    }

    private void AddMap(Vector2 pos, int MinSpawnCount = 1)
    {
        if (remainingRoomCount <= 0)
            return;

        int x = (int)pos.x;
        int y = (int)pos.y;
        int nx = 0; // 다음 x좌표
        int ny = 0; // 다음 y좌표

        int spawnRoomCount = 0;

        // 디버깅용
        int loopCount = 0;

        while(spawnRoomCount < MinSpawnCount)
        {
            loopCount++;
            if (loopCount >= 100)
            {
                Debug.LogError("Too many loop!");
                break;
            }

            for (int i = 0; i < 4; ++i)
            {
                if (remainingRoomCount <= 0)
                    return;

                nx = x + dx[i];
                ny = y + dy[i];

                // 범위초과 또는 생성하려는 좌표에 방이 있다면 취소
                if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                    continue;

                if (mapArr[ny, nx] != 0)
                    continue;

                if (CheckNearbyRoomCount(nx, ny) >= 2)
                    continue;

                int rand = Random.Range(0, 2);
                if (rand < 1)
                {
                    mapArr[ny, nx] = (int)Define.RoomTypeFlag.EnemyRoom;
                    GameObject Room = Instantiate(room, new Vector3(nx - 3, ny - 3), Quaternion.identity);
                    Room.GetComponent<SpriteRenderer>().color = Color.white;
                    rooms.Add(Room);
                    remainingRoomCount--;
                    spawnRoomCount++;
                    roomPosQueue.Enqueue(new Vector2(nx, ny));
                }
            }
        }
    }

    // 주변 방 체크 함수
    private int CheckNearbyRoomCount(int x, int y)
    {
        int nx = 0;
        int ny = 0;
        int roomCount = 0;

        for (int i = 0; i < 4; ++i)
        {
            nx = x + dx[i];
            ny = y + dy[i];

            if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                continue;

            if (mapArr[ny, nx] != 0)
                roomCount++;
        }

        return roomCount;
    }
}
