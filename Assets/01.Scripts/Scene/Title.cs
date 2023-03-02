using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : Base
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.TitleScene;
    }
    public override void Clear()
    {

    }
}
