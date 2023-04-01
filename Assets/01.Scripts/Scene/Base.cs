using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoSingleton<Base>
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;
    public Define.MapTypeFlag MapType = Define.MapTypeFlag.Default;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    public abstract void Clear();
}
