using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Base
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.CenterScene;
    }
    public override void Clear()
    {

    }
}
