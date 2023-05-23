using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapInfo
{
    public static float xDir = 26f;
    public static float yDir = 26f;

    public static float firstPosX = -8f;
    public static float firstPosY = -2.25f;

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
        {2, 0, 2, 0, 0, 0, 0},
        {1, 2, 1, 2, 1, 0, 0},
        {1, 2, 1, 0, 1, 0, 0},
        {2, 0, 2, 0, 2, 0, 0},
        {1, 2, 1, 2, 1, 0, 0},
        {2, 0, 2, 0, 2, 0, 0},
        {1, 0, 1, 2, 1, 0, 0},
    };


}
