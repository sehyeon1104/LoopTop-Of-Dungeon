using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    public Define.MapTypeFlag mapTypeFlag { private set; get; }
    public Define.StageSceneNum stageSceneNum;

    private GameObject playerPre;


    // 임시방편
    public Player Player => _player ??= FindObjectOfType<Player>();
    private Player _player;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "TitleScene")
        {
            _player = null;
            return;
        }

        Debug.Log("Get Player Instance");
        _player = FindObjectOfType<Player>();

        if(_player == null)
        {
            Debug.Log("Get Player Instance One more");
            _player = FindObjectOfType<Player>();

            if (_player == null)
            {
                Debug.Log("Instantiate Player");
                playerPre = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/Player(prototype).prefab");
                var temp = Instantiate(playerPre, transform.position, Quaternion.identity);
                _player = temp.GetComponent<Player>();
            }
            //else
            //{
            //    players = FindObjectsOfType<Player>();
            //    if (players.Length > 1)
            //    {
            //        for (int i = 1; i < players.Length; ++i)
            //        {
            //            Destroy(players[i]);
            //        }
            //    }
            //}
        }
    }

    private void Start()
    {
        mapTypeFlag = Define.MapTypeFlag.Ghost;
    }

    public void SetStageSceneNum(Define.StageSceneNum sceneNum)
    {
        stageSceneNum = sceneNum;
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
