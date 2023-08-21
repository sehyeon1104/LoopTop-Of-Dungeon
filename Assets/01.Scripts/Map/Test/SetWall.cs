using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetWall : MonoBehaviour
{
    private enum Wall
    {
        T = 0,
        B,
        L,
        R
    }

    // 남은 방 개수
    int remainingRoomCount = 12;

    private int[,] mapArr;
    private int arrSize = 0;

    // 상, 하, 좌, 우
    private int[] dx = new int[4] { 0, 0, -1, 1 };
    private int[] dy = new int[4] { -1, 1, 0, 0 };

    private Queue<Vector2> roomPosQueue = new Queue<Vector2>();

    private string spawnWallstr = "";

    public float wallInterval { private set; get; } = 28f;

    public bool isSetupComplete { private set; get; } = false;


    public void StartSetWall(Transform parent)
    {
        mapArr = StageManager.Instance.GetMapArr();
        arrSize = StageManager.Instance.arrSize;

        Init();

        SetMapForm();

        InstantiateWall(parent);

        isSetupComplete = true;
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

        while (spawnRoomCount < MinSpawnCount)
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

    private void InstantiateWall(Transform parent)
    {
        int nx = 0;
        int ny = 0;

        for (int y = 0; y < arrSize; ++y)
        {
            for (int x = 0; x < arrSize; ++x)
            {
                if (mapArr[y, x] == 0)
                    continue;

                spawnWallstr = "Wall_";
                for (int i = 0; i < 4; ++i)
                {
                    nx = x + dx[i];
                    ny = y + dy[i];

                    if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                        continue;

                    if (mapArr[ny, nx] != 0)
                        spawnWallstr += Enum.GetName(typeof(Wall), i);
                }

                GameObject room = Managers.Resource.Instantiate($"Assets/03.Prefabs/Map_Wall/{spawnWallstr}.prefab");
                room.transform.SetParent(parent);
                room.transform.position = new Vector3(x * wallInterval, -y * wallInterval);
                StageManager.Instance.AddWallDicKey(new Vector3(room.transform.position.x / wallInterval, -room.transform.position.y / wallInterval), room);
            }
        }

    }
}
