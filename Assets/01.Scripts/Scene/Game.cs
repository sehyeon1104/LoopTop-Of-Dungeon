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
                if(SceneType == Define.Scene.StageScene)
                    Managers.Sound.Play("Assets/05.Sounds/BGM/Field_Ghost.mp3", Define.Sound.Bgm, 1, 0.5f);
                else if(SceneType == Define.Scene.BossScene)
                    Managers.Sound.Play("Assets/05.Sounds/BGM/Boss_Ghost.mp3", Define.Sound.Bgm, 1, 0.5f);
                else
                {

                }

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
