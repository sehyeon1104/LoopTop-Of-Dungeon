using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetRoom : MonoBehaviour
{
    int eventRoomCount = 4;

    int arrSize = 6;

    // 상, 하, 좌, 우
    private int[] dx = new int[4] { 0, 0, -1, 1 };
    private int[] dy = new int[4] { -1, 1, 0, 0 };

    public void SetRoomInMapArr(ref int[,] mapArr)
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

        if (furthestRooms.Count != 0)
        {
            int rand = UnityEngine.Random.Range(0, furthestRooms.Count);
            mapArr[(int)furthestRooms[rand].y, (int)furthestRooms[rand].x] = 3;
        }
        else
        {
            Debug.LogError("오류. 엘리트몹 방 생성 안됨");
        }

    }

    List<Vector2> furthestRooms = new List<Vector2>();
    int farDis = 0;
    int[,] tempMapArr;

    private void FindFurthestRoom(int x, int y, int dis)
    {
        int nx = 0;
        int ny = 0;
        int findCount = 0;

        for (int i = 0; i < 4; ++i)
        {
            nx = x + dx[i];
            ny = y + dy[i];

            if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                continue;

            if (tempMapArr[ny, nx] != 0 && tempMapArr[ny, nx] != 1)
            {
                tempMapArr[ny, nx] = 0;
                FindFurthestRoom(nx, ny, dis++);
                findCount++;
            }
        }

        if (findCount == 0 && dis >= farDis)
        {
            farDis = dis;
            furthestRooms.Add(new Vector2(x, y));
        }
    }
}
