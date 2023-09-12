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
    private KeySettingData keySettingData = new KeySettingData();
    private ImportantGameData importantGameData = new ImportantGameData();

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
            ItemManager.Instance.Init();
            SetItemData();
            SaveManager.Save<ItemData>(ref itemData);
            InventoryUI.Instance.LoadItemSlot();
        }
        else
        {
            Debug.Log("[GameManager] ItemData �������� ����");
            SaveManager.Load<ItemData>(ref itemData);
            LoadItemData();
            ItemManager.Instance.Init();
        }

        if (!SaveManager.GetCheckDataBool("KeySettingData"))
        {
            Debug.Log("[GameManager] KeySettingData �������� ����");
            KeyManager.Instance.InitKey();
            SaveManager.Save<KeySettingData>(ref keySettingData);
        }
        else
        {
            Debug.Log("[GameManager] KeySettingData �������� ����");
            SaveManager.Load<KeySettingData>(ref keySettingData);
            SetKeyData();
        }

        if (!SaveManager.GetCheckDataBool("ImportantGameData"))
        {
            Debug.Log("[GameManager] ImportantGameData �������� ����");
            SaveManager.Save<ImportantGameData>(ref importantGameData);
        }
        else
        {
            Debug.Log("[GameManager] ImportantGameData �������� ����");
            SaveManager.Load<ImportantGameData>(ref importantGameData);
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
        // ȸ��
        itemRateColor[(int)Define.ItemRating.Common] = "#D3D3D3";
        // �ϴû�
        itemRateColor[(int)Define.ItemRating.Rare] = "#00FFFF";
        // �����
        itemRateColor[(int)Define.ItemRating.Epic] = "#9932CC";
        // ������
        itemRateColor[(int)Define.ItemRating.Legendary] = "#FFA500";
        // ũ����
        itemRateColor[(int)Define.ItemRating.Special] = "#DC143C";
        // ���
        itemRateColor[(int)Define.ItemRating.ETC] = "#FFFFFF";
    }

    private void Start()
    {
        if(_player != null)
        {
            InitPlayerInfo();
            Managers.Pool.CreatePool(hitEffect, 10);
            Managers.Pool.CreatePool(critHitEffect, 10);
            Player.playerBase.FragmentAmount = Player.playerBase.FragmentAmount;
            InventoryUI.Instance.LoadItemSlot();

            MouseManager.Lock(true);
            MouseManager.Show(false);
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

    public void PlayHitEffect(Transform objTransform, bool isCritRate = false , Poolable effect = null)
    {
        //Debug.Log($"��Ʈ ����Ʈ : {hitEffect != null}");
        if (effect == null)
        {
            if (isCritRate)
                effect = Managers.Pool.Pop(critHitEffect);
            else
                effect = Managers.Pool.Pop(hitEffect);
        }
        
        effect.transform.position = (Vector2)objTransform.position + (Random.insideUnitCircle * 0.5f);
        effect.gameObject.SetActive(false);
        effect.gameObject.SetActive(true);
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
        playerData.slotLevel = Player.playerBase.SlotLevel;
        playerData.attack = Player.playerBase.Attack;
        playerData.damage = Player.playerBase.Damage;
        playerData.attackSpeed = Player.playerBase.AttackSpeed;
        playerData.attackRange = Player.playerBase.AttackRange;
        playerData.moveSpeed = Player.playerBase.MoveSpeed;
        playerData.critChance = Player.playerBase.CritChance;
        playerData.critDamage = Player.playerBase.CritDamage;
        playerData.playerSkillNum = Player.playerBase.PlayerSkillNum;
        playerData._fragmentAmount = Player.playerBase.FragmentAmount;
        playerData.bossFragmentAmount = Player.playerBase.BossFragmentAmount;
        playerData.fragmentAddAcq = Player.playerBase.FragmentAddAcq;
        playerData.playerTransformTypeFlag = Player.playerBase.PlayerTransformTypeFlag;
        playerData.skillCoolDown = Player.playerBase.SkillCoolDown;
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
        Player.playerBase.SlotLevel = playerData.slotLevel;
        Player.playerBase.Attack = playerData.attack;
        Player.playerBase.Damage = playerData.damage;
        Player.playerBase.AttackSpeed = playerData.attackSpeed;
        Player.playerBase.AttackRange = playerData.attackRange;
        Player.playerBase.MoveSpeed = playerData.moveSpeed;
        Player.playerBase.CritChance = playerData.critChance;
        Player.playerBase.CritDamage = playerData.critDamage;
        Player.playerBase.PlayerSkillNum = playerData.playerSkillNum;
        Player.playerBase.FragmentAmount = playerData._fragmentAmount;
        Player.playerBase.BossFragmentAmount = playerData.bossFragmentAmount;
        Player.playerBase.FragmentAddAcq = playerData.fragmentAddAcq;
        Player.playerBase.PlayerTransformTypeFlag = playerData.playerTransformTypeFlag;
        Player.playerBase.SkillCoolDown = playerData.skillCoolDown;
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

    // �����
    public void SetItemData(List<Item> item)
    {
        foreach(var items in item)
        {
            ItemManager.Instance.curItemDic.Add(items.itemNameEng, items);
        }
        //itemData.curItemDic = item;
        SaveManager.Save<ItemData>(ref itemData);
    }

    public void SetKeyData()
    {
        KeySetting.keys.Clear();

        for (int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            if (!KeySetting.keys.ContainsValue(keySettingData.keySetting[i]))
                KeySetting.keys.Add((KeyAction)i, keySettingData.keySetting[i]);
        }
    }

    public void SaveKeyData()
    {
        keySettingData.keySetting.Clear();

        foreach (var key in KeySetting.keys.Values)
        {
            keySettingData.keySetting.Add(key);
        }
    }

    public void ClearTuto()
    {
        importantGameData.isClearTuto = true;
    }

    public void GetBossFragment()
    {
        importantGameData.bossFragmentAmount = playerData.bossFragmentAmount;
    }

    public void ObtainCharacter(Define.PlayerTransformTypeFlag playerTransformTypeFlag)
    {
        importantGameData.isObtainBoss[(int)playerTransformTypeFlag] = true;
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
        SaveKeyData();
        SaveManager.Save<GameData>(ref gameData);
        SaveManager.Save<ItemData>(ref itemData);
        SaveManager.Save<KeySettingData>(ref keySettingData);
        SaveManager.Save<ImportantGameData>(ref importantGameData);
    }

    public void LoadData()
    {
        SaveManager.Load<GameData>(ref gameData);
        GetGameData();

        SaveManager.Load<PlayerData>(ref playerData);
        GetPlayerStat();

        SaveManager.Load<ItemData>(ref itemData);
        LoadItemData();

        SaveManager.Load<KeySettingData>(ref keySettingData);
        SetKeyData();

        SaveManager.Load<ImportantGameData>(ref importantGameData);
    }

    public void GameQuit()
    {
        if(sceneType == Define.Scene.Center)
        {
            SaveData();
        }

        Application.Quit();
    }
}
