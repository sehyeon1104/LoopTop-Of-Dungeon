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
        Power,
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
        EliteMobRoom,
        EventRoom,
    }

    public enum EventRoomTypeFlag
    {
        ShopRoom,
        ChestRoom,
        BrokenItemRoom,
        DiceRoom,
        DevilSwordRoom,
        BloodyAltarRoom,
        BloodDonationRoom,
        RedFountainRoom,
    }

    public enum PlayerTransformTypeFlag
    {
        Power = 0,
        Ghost,
        LavaSlime,
        Electricity,
        Werewolf,
        Lizard,

    }

    public enum Scene
    {
        Unknown,
        TitleScene = 0,
        CenterScene,
        StageScene,
        BossScene,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum ItemType
    {
        Default = 0,
        buff,
        heal,
        broken,
        set,
    }

    public enum ItemRating
    {
        Default = 0,
        Common,
        Rare,
        Epic,
        Legendary,
        Special,
        Set,
        ETC,
    }

    public enum ChestRating
    {
        Default = 0,
        Common,
        Rare,
        Epic,
        Legendary,
        Special,
    }

    public enum PlatForm
    {
        PC,
        Mobile
    }

    public enum MobTypeFlag
    {
        Normal = 0,
        HighHp,
        LongDis,
        HighSpeed,
    }
}
