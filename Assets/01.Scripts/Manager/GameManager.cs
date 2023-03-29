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

    // �ӽù���
    public Player Player => _player ??= FindObjectOfType<Player>();
    private Player _player;

    private GameObject hitEffect = null;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "TitleScene")
        {
            _player = null;
            return;
        }

        Rito.Debug.Log("Get Player Instance");
        _player = FindObjectOfType<Player>();

        if(_player == null)
        {
            Rito.Debug.Log("Get Player Instance One more");
            _player = FindObjectOfType<Player>();

            if (_player == null)
            {
                Rito.Debug.Log("Instantiate Player");
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
        hitEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/HitEffect3.prefab");
        InitPlayerInfo();
    }

    private void Start()
    {
        Managers.Pool.CreatePool(hitEffect, 10);
        mapTypeFlag = Define.MapTypeFlag.Ghost;
        Player.playerBase.FragmentAmount = Player.playerBase.FragmentAmount;
    }
    

    public void InitPlayerInfo()
    {
        Player.playerBase.FragmentAmount = PlayerPrefs.GetInt("PlayerFragmentAmount");
        UIManager.Instance.UpdateGoods();
    }

    public void SetStageSceneNum(Define.StageSceneNum sceneNum)
    {
        stageSceneNum = sceneNum;
    }

    //public void PlayHitEffect(Transform objTransform)
    //{
    //    var effect = Managers.Pool.Pop(hitEffect, objTransform);
    //    effect.transform.position = objTransform.position;
    //}

    public void GameQuit()
    {
        Application.Quit();
    }
}
