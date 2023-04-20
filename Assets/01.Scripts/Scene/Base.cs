using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoSingleton<Base>
{
    public Define.Scene SceneType = Define.Scene.Unknown;
    public Define.MapTypeFlag MapType = Define.MapTypeFlag.Default;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        SceneType = GameManager.Instance.sceneType;
        MapType = GameManager.Instance.mapTypeFlag;
    }

    public abstract void Clear();
}
