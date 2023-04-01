using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Base
{
    protected override void Init()
    {
        base.Init();

        switch(MapType)
        {
            case Define.MapTypeFlag.Ghost:
                if(SceneType == Define.Scene.Ghost_Stage1 || SceneType == Define.Scene.Ghost_Stage2) { }
                else
                    Managers.Sound.Play("Assets/05.Sounds/BGM/Boss_Ghost.mp3", Define.Sound.Bgm, 1, 0.5f);
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
        SceneType = Define.Scene.CenterScene;
    }
    public override void Clear()
    {

    }
}
