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
        LavaSlime,
        Electricity,
        Werewolf,
        Lizard,
    }

    public enum RoomTypeFlag
    {
        Default = 0,
        StartRoom,
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
        GhostScene1,
        GhostScene2,
        GhostSceneBoss,
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

    public enum StageSceneNum
    {
        Default = 0,
        CenterMap = 1,
        Ghost = 2,
        LavaSlime = 5,
        Electricity = 8,
        Werewolf = 11,
        Lizard = 14,
    }
}
