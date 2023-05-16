using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoSingleton<Base>
{
    [SerializeField]
    protected Define.Scene SceneType = Define.Scene.Unknown;
    [SerializeField]
    protected Define.MapTypeFlag MapType = Define.MapTypeFlag.Default;

    public virtual void Init()
    {
        Managers.Sound.Clear();
        SceneType = GameManager.Instance.sceneType;
        MapType = GameManager.Instance.mapTypeFlag;
    }

    public abstract void Clear();
}
