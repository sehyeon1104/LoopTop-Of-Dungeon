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
        if (!GameManager.Instance.CheckClearTuto())
        {
            Managers.Scene.LoadScene(Define.Scene.Tutorial);
            return;
        }

        GameManager.Instance.SetMapTypeFlag(Define.MapTypeFlag.CenterMap);
        GameManager.Instance.SetSceneType(Define.Scene.Center);
        GameManager.Instance.StageMoveCount = 0;
        GameManager.Instance.SaveData();

        Game.Instance.Init();
    }
}
