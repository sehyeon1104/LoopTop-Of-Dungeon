using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    public static int stageMoveCount { get; private set; } = 0;
    public int StageMoveCount
    {
        get
        {
            return stageMoveCount;
        }
        set
        {
            stageMoveCount = value;
            stageMoveCount %= 3;
        }
    }
    public Define.PlatForm platForm;
    public Define.MapTypeFlag mapTypeFlag; //{ private set; get; }
    public Define.Scene sceneType; //{ private set; get; }

    public Player Player => _player ??= FindObjectOfType<Player>();
    private Player _player;

    [Tooltip("������ �߰�")]
    [field: SerializeField]
    public List<Item> allItemList { get; private set; } = new List<Item>();

    private PlayerData playerData = new PlayerData();
    private GameData gameData = new GameData();
    private ItemData itemData = new ItemData();

    private GameObject hitEffect = null;

    public MinimapCamera minimapCamera { get; private set; } = null;

    private void Awake()
    {
        Application.targetFrameRate = 300;

        if (_player == null)
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

        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            _player = null;
            return;
        }

        minimapCamera = FindObjectOfType<MinimapCamera>();

        #region ���� ������ �ε�
        if (!SaveManager.GetCheckDataBool("GameData"))
        {
            Debug.Log("[GameManager] GameData �������� ����");
            SetGameData();
            SaveManager.Save<GameData>(ref gameData);
        }
        else
        {
            Debug.Log("[GameManager] GameData �������� ����");
            SaveManager.Load<GameData>(ref gameData);
            GetGameData();
        }

        #endregion

        if(_player == null)
        {
            Rito.Debug.Log("_player is null");
            return;
        }
        #region �÷��̾� ���� �ε�
        if (!SaveManager.GetCheckDataBool("PlayerData"))
        {
            Debug.Log("[GameManager] PlayerData �������� ����");
            Player.playerBase.InitPlayerStat();
            SetPlayerStat();
            SaveManager.Save<PlayerData>(ref playerData);
        }
        else
        {
            Debug.Log("[GameManager] PlayerData �������� ����");
            SaveManager.Load<PlayerData>(ref playerData);
            GetPlayerStat();
        }

        if (!SaveManager.GetCheckDataBool("ItemData"))
        {
            Debug.Log("[GameManager] ItemData  �������� ����");
            SetItemData();
            SaveManager.Save<ItemData>(ref itemData);
            InventoryUI.Instance.LoadItemSlot();
        }
        else
        {
            Debug.Log("[GameManager] ItemData �������� ����");
            SaveManager.Load<ItemData>(ref itemData);
            LoadItemData();
            InventoryUI.Instance.LoadItemSlot();
        }

        Player.playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        Player.playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));
        Player.playerBase.PlayerTransformData = Player.playerBase.PlayerTransformDataSOList[(int)playerData.playerTransformTypeFlag];

        #endregion
        Base.Instance.Init();

        hitEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/HitEffect3.prefab");
    }

    private void Start()
    {
        if(_player != null)
        {
            InitPlayerInfo();
            Managers.Pool.CreatePool(hitEffect, 20);
            Player.playerBase.FragmentAmount = Player.playerBase.FragmentAmount;
        }

        // �����
        //SetItemData(allItemList);
    }

    public void ResetStageClearCount()
    {
        StageMoveCount = 0;
    }

    public void InitPlayerInfo()
    {   
        UIManager.Instance.UpdateGoods();
    }

    public void PlayHitEffect(Transform objTransform)
    {
        var effect = Managers.Pool.Pop(hitEffect);
        effect.transform.position = (Vector2)objTransform.position + (Random.insideUnitCircle * 0.5f);
    }

    public void InitSomeStats()
    {
        playerData.maxHp = Player.playerBase.InitMaxHp;
        playerData.hp = Player.playerBase.InitMaxHp;
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
        playerData.slotLevel = Player.playerBase.slotLevel;
        playerData.attack = Player.playerBase.Attack;
        playerData.damage = Player.playerBase.Damage;
        playerData.attackSpeed = Player.playerBase.AttackSpeed;
        playerData.moveSpeed = Player.playerBase.MoveSpeed;
        playerData.critChance = Player.playerBase.CritChance;
        playerData.expTable = Player.playerBase.ExpTable;
        playerData.exp = Player.playerBase.Exp;
        playerData._fragmentAmount = Player.playerBase.FragmentAmount;
        playerData.bossFragmentAmount = Player.playerBase.BossFragmentAmount;
        playerData.playerTransformTypeFlag = Player.playerBase.PlayerTransformTypeFlag;
    }

    /// <summary>
    /// ���ӵ����� ����
    /// </summary>
    public void SetGameData()
    {
        gameData.mapTypeFlag = mapTypeFlag;
        gameData.sceneType = sceneType;
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
        Player.playerBase.slotLevel = playerData.slotLevel;
        Player.playerBase.Attack = playerData.attack;
        Player.playerBase.Damage = playerData.damage;
        Player.playerBase.AttackSpeed = playerData.attackSpeed;
        Player.playerBase.MoveSpeed = playerData.moveSpeed;
        Player.playerBase.CritChance = playerData.critChance;
        Player.playerBase.ExpTable = playerData.expTable;
        Player.playerBase.Exp = playerData.exp;
        Player.playerBase.FragmentAmount = playerData._fragmentAmount;
        Player.playerBase.BossFragmentAmount = playerData.bossFragmentAmount;
        Player.playerBase.PlayerTransformTypeFlag = playerData.playerTransformTypeFlag;
    }

    /// <summary>
    /// ���� ������ �ҷ���
    /// </summary>
    public void GetGameData()
    {
        mapTypeFlag = gameData.mapTypeFlag;
        sceneType = gameData.sceneType;
    }

    public void SetItemData()
    {
        itemData.allItemList = allItemList;
    }

    public void LoadItemData()
    {
        allItemList = itemData.allItemList;
    }

    public void AddItemData(Item item)
    {
        itemData.curItemList.Add(item);
    }

    // �����
    public void SetItemData(List<Item> item)
    {
        itemData.curItemList = item;
        SaveManager.Save<ItemData>(ref itemData);
    }

    public List<Item> GetItemList()
    {
        return itemData.curItemList;
    }

    public void SetMapTypeFlag(Define.MapTypeFlag mapTypeFlag)
    {
        this.mapTypeFlag = mapTypeFlag;
    }
    public void SetSceneType(Define.Scene sceneType)
    {
        this.sceneType = sceneType;
    }

    /// <summary>
    /// �÷��̾� ������ ����
    /// </summary>
    public void SaveData()
    {
        if(_player != null)
        {
            SetPlayerStat();
            SaveManager.Save<PlayerData>(ref playerData);
        }

        SetGameData();
        SaveManager.Save<GameData>(ref gameData);
        SaveManager.Save<ItemData>(ref itemData);
    }

    public void LoadData()
    {
        SaveManager.Load<GameData>(ref gameData);
        GetGameData();

        SaveManager.Load<PlayerData>(ref playerData);
        GetPlayerStat();

        SaveManager.Load<ItemData>(ref itemData);
        LoadItemData();
    }

    public void GameQuit()
    {
        if(sceneType == Define.Scene.CenterScene)
        {
            SaveData();
        }

        Application.Quit();
    }
}
