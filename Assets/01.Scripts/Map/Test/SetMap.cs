using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using TMPro;

// 테스트용으로 만든 것이므로, 리팩토링 필수. 현재 필요한 모든 것들을 한 스크립트에 박아둠

public class SetMap : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI debug = null;

    private enum Wall
    {
        T = 0,
        B,
        L,
        R
    }

    // 남은 방 개수
    int remainingRoomCount = 12;

    int eventRoomCount = 4;

    // 상, 하, 좌, 우
    private int[] dx = new int[4] { 0, 0, -1, 1 };
    private int[] dy = new int[4] { -1, 1, 0, 0 };

    private int[,] mapArr;
    private int arrSize = 6;

    private Queue<Vector2> roomPosQueue = new Queue<Vector2>();

    private string spawnWallstr = "";

    private void Awake()
    {
        mapArr = new int[arrSize, arrSize];
    }

    private void Start()
    {
        Init();

        SetMapForm();

        SetRoom();

        InstantiateWall();
        InstantiateRoom();

        DebugTest();
    }

    private void Init()
    {
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
        remainingRoomCount--;

        GenerateMapArr(new Vector2(3, 3), 2);
        while (roomPosQueue.Count != 0)
        {
            GenerateMapArr(roomPosQueue.Dequeue());
        }
    }

    private void GenerateMapArr(Vector2 pos, int MinSpawnCount = 1)
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

                int rand = UnityEngine.Random.Range(0, 2);
                if (rand < 1)
                {
                    mapArr[ny, nx] = (int)Define.RoomTypeFlag.EnemyRoom;
                    remainingRoomCount--;
                    spawnRoomCount++;
                    roomPosQueue.Enqueue(new Vector2(nx, ny));
                }
            }
        }
    }

    private void SetRoom()
    {
        int nx = 0;
        int ny = 0;

        // 이벤트방 설정
        for (int y = 0; y < arrSize; ++y)
        {
            if (eventRoomCount == 0)
                break;

            for (int x = 0; x < arrSize; ++x)
            {
                if (eventRoomCount == 0)
                    break;

                // 몬스터방일 경우
                if (mapArr[y, x] == 2)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        if (eventRoomCount == 0)
                            break;

                        nx = x + dx[i];
                        ny = y + dy[i];

                        if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                            continue;

                        if (mapArr[ny, nx] != 1 && mapArr[ny, nx] != 0)
                        {
                            mapArr[ny, nx] = 4;
                            eventRoomCount--;
                        }
                    }
                }
            }
        }

        // 엘리트몹 방 설정 (임시)
        int dis = 0;
        tempMapArr = (int[,])mapArr.Clone();
        FindFurthestRoom(3, 3, dis);

        if(furthestRooms.Count != 0)
        {
            int rand = UnityEngine.Random.Range(0, furthestRooms.Count);
            mapArr[(int)furthestRooms[rand].y, (int)furthestRooms[rand].x] = 3;
        }

    }

    List<Vector2> furthestRooms = new List<Vector2>();
    int farDis = 0;
    int[,] tempMapArr;

    private void FindFurthestRoom(int x, int y, int dis)
    {
        Debug.Log("가장 먼 방 찾는중..");
        int nx = 0;
        int ny = 0;
        int findCount = 0;

        for (int i = 0; i < 4; ++i)
        {
            nx = x + dx[i];
            ny = y + dy[i];

            if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                continue;

            if(tempMapArr[ny, nx] != 0 && tempMapArr[ny, nx] != 1)
            {
                tempMapArr[ny, nx] = 0;
                FindFurthestRoom(nx, ny, dis++);
                findCount++;
            }
        }

        if(findCount == 0 && dis >= farDis)
        {
            farDis = dis;
            furthestRooms.Add(new Vector2(x, y));
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

    private void InstantiateWall()
    {
        int nx = 0;
        int ny = 0;

        for(int y = 0; y < arrSize; ++y)
        {
            for(int x = 0; x < arrSize; ++x)
            {
                if (mapArr[y, x] == 0)
                    continue;

                spawnWallstr = "Wall_";
                for(int i = 0; i < 4; ++i)
                {
                    nx = x + dx[i];
                    ny = y + dy[i];

                    if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                        continue;

                    if (mapArr[ny, nx] != 0)
                        spawnWallstr += Enum.GetName(typeof(Wall), i);
                }

                GameObject room = Managers.Resource.Instantiate($"Assets/03.Prefabs/Map_Wall/{spawnWallstr}.prefab");
                room.transform.position = new Vector3(x * 21, -y * 21);
            }
        }
        
    }

    // 테스트용
    private void InstantiateRoom()
    {
        for (int y = 0; y < arrSize; ++y)
        {
            for (int x = 0; x < arrSize; ++x)
            {
                if (mapArr[y, x] != 0)
                {
                    //// 테스트용
                    //if(mapArr[y, x] == 1)
                    //{
                    //    Managers.Resource.Instantiate($"03.Prefabs/Maps/Ghost/GhostFieldNormal.Start", transform);
                    //}
                    //if (mapArr[y, x] == 2)
                    //{
                    //    Managers.Resource.Instantiate($"Assets/03.Prefabs/Maps/Ghost/GhostFieldNormal.0.prefab", transform);
                    //}
                    //if (mapArr[y, x] == 3)
                    //{
                    //    Managers.Resource.Instantiate($"03.Prefabs/Maps/Ghost/GhostFieldNormal.Elite", transform);
                    //}
                    //if (mapArr[y, x] == 4)
                    //{
                    //    // 임시
                    //    Managers.Resource.Instantiate($"03.Prefabs/Maps/Ghost/GhostFieldNormal.Start", transform);
                    //}

                }
            }
        }
    }

    private void DebugTest()
    {
        string s = "";
        for (int y = 0; y < arrSize; ++y)
        {
            for (int x = 0; x < arrSize; ++x)
            {
                s += mapArr[y, x];
            }
            s += "\n";
        }

        debug.SetText(s);
    }
}
