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

    private PlayerData playerData;

    private GameObject hitEffect = null;

    private void Awake()
    {
        if(_player == null)
        {
            Rito.Debug.Log("Get Player Instance");
            _player = FindObjectOfType<Player>();

            if (_player == null)
            {
                Rito.Debug.Log("Get Player Instance One more");
                _player = FindObjectOfType<Player>();

                if (_player == null)
                {
                    //Rito.Debug.Log("Instantiate Player");
                    //playerPre = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/Player(prototype).prefab");
                    //var temp = Instantiate(playerPre, transform.position, Quaternion.identity);
                    //_player = temp.GetComponent<Player>();
                    Debug.LogError("Can't Get Player Instance");
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

        playerData = new PlayerData();
        if (!SaveManager.GetCheckBool())
        {
            Debug.Log("[GameManager] 저장파일 없음");
            Player.playerBase.InitPlayerStat();
            GetPlayerStat();
            SaveManager.Save<PlayerData>(ref playerData);
        }
        else
        {
            GetPlayerStat();
            Debug.Log("[GameManager] 저장파일 있음");
            SaveManager.Load<PlayerData>(ref playerData);
            LoadPlayerStat();
        }

        if(SceneManager.GetActiveScene().name == "TitleScene")
        {
            _player = null;
            return;
        }

        hitEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/HitEffect3.prefab");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Save");
            Debug.Log(Player.playerBase.Hp);
            GetPlayerStat();
            Debug.Log(playerData._fragmentAmount);
            SaveManager.Save<PlayerData>(ref playerData);
        }
    }

    private void Start()
    {
        InitPlayerInfo();
        Managers.Pool.CreatePool(hitEffect, 10);
        mapTypeFlag = Define.MapTypeFlag.Ghost;
        Player.playerBase.FragmentAmount = Player.playerBase.FragmentAmount;
    }
    

    public void InitPlayerInfo()
    {
        UIManager.Instance.UpdateGoods();
    }

    public void SetStageSceneNum(Define.StageSceneNum sceneNum)
    {
        stageSceneNum = sceneNum;
    }

    public void PlayHitEffect(Transform objTransform)
    {
        var effect = Managers.Pool.Pop(hitEffect);
        effect.transform.position = (Vector2)objTransform.position + (Random.insideUnitCircle * 0.5f);
    }

    public void GetPlayerStat()
    {
        playerData.hp = Player.playerBase.Hp;
        playerData.maxHp = Player.playerBase.MaxHp;
        playerData.level = Player.playerBase.Level;
        playerData.maxLevel = Player.playerBase.MaxLevel;
        playerData.damage = Player.playerBase.Damage;
        playerData.critChance = Player.playerBase.CritChance;
        playerData.exp = Player.playerBase.Exp;
        playerData._fragmentAmount = Player.playerBase.FragmentAmount;
        playerData.playerTransformTypeFlag = Player.playerBase.PlayerTransformTypeFlag;
    }

    public void LoadPlayerStat()
    {
        Player.playerBase.Hp = playerData.hp;
        Player.playerBase.Level = playerData.level;
        Player.playerBase.Damage = playerData.damage;
        Player.playerBase.CritChance = playerData.critChance;
        Player.playerBase.Exp = playerData.exp;
        Player.playerBase.FragmentAmount = playerData._fragmentAmount;
        Player.playerBase.PlayerTransformTypeFlag = playerData.playerTransformTypeFlag;
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
