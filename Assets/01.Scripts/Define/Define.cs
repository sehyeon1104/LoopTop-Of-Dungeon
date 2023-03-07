using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum MapTypeFlag
    {
        Default = 0,
        CenterMap,
        Ghost,
    }

    public enum RoomTypeFlag
    {
        Default = 0,
        EnemyRoom,
        BossRoom,
        Shop
    }

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
    public enum SkillNum
    {
        firstSkill = 1,
        secondSkill,
        thirdSkill,
        fourthSkill,
        fifthSkill
    }
}
