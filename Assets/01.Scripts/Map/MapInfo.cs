using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapInfo
{
    public static int[,] WallGrid { get; private set; } =new int[7, 7]
    {
        {1, 0, 0, 0, 0, 0, 0},
        {2, 0, 0, 0, 0, 0, 0},
        {1, 2, 1, 2, 1, 0, 0},
        {0, 0, 2, 0, 2, 0, 0},
        {1, 2, 1, 2, 1, 0, 1},
        {2, 0, 2, 0, 2, 0, 2},
        {1, 2, 1, 0, 1, 2, 1}
    };


}
