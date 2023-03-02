using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum PlayerTransformTypeFlag
    {
        Normal = 0,
        Power,
        Ghost,

    }
    public enum Scene
    {
        Unknown,
        TitleScene,
        MainScene,
    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
}
