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

    private PlayerData playerData = new PlayerData();
    private GameData gameData = new GameData();
    private ItemData itemData = new ItemData();

    private GameObject hitEffect = null;
    private GameObject critHitEffect = null;

    public MinimapCamera minimapCamera { get; private set; } = null;

    public Dictionary<int, string> itemRateColor { get; private set; } = new Dictionary<int, string>();

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

        #region 게임 데이터 로딩
        if (!SaveManager.GetCheckDataBool("GameData"))
        {
            Debug.Log("[GameManager] GameData 저장파일 없음");
            SetGameData();
            SaveManager.Save<GameData>(ref gameData);
        }
        else
        {
            Debug.Log("[GameManager] GameData 저장파일 있음");
            SaveManager.Load<GameData>(ref gameData);
            GetGameData();
        }

        #endregion

        if(_player == null)
        {
            Rito.Debug.Log("_player is null");
            return;
        }
        #region 플레이어 정보 로딩
        if (!SaveManager.GetCheckDataBool("PlayerData"))
        {
            Debug.Log("[GameManager] PlayerData 저장파일 없음");
            Player.playerBase.InitPlayerStat();
            SetPlayerStat();
            SaveManager.Save<PlayerData>(ref playerData);
        }
        else
        {
            Debug.Log("[GameManager] PlayerData 저장파일 있음");
            SaveManager.Load<PlayerData>(ref playerData);
            GetPlayerStat();
        }

        ItemManager.Instance.Init();
        if (!SaveManager.GetCheckDataBool("ItemData"))
        {
            Debug.Log("[GameManager] ItemData  저장파일 없음");
            SetItemData();
            SaveManager.Save<ItemData>(ref itemData);
            InventoryUI.Instance.LoadItemSlot();
        }
        else
        {
            Debug.Log("[GameManager] ItemData 저장파일 있음");
            SaveManager.Load<ItemData>(ref itemData);
            LoadItemData();
        }

        Player.playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Power.asset"));
        Player.playerBase.PlayerTransformDataSOList.Add(Managers.Resource.Load<PlayerSkillData>("Assets/07.SO/Player/Ghost.asset"));
        Player.playerBase.PlayerTransformData = Player.playerBase.PlayerTransformDataSOList[(int)playerData.playerTransformTypeFlag];

        #endregion
        Base.Instance.Init();

        hitEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/HitEffect3.prefab");
        critHitEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/DeathHitEffect.prefab");

        InitItemColorDic();
    }

    private void InitItemColorDic()
    {
        itemRateColor.Clear();
        // 회색
        itemRateColor[(int)Define.ItemRating.Common] = "#D3D3D3";
        // 하늘색
        itemRateColor[(int)Define.ItemRating.Rare] = "#00FFFF";
        // 보라색
        itemRateColor[(int)Define.ItemRating.Epic] = "#9932CC";
        // 빨간색
        itemRateColor[(int)Define.ItemRating.Legendary] = "#FFA500";
        // 크림슨
        itemRateColor[(int)Define.ItemRating.Special] = "#DC143C";
        // 흰색
        itemRateColor[(int)Define.ItemRating.ETC] = "#FFFFFF";
    }

    private void Start()
    {
        if(_player != null)
        {
            InitPlayerInfo();
            Managers.Pool.CreatePool(hitEffect, 20);
            Managers.Pool.CreatePool(critHitEffect, 20);
            Player.playerBase.FragmentAmount = Player.playerBase.FragmentAmount;
            InventoryUI.Instance.LoadItemSlot();

            MouseManager.Lock(true);
            MouseManager.Show(false);
        }
        // 디버깅
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

    public void PlayHitEffect(Transform objTransform, bool isCritRate = false , Poolable effect = null)
    {
        //Debug.Log($"히트 이펙트 : {hitEffect != null}");
        if (effect == null)
        {
            if (isCritRate)
                effect = Managers.Pool.Pop(critHitEffect);
            else
                effect = Managers.Pool.Pop(hitEffect);
        }
        
        effect.transform.position = (Vector2)objTransform.position + (Random.insideUnitCircle * 0.5f);
    }

    public void InitSomeStats()
    {
        playerData.maxHp = Player.playerBase.InitMaxHp;
        playerData.hp = Player.playerBase.InitMaxHp;
    }

    /// <summary>
    /// playerData에 현재 플레이어 정보 저장
    /// </summary>
    public void SetPlayerStat()
    {
        playerData.maxHp = Player.playerBase.MaxHp;
        playerData.hp = Player.playerBase.Hp;
        playerData.slotLevel = Player.playerBase.SlotLevel;
        playerData.attack = Player.playerBase.Attack;
        playerData.damage = Player.playerBase.Damage;
        playerData.attackSpeed = Player.playerBase.AttackSpeed;
        playerData.attackRange = Player.playerBase.AttackRange;
        playerData.moveSpeed = Player.playerBase.MoveSpeed;
        playerData.critChance = Player.playerBase.CritChance;
        playerData.playerSkillNum = Player.playerBase.PlayerSkillNum;
        playerData._fragmentAmount = Player.playerBase.FragmentAmount;
        playerData.bossFragmentAmount = Player.playerBase.BossFragmentAmount;
        playerData.fragmentAddAcq = Player.playerBase.FragmentAddAcq;
        playerData.playerTransformTypeFlag = Player.playerBase.PlayerTransformTypeFlag;
        playerData.coolDown = Player.playerBase.coolDown;
    }

    /// <summary>
    /// 게임데이터 저장
    /// </summary>
    public void SetGameData()
    {
        gameData.mapTypeFlag = mapTypeFlag;
        gameData.sceneType = sceneType;
    }

    /// <summary>
    /// 현재 플레이어 정보에 playerData 불러옴
    /// </summary>
    public void GetPlayerStat()
    {
        Player.playerBase.MaxHp = playerData.maxHp;
        Player.playerBase.Hp = playerData.hp;
        Player.playerBase.SlotLevel = playerData.slotLevel;
        Player.playerBase.Attack = playerData.attack;
        Player.playerBase.Damage = playerData.damage;
        Player.playerBase.AttackSpeed = playerData.attackSpeed;
        Player.playerBase.AttackRange = playerData.attackRange;
        Player.playerBase.MoveSpeed = playerData.moveSpeed;
        Player.playerBase.CritChance = playerData.critChance;
        Player.playerBase.PlayerSkillNum = playerData.playerSkillNum;
        Player.playerBase.FragmentAmount = playerData._fragmentAmount;
        Player.playerBase.BossFragmentAmount = playerData.bossFragmentAmount;
        Player.playerBase.FragmentAddAcq = playerData.fragmentAddAcq;
        Player.playerBase.PlayerTransformTypeFlag = playerData.playerTransformTypeFlag;
        Player.playerBase.coolDown = playerData.coolDown;
    }

    /// <summary>
    /// 게임 데이터 불러옴
    /// </summary>
    public void GetGameData()
    {
        mapTypeFlag = gameData.mapTypeFlag;
        sceneType = gameData.sceneType;
    }

    public void SetItemData()
    {
        foreach(Item item in ItemManager.Instance.allItemDic.Values)
        {
            itemData.allItemList.Add(item);
        }
        //itemData.allItemList = ItemManager.Instance.allItemList;
    }

    public void LoadItemData()
    {
        ItemManager.Instance.SetAllItemDic(itemData.allItemList);
        ItemManager.Instance.SetCurItemDic(itemData.curItemList);
        //ItemManager.Instance.allItemList = itemData.allItemList;
    }

    // 디버깅
    public void SetItemData(List<Item> item)
    {
        foreach(var items in item)
        {
            ItemManager.Instance.curItemDic.Add(items.itemNameEng, items);
        }
        //itemData.curItemDic = item;
        SaveManager.Save<ItemData>(ref itemData);
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
    /// 플레이어 데이터 저장
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
