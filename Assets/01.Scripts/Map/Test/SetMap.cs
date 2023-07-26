using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMap : MonoBehaviour
{
    // 디버깅용
    public GameObject room = null;
    public List<GameObject> rooms = new List<GameObject>();

    int remainingRoomCount = 12;

    // 상, 하, 좌, 우
    private int[] dx = new int[4] { 0, 0, -1, 1 };
    private int[] dy = new int[4] { -1, 1, 0, 0 };

    private int[,] mapArr = new int[7, 7];

    private Queue<Vector2> roomPosQueue = new Queue<Vector2>(); 

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

        for (int i = 0; i < 7; ++i)
        {
            for (int j = 0; j < 7; ++j)
            {
                if (mapArr[i, j] != 0)
                    mapArr[i, j] = 0;
            }
        }

        remainingRoomCount = 12;
    }

    private void SetMapForm()
    {
        mapArr[3, 3] = (int)Define.RoomTypeFlag.StartRoom;
        GameObject Room = Instantiate(room, new Vector3(3 - 3, 3 - 3), Quaternion.identity);
        rooms.Add(Room);
        remainingRoomCount--;

        AddMap(new Vector2(3, 3), 2);
        while(roomPosQueue.Count != 0)
        {
            AddMap(roomPosQueue.Dequeue());
        }
    }

    private void AddMap(Vector2 pos, int MinSpawnCount = 1)
    {
        if (remainingRoomCount <= 0)
        {
            Debug.Log("방 전부 소환 완료");
            return;
        }

        int x = (int)pos.x;
        int y = (int)pos.y;
        int nx = 0; // 다음 x좌표
        int ny = 0; // 다음 y좌표

        int roomCount = 0;
        int spawnRoomCount = 0;

        // 방이 2개 이상인지 체크
        for(int i = 0; i < 4; ++i)
        {
            nx = x + dx[i];
            ny = y + dy[i];

            if (nx > 6 || nx < 0 || ny > 6 || ny < 0)
            {
                Debug.Log($"nx : {nx}, nx : {ny}");
                Debug.Log("범위 초과");
                continue;
            }

            if (mapArr[ny, nx] != 0)
                roomCount++;
        }

        if (roomCount >= 2)
            return;

        // 디버깅용
        int loopCount = 0;

        while(spawnRoomCount < MinSpawnCount)
        {
            loopCount++;
            if (loopCount >= 100)
            {
                Debug.LogError("Too many loop!");
                Debug.Log(x + "," + y);
                break;
            }

            for (int i = 0; i < 4; ++i)
            {
                nx = x + dx[i];
                ny = y + dy[i];

                // 범위초과 또는 생성하려는 좌표에 방이 있다면 취소
                if (nx > 6 || nx < 0 || ny > 6 || ny < 0)
                {
                    Debug.Log($"nx : {nx}, nx : {ny}");
                    Debug.Log("범위 초과");
                    continue;
                }

                if(mapArr[ny, nx] != 0)
                    continue;

                int rand = Random.Range(0, 2);
                if (rand < 1)
                {
                    Debug.Log("방 소환");
                    mapArr[ny, nx] = (int)Define.RoomTypeFlag.EnemyRoom;
                    GameObject Room = Instantiate(room, new Vector3(nx - 3, ny - 3), Quaternion.identity);
                    rooms.Add(Room);
                    remainingRoomCount--;
                    spawnRoomCount++;
                    roomPosQueue.Enqueue(new Vector2(nx, ny));
                }
            }
        }

    }
}
