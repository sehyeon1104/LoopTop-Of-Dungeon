using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Base
{
    protected override void Init()
    {
        base.Init();
        Managers.Sound.Play("Assets/05.Sounds/BGM/TestBGM.mp3", Define.Sound.Bgm);
        SceneType = Define.Scene.CenterScene;
    }
    public override void Clear()
    {

    }
}
