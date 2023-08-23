using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetRoad : MonoBehaviour
{
    private enum RoadType
    {
        Vertical = 0,
        Horizontal,
    }

    int arrSize = 6;

    // ╩С, го, аб, ©Л
    private int[] dx = new int[4] { 0, 0, -1, 1 };
    private int[] dy = new int[4] { -1, 1, 0, 0 };

    private float wallInterval = 0f;
    private float roadIntervalVerticalX = 12f;
    private float roadIntervalVerticalY = -5f;
    private float roadIntervalHorizontalX = -1f;
    private float roadIntervalHorizontalY = 9f;

    private int[,] tempMapArr;

    public bool isSetupComplete { private set; get; } = false;

    public void StartSetRoad(Transform parent)
    {
        tempMapArr = (int[,])StageManager.Instance.GetMapArr().Clone();
        wallInterval = StageManager.Instance.setWall.wallInterval;

        SearchForRoom(3, 3, parent);

        isSetupComplete = true;
    }

    private void SearchForRoom(int x, int y, Transform parent)
    {
        int nx = 0;
        int ny = 0;

        for (int i = 0; i < 4; ++i)
        {
            nx = x + dx[i];
            ny = y + dy[i];

            if (nx > arrSize - 1 || nx < 0 || ny > arrSize - 1 || ny < 0)
                continue;

            if (tempMapArr[ny, nx] != 0)
            {
                tempMapArr[y, x] = 0;

                SearchForRoom(nx, ny, parent);

                if (i < 2)
                    InstantiateRoad(RoadType.Vertical, parent, i % 2 == 0 ? 0 : 1, x, -y);
                else
                    InstantiateRoad(RoadType.Horizontal, parent, i % 2 == 0 ? 0 : 1, x, -y);
            }
        }
    }

    private void InstantiateRoad(RoadType roadType, Transform parent, int loc, int x, int y)
    {
        if(roadType == RoadType.Vertical)
        {
            GameObject road = Managers.Resource.Instantiate($"Assets/03.Prefabs/Map_Wall/Road_Vertical.prefab", parent);
            road.transform.position = parent.position;
            // ╩С
            if (loc == 0)
                road.transform.Translate(x * wallInterval + roadIntervalVerticalX, y * wallInterval + roadIntervalVerticalY + wallInterval, 0);
            // го
            else
                road.transform.Translate(x * wallInterval + roadIntervalVerticalX, y * wallInterval + roadIntervalVerticalY, 0);
        }
        else
        {
            GameObject road = Managers.Resource.Instantiate($"Assets/03.Prefabs/Map_Wall/Road_Horizontal.prefab", parent);
            road.transform.position = parent.position;
            // аб
            if (loc == 0)
                road.transform.Translate(x * wallInterval + roadIntervalHorizontalX, y * wallInterval + roadIntervalHorizontalY, 0);
            // ©Л
            else
                road.transform.Translate(x * wallInterval + roadIntervalHorizontalX + wallInterval, y * wallInterval + roadIntervalHorizontalY, 0);
        }

    }
}
