using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterSceneManager : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Debug.Log("CenterSceneManager Init");
        GameManager.Instance.SetMapTypeFlag(Define.MapTypeFlag.CenterMap);
        GameManager.Instance.SetSceneType(Define.Scene.CenterScene);
        GameManager.Instance.StageMoveCount = 0;
        GameManager.Instance.SaveData();

        Game.Instance.Init();
    }
}
