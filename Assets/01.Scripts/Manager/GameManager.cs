using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Define.MapTypeFlag mapTypeFlag { private set; get; }

    private void Start()
    {
        mapTypeFlag = Define.MapTypeFlag.Ghost;
    }


    public void GameQuit()
    {
        Application.Quit();
    }

}
