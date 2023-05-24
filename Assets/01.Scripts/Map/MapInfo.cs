using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapInfo
{
    public static float xDir { get; private set; } = 26f;
    public static float yDir { get; private set; } = 26f;

    public static float firstPosX { get; private set; } = -8f;
    public static float firstPosY { get; private set; } = -2.25f;

    public static float xDirWay { get; private set; } = 13f;
    public static float yDirWay { get; private set; } = 13f;

    public static float firstPosXWay { get; private set; } = -7.5f;
    public static float firstPosYWay { get; private set; } = -2.75f;

    public static int[,] WallGrid1 { get; private set; } =new int[7, 7]
    {
        {1, 2, 1, 0, 1, 2, 1},
        {2, 0, 2, 0, 2, 0, 2},
        {1, 2, 1, 2, 1, 0, 1},
        {0, 0, 2, 0, 2, 0, 0},
        {1, 2, 1, 2, 1, 0, 0},
        {2, 0, 0, 0, 0, 0, 0},
        {1, 0, 0, 0, 0, 0, 0},
    };

    public static int[,] WallGrid2 { get; private set; } = new int[7, 7]
    {
        { 1, 2, 1, 0, 1, 2, 1 },
        { 2, 0, 2, 0, 2, 0, 2 },
        { 1, 2, 1, 2, 1, 0, 1 },
        { 0, 0, 2, 0, 2, 0, 2 },
        { 0, 0, 1, 2, 1, 0, 1 },
        { 0, 0, 0, 0, 2, 0, 0 },
        { 0, 0, 0, 0, 1, 0, 0 },

    };

    public static int[,] WallGrid3 { get; private set; } = new int[7, 7]
    {
        {1, 2, 1, 2, 1, 2, 1 },
        {2, 0, 2, 0, 0, 0, 0 } ,
        {1, 2, 1, 0, 0, 0, 0}  ,
        {2, 0, 2, 0, 0, 0, 0}  ,
        {1, 0, 1, 2, 1, 0, 0}  ,
        {2, 0, 2, 0, 2, 0, 0}  ,
        {1, 0, 1, 2, 1, 0, 0}  ,
    };

    public static int[,] WallGrid4 { get; private set; } = new int[7, 7]
    {
        {1, 2, 1, 2, 1, 0, 0},
        {2, 0, 2, 0, 0, 0, 0},
        {1, 2, 1, 0, 1, 0, 0},
        {2, 0, 2, 0, 2, 0, 0},
        {1, 2, 1, 2, 1, 0, 0},
        {2, 0, 2, 0, 2, 0, 0},
        {1, 0, 1, 2, 1, 0, 0},
    };


}
