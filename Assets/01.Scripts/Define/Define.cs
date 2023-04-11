using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum SkillNum
    {
        FirstSkill,
        SecondSkill,
        ThirdSkill,
        ForthSkill,
        FifthSkill,
        Attack,
        UltimateSkill,
        DashSkill,
    }

    public enum MapTypeFlag
    {
        Default = 0,
        CenterMap,
        Ghost,
        LavaSlime,
        Electricity,
        Werewolf,
        Lizard,
        Power,
    }

    public enum RoomTypeFlag
    {
        Default = 0,
        StartRoom,
        EnemyRoom,
        BossRoom,
        Shop,
    }

    public enum PlayerTransformTypeFlag
    {
        Power = 0,
        Ghost,

    }
    public enum Scene
    {
        Unknown,
        TitleScene,
        CenterScene,
        Ghost_Stage1,
        Ghost_Stage2,
        Ghost_Boss,
    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
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

    public enum ItemType
    {
        Default = 0,
        buff,
        heal,
        
    }
}
