using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    public Define.MapTypeFlag mapTypeFlag { private set; get; }
    public Define.StageSceneNum stageSceneNum;

    public Player Player => _player ??= FindObjectOfType<Player>();
    private Player _player;

    private PlayerData playerData = new PlayerData();

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
                    Rito.Debug.LogError("Can't Get Player Instance");
                }
            }
        }

        #region �÷��̾� ���� �ε�
        if (!SaveManager.GetCheckBool())
        {
            Rito.Debug.Log("[GameManager] �������� ����");
            Player.playerBase.InitPlayerStat();
            SetPlayerStat();
            SaveManager.Save<PlayerData>(ref playerData);
        }
        else
        {
            Rito.Debug.Log("[GameManager] �������� ����");
            SaveManager.Load<PlayerData>(ref playerData);
            GetPlayerStat();
        }

        Player.playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        Player.playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));
        Player.playerBase.PlayerTransformData = Player.playerBase.PlayerTransformDataSOList[(int)playerData.playerTransformTypeFlag];

        #endregion

        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            _player = null;
            return;
        }

        hitEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/HitEffect3.prefab");
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

    /// <summary>
    /// playerData�� ���� �÷��̾� ���� ����
    /// </summary>
    public void SetPlayerStat()
    {
        playerData.maxHp = Player.playerBase.MaxHp;
        playerData.hp = Player.playerBase.Hp;
        playerData.maxLevel = Player.playerBase.MaxLevel;
        playerData.level = Player.playerBase.Level;
        playerData.damage = Player.playerBase.Damage;
        playerData.critChance = Player.playerBase.CritChance;
        playerData.expTable = Player.playerBase.ExpTable;
        playerData.exp = Player.playerBase.Exp;
        playerData._fragmentAmount = Player.playerBase.FragmentAmount;
        playerData.playerTransformTypeFlag = Player.playerBase.PlayerTransformTypeFlag;
    }

    /// <summary>
    /// ���� �÷��̾� ������ playerData �ҷ���
    /// </summary>
    public void GetPlayerStat()
    {
        Player.playerBase.MaxHp = playerData.maxHp;
        Player.playerBase.Hp = playerData.hp;
        Player.playerBase.MaxLevel = playerData.maxLevel;
        Player.playerBase.Level = playerData.level;
        Player.playerBase.Damage = playerData.damage;
        Player.playerBase.CritChance = playerData.critChance;
        Player.playerBase.ExpTable = playerData.expTable;
        Player.playerBase.Exp = playerData.exp;
        Player.playerBase.FragmentAmount = playerData._fragmentAmount;
        Player.playerBase.PlayerTransformTypeFlag = playerData.playerTransformTypeFlag;
    }

    /// <summary>
    /// �÷��̾� ������ ����
    /// </summary>
    public void SavePlayerStat()
    {
        SetPlayerStat();
        SaveManager.Save<PlayerData>(ref playerData);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
