using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Base
{
    [SerializeField] GameObject[] BossType;
    [SerializeField] GameObject[] BossFieldType;
    [SerializeField] GameObject[] BossUI;

    public override void Init()
    {
        base.Init();

        Managers.Sound.Play($"Assets/05.Sounds/BGM/{MapType}/{SceneType}_{MapType}.mp3", Define.Sound.Bgm);
        if(SceneType == Define.Scene.Boss)
        {
            if (BossType[(int)MapType] != null) Instantiate(BossType[(int)MapType]);
            if (BossFieldType[(int)MapType] != null) Instantiate(BossFieldType[(int)MapType]);
            if (BossUI[(int)MapType] != null) Instantiate(BossUI[(int)MapType]);
        }
        switch(MapType)
        {
            case Define.MapTypeFlag.Ghost:
                break;

            case Define.MapTypeFlag.LavaSlime:
                break;

            case Define.MapTypeFlag.Electricity:
                break;

            case Define.MapTypeFlag.Werewolf:
                break;

            case Define.MapTypeFlag.Lizard:
                break;

            case Define.MapTypeFlag.Power:
                break;

            case Define.MapTypeFlag.CenterMap:
                break;

            default:
                break;
        }
    }
    public override void Clear()
    {

    }
}
