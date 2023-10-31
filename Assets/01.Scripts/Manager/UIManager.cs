using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Linq;
using System;
using UnityEngine.Playables;
using UnityEngine.VFX;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoSingleton<UIManager>
{
    Transform player;
    Animator animator;
    PlayableDirector[] ults;
    VisualEffect[] clawEffect;
    float currentSpeed;
    public GameObject playerUI;
    public GameObject playerPCUI;
    public GameObject playerPPUI;

    public GameObject ultFade;
    [Header("LeftUp")]
    [SerializeField]
    private GameObject hpPrefab;
    [SerializeField]
    private GameObject hpSpace;
    [SerializeField]
    private TextMeshProUGUI fragmentAmountTMP = null;
    [SerializeField]
    private TextMeshProUGUI bossFragmentAmountTMP = null;

    [Header("LeftDown")]
    [SerializeField]
    private Transform playerItemListUI = null;
    [SerializeField]
    private GameObject itemUITemplate = null;

    [Header("Middle")]

    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject checkOneMorePanel;
    
    [SerializeField]
    private Button resumeBtn;
    [SerializeField]
    private Button settingBtn;
    [SerializeField]
    private Button quitBtn;

    private Slider MasterVolume;
    private Slider SfxVolume;
    private Slider BgmVolume;

    [SerializeField]
    private GameObject obtainItemInfo = null;
    [SerializeField]
    private Image obtainItemInfoImg = null;
    [SerializeField]
    private TextMeshProUGUI obtainItemInfoTMP = null;
    private Vector3 obtainItemInfoScale;
    private bool isShowObtainItemInfo = false;
    private Queue<Item> obtainItemQueue = new Queue<Item>();

    [SerializeField]
    private GameObject showCurStageNameObj;
    [SerializeField]
    private TextMeshProUGUI curStageName;
    [SerializeField]
    private Image curStageNameLine;
    [SerializeField]
    private float showCurStageNameTime = 3f;
    [Header("RightUp")]
    private Transform minimap;
    [Header("RightDown")]
    public GameObject skill1Button;
    public GameObject skill2Button;
    public GameObject ultButton;
    public GameObject dashButton;

    private TextMeshProUGUI skill1Tmp;
    private TextMeshProUGUI skill2Tmp;
    private TextMeshProUGUI ultTmp;

    public Button reviveButton;
    public Button leaveButton;

    Image[] skillIcons = new Image[5];
    Image[] pcSkillIcons = new Image[5];

    GameObject CoolDownedParticle;

    GameObject AttackButton;
    GameObject InteractionButton;

    [SerializeField]
    private GameObject blurPanel;
    public List<Heart> avcList = new List<Heart>();
    public List<Image> hpbars = new List<Image>();
    public Image hpBar;
    public TextMeshProUGUI hpText;
    public PlayerSkill playerskill;
    private int maxHpCount = 0;

    private WaitForEndOfFrame waitForEndOfFrame;
    PlayerBase playerBase;
    public ShopUI shopUI { private set; get; } = null;
    public GameOverUI gameOverUI { get; private set; } = null;
    public float[] currentFillAmount;
    private float nowTimeScale;
    public bool isSetting { get; private set; } = false;

    private void Awake()
    {
        playerBase = GameManager.Instance.Player.playerBase;
        currentFillAmount = new float[10];
        playerUI = GameObject.Find("PlayerUI").gameObject;
        hpPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/Heart.prefab");
        playerskill = FindObjectOfType<PlayerSkill>();  
        AttackButton = playerUI.transform.Find("RightDown/Btns/AttackBtn").gameObject;
        skill1Button = playerUI.transform.Find("RightDown/Btns/Skill1_Btn").gameObject;
        skill2Button = playerUI.transform.Find("RightDown/Btns/Skill2_Btn").gameObject;
        ultButton = playerUI.transform.Find("RightDown/Btns/UltimateSkill_Btn").gameObject;
        dashButton = playerUI.transform.Find("RightDown/Btns/Dash_Btn").gameObject;
        InteractionButton = playerUI.transform.Find("RightDown/Btns/Interaction_Btn").gameObject;
        player = GameManager.Instance.Player.transform; 
        if (GameManager.Instance.platForm == Define.PlatForm.Mobile)
        {
            hpSpace = playerUI.transform.Find("LeftUp/PlayerHP").gameObject;
            fragmentAmountTMP = playerUI.transform.Find("LeftUp/Goods/ExperienceFragmentUI/FragmentAmountTMP").GetComponent<TextMeshProUGUI>();
            bossFragmentAmountTMP = playerUI.transform.Find("LeftUp/Goods/BossFragmentUI/BossFragmentAmountTMP").GetComponent<TextMeshProUGUI>();
            pausePanel = playerUI.transform.Find("Middle/PausePanel").gameObject;
            gameOverPanel = playerUI.transform.Find("All/GameOverPanel").gameObject;
            checkOneMorePanel = playerUI.transform.Find("Middle/CheckOneMorePanel").gameObject;
            showCurStageNameObj = playerUI.transform.Find("Middle/ShowCurStageName").gameObject;
            curStageName = showCurStageNameObj.transform.Find("CurStageName").GetComponent<TextMeshProUGUI>();
            curStageNameLine = showCurStageNameObj.transform.Find("Line").GetComponent<Image>();
            blurPanel = playerUI.transform.Find("All/BlurPanel").gameObject;
            reviveButton = playerUI.transform.Find("All/GameOverPanel/Panel/Btns/Revive").GetComponent<Button>();
            leaveButton = playerUI.transform.Find("All/GameOverPanel/Panel/Btns/Leave").GetComponent<Button>(); 
            skillIcons[0] = skill1Button.transform.Find("ShapeFrame/Icon").GetComponent<Image>();
            skillIcons[1] = skill2Button.transform.Find("ShapeFrame/Icon").GetComponent<Image>();
            skillIcons[2] = ultButton.transform.Find("ShapeFrame/Icon").GetComponent<Image>();
            skillIcons[3] = dashButton.transform.Find("ShapeFrame/Icon").GetComponent<Image>();
            skillIcons[4] = playerUI.transform.Find("RightDown/Btns/Dash_Btn/ShapeFrame/Icon").GetComponent<Image>();
        }
        else
        {
            playerPCUI = GameObject.Find("PCPlayerUI").gameObject;
            playerPPUI = GameObject.Find("PPPlayerUI").gameObject;
            ultFade = playerPCUI.transform.Find("UltFade").gameObject;
            hpSpace = ultFade.transform.Find("LeftDown/NewPlayerHp").gameObject;
            playerItemListUI = ultFade.transform.Find("LeftDown/PlayerItemList");

            fragmentAmountTMP = ultFade.transform.Find("LeftUp/Goods/ExperienceFragmentUI/FragmentAmountTMP").GetComponent<TextMeshProUGUI>();
            bossFragmentAmountTMP = ultFade.transform.Find("LeftUp/Goods/BossFragmentUI/BossFragmentAmountTMP").GetComponent<TextMeshProUGUI>();

            pcSkillIcons[0] = playerPPUI.transform.Find("LeftDown/Btns/Skill1_Btn/ShapeFrame/Icon").GetComponent<Image>();
            pcSkillIcons[1] = playerPPUI.transform.Find("LeftDown/Btns/Skill2_Btn/ShapeFrame/Icon").GetComponent<Image>();
            pcSkillIcons[2] = playerPPUI.transform.Find("LeftDown/Btns/UltimateSkill_Btn/ShapeFrame/Icon").GetComponent<Image>();
            pcSkillIcons[3] = playerPPUI.transform.Find("LeftDown/Btns/Dash_Btn/ShapeFrame/Icon").GetComponent<Image>();
            pcSkillIcons[4] = playerPPUI.transform.Find("LeftDown/Btns/Attack_Btn/ShapeFrame/Icon").GetComponent<Image>();

            CoolDownedParticle = playerPPUI.transform.Find("LeftDown/Btns/CoolDowned").gameObject;

            gameOverPanel = ultFade.transform.Find("All/GameOverPanel").gameObject;
            blurPanel = ultFade.transform.Find("All/BlurPanel").gameObject;
            reviveButton = ultFade.transform.Find("All/GameOverPanel/Panel/Btns/Revive").GetComponent<Button>();
            leaveButton = ultFade.transform.Find("All/GameOverPanel/Panel/Btns/Leave").GetComponent<Button>();
            resumeBtn = playerPCUI.transform.Find("Middle/PausePanel/Panel/Btns/Resume").GetComponent<Button>();
            settingBtn = playerPCUI.transform.Find("Middle/PausePanel/Panel/Btns/Setting").GetComponent<Button>();
            settingPanel = playerPCUI.transform.Find("SettingPanel").gameObject;
            MasterVolume = settingPanel.transform.Find("Panel/Volume/MasterVolume").GetComponentInChildren<Slider>();
            SfxVolume = settingPanel.transform.Find("Panel/Volume/EffectVolume").GetComponentInChildren<Slider>();
            BgmVolume = settingPanel.transform.Find("Panel/Volume/BGMVolume").GetComponentInChildren<Slider>();
            pausePanel = playerPCUI.transform.Find("Middle/PausePanel").gameObject;
            quitBtn = playerPCUI.transform.Find("Middle/PausePanel/Panel/Btns/Quit").GetComponent<Button>();
            checkOneMorePanel = playerPCUI.transform.Find("Middle/CheckOneMorePanel").gameObject;
            showCurStageNameObj = playerPCUI.transform.Find("Middle/ShowCurStageName").gameObject;
            obtainItemInfo = playerPCUI.transform.Find("Middle/ObtainItemInfo").gameObject;
            obtainItemInfoImg = obtainItemInfo.transform.Find("ItemImg").GetComponent<Image>();
            obtainItemInfoTMP = obtainItemInfo.transform.Find("ItemNameTMP").GetComponent<TextMeshProUGUI>();
            minimap = ultFade.transform.Find("Minimap");
            curStageName = showCurStageNameObj.transform.Find("CurStageName").GetComponent<TextMeshProUGUI>();
            curStageNameLine = showCurStageNameObj.transform.Find("Line").GetComponent<Image>();

            hpBar = hpSpace.GetComponentInChildren<Heart>().transform.Find("HPImg").GetComponent<Image>();
            hpText = hpBar.GetComponentInChildren<TextMeshProUGUI>();
        }
        itemUITemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/ItemUI.prefab");
        ults = player.GetComponentsInChildren<PlayableDirector>();
         clawEffect = player.GetComponentsInChildren<VisualEffect>(true);
        animator = player.Find("Skill/GhostUlt/GhostBossUlt/GhostBoss").GetComponent<Animator>();

        InitBtns();

        shopUI = FindObjectOfType<ShopUI>();
        gameOverUI = FindObjectOfType<GameOverUI>();
        skill1Tmp = playerPPUI.transform.Find("LeftDown/Btns/Skill1_Btn/KeyCodeBack/SkillKeyTMP").GetComponent<TextMeshProUGUI>();
        skill2Tmp = playerPPUI.transform.Find("LeftDown/Btns/Skill2_Btn/KeyCodeBack/SkillKeyTMP").GetComponent<TextMeshProUGUI>();
        ultTmp = playerPPUI.transform.Find("LeftDown/Btns/UltimateSkill_Btn/KeyCodeBack/SkillKeyTMP").GetComponent<TextMeshProUGUI>();

        waitForEndOfFrame = new WaitForEndOfFrame();
    }

    public void InitBtns()
    {
        leaveButton.onClick.RemoveListener(LeaveBtn);
        leaveButton.onClick.AddListener(LeaveBtn);
        resumeBtn.onClick.RemoveListener(Resume);
        resumeBtn.onClick.AddListener(Resume);
        settingBtn.onClick.RemoveListener(ToggleSettingPanel);
        settingBtn.onClick.AddListener(ToggleSettingPanel);
        quitBtn.onClick.RemoveListener(LeaveBtn);
        quitBtn.onClick.AddListener(LeaveBtn);
        MasterVolume.onValueChanged.AddListener(delegate { SetVolume(); });
        SfxVolume.onValueChanged.AddListener(delegate { SetVolume(); });
        BgmVolume.onValueChanged.AddListener(delegate { SetVolume(); });
    }

    private void Start()
    {
        obtainItemInfoScale = obtainItemInfo.transform.localScale;
        //HPInit();
        UpdateUI();
        DisActiveAllPanels();
        if(SceneManager.GetActiveScene().name == "CenterScene")
        {
            minimap.gameObject.SetActive(false);
        }
        RotateAttackButton();
    }

    public void UpdateUI()
    {
        //MaxHpUpdate();
        //HpUpdate();
        NewHpUpdate();
        UpdateGoods();
        UpdateSkillKey();
        SetVolume();
    }

    public void HPInit()
    {
        foreach (var avc in hpSpace.GetComponentsInChildren<Heart>())
        {
            avcList.Add(avc);
            hpbars.Add(avc.transform.Find("HeartImg").GetComponent<Image>());
            avc.gameObject.SetActive(false);
        }
    }

    public void SetVolume()
    {
        Managers.Instance.MasterVolume = MasterVolume.value;
        Managers.Instance.SfxVolume = SfxVolume.value;
        Managers.Instance.BgmVolume = BgmVolume.value;
    }
    

    public void TogglePlayerAttackUI()
    {
        AttackButton.SetActive(!AttackButton.activeSelf);
        skill1Button.SetActive(!skill1Button.activeSelf);
        skill2Button.SetActive(!skill2Button.activeSelf);
        ultButton.SetActive(!ultButton.activeSelf);
    }

    public void UpdateSkillKey()
    {
        skill1Tmp.SetText($"{KeyStringException(KeySetting.keys[KeyAction.SKILL1].ToString())}");
        skill2Tmp.SetText($"{KeyStringException(KeySetting.keys[KeyAction.SKILL2].ToString())}");
        ultTmp.SetText($"{KeyStringException(KeySetting.keys[KeyAction.ULTIMATE].ToString())}");
    }

    private string KeyStringException(string key)
    {
        string str = string.Empty;
        str += key;
        if (str == "Mouse0")
            str = "M1";
        else if (str == "Mouse1")
            str = "M2";
        else if (str == "Mouse2")
            str = "M3";
        else if (str == "Space")
            str = "Spc";

        return str;
    }


    #region Panels
    private Stack<GameObject> panelStack = new Stack<GameObject>();
    private Dictionary<string, bool> panelDic = new Dictionary<string, bool>();

    /// <summary>
    /// 패널 활성화
    /// </summary>
    /// <param name="panel"></param>
    public void PushPanel(GameObject panel)
    {
        panel.SetActive(true);
        if (panelDic.ContainsKey(panel.name))
            return;

        panelDic.Add(panel.name, true);
        panelStack.Push(panel);
    }

    public Stack<GameObject> GetPanelStack()
    {
        return panelStack;
    }

    public void DisActiveAllPanels()
    {
        blurPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        checkOneMorePanel.SetActive(false);
        InventoryUI.Instance.transform.Find("Background").gameObject.SetActive(false);
    }

    public void ToggleSettingPanel()
    {
        PushPanel(settingPanel);
    }

    /// <summary>
    /// 패널 비활성화
    /// </summary>
    public void PopPanel()
    {
        PlayerMovement.Instance.IsControl = true;
        panelDic.Remove(panelStack.Peek().name);
        if (panelStack.Peek().name == "PausePanel")
        {
            TogglePausePanel();
        }

        panelStack.Pop().SetActive(false);
    }

    public void TogglePausePanel()
    {
        isSetting = !pausePanel.activeSelf;
        blurPanel.SetActive(!pausePanel.activeSelf);
        pausePanel.SetActive(!pausePanel.activeSelf);
        if (pausePanel.activeSelf)
        {
            nowTimeScale = Time.timeScale;

            for(int i = 0; i < ults.Length; i++)
            {
                ults[i].timeUpdateMode = DirectorUpdateMode.GameTime;
            }
            currentSpeed = clawEffect[0].playRate;
            for (int i=0; i <clawEffect.Length; i++)
            {
                clawEffect[i].playRate = 0;
            }
            animator.updateMode = AnimatorUpdateMode.Normal;
            Time.timeScale = 0f;
            MouseManager.Lock(false);
            MouseManager.Show(true);
        }
        else
        {

            for (int i = 0; i < ults.Length; i++)
            {
                ults[i].timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
            }
            for (int i = 0; i < clawEffect.Length; i++)
            {
                clawEffect[i].playRate = currentSpeed;
            }

            Time.timeScale = nowTimeScale;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            MouseManager.Lock(true);
            MouseManager.Show(false);
        }
    }

    public void Resume()
    {
        TogglePausePanel();
    }
   
    public void ToggleGameOverPanel()
    {
        TogglePlayerAttackUI();
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
        MouseManager.Show(gameOverPanel.activeSelf);
        MouseManager.Lock(!gameOverPanel.activeSelf);
    }
    public void CloseGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }
    public void ToggleCheckOneMorePanel()
    {
        checkOneMorePanel.SetActive(!checkOneMorePanel.activeSelf);
    }
    #endregion

    public void UpdateGoods()
    {
        fragmentAmountTMP.SetText(GameManager.Instance.Player.playerBase.FragmentAmount.ToString());
        bossFragmentAmountTMP.SetText(GameManager.Instance.GetBossFragmentAmount().ToString());
    }

    #region GameOver
    public void Revive()
    {
        // TODO : 광고시청 후 부활 or 재화소모 후 부활 구현

        Debug.Log("Revive");
        ToggleGameOverPanel();
        GameManager.Instance.Player.RevivePlayer();
    }

    public void Leave()
    {
        ToggleCheckOneMorePanel();
    }

    public void CheckOneMorePanelYes()
    {

        LoadToCenterScene();
    }

    public void CheckOneMorePanelNo()
    {
        ToggleCheckOneMorePanel();
    }
    #endregion

    #region Item
    //public void AddItemListUI(Item item)
    //{
    //    if(item.itemType == Define.ItemType.heal)
    //    {
    //        return;
    //    }

    //    GameObject itemUI = Instantiate(itemUITemplate, playerItemListUI);
    //    Image itemIcon = itemUI.transform.Find("ItemIcon").GetComponent<Image>();
    //    itemIcon.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
    //}

    public IEnumerator ShowObtainItemInfo(Item item)
    {
        if (!isShowObtainItemInfo)
        {
            isShowObtainItemInfo = true;

            obtainItemInfo.SetActive(true);
            obtainItemInfo.transform.localScale = new Vector3(obtainItemInfoScale.x * 0.25f, obtainItemInfoScale.y * 0.25f, 0);
            obtainItemInfoImg.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
            obtainItemInfoTMP.text = $"<color={GameManager.Instance.itemRateColor[(int)item.itemRating]}>{item.itemName}</color>";
            obtainItemInfo.transform.DOScale(obtainItemInfoScale, 0.4f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
            obtainItemInfo.transform.DOScale(new Vector3(obtainItemInfoScale.x * 0.2f, obtainItemInfoScale.y * 0.2f, 0), 0.3f).SetEase(Ease.OutCirc);
            yield return new WaitForSeconds(0.3f);
            obtainItemInfo.SetActive(false);
            obtainItemInfo.transform.localScale = obtainItemInfoScale;

            isShowObtainItemInfo = false;
        }
        else if (isShowObtainItemInfo)
        {
            obtainItemQueue.Enqueue(item);
            while(obtainItemQueue.Count != 0)
            {
                yield return new WaitUntil(() => !isShowObtainItemInfo);
                if(obtainItemQueue.Count == 0)
                {
                    break;
                }
                StartCoroutine(ShowObtainItemInfo(obtainItemQueue.Dequeue()));

                yield return waitForEndOfFrame;
            }
        }
    }
    #endregion


    public bool SkillCooltime(PlayerSkillData skillData,int skillNum , bool isCheck = false)
    {
        float coolTime = skillData.skill[skillNum].skillDelay;
        float skillCoolTime = coolTime - (coolTime * playerBase.SkillCoolDown / 100);
        int num =skillNum; 
        if (skillNum ==7)
            num = 2;
        else if (skillNum == 8)
            num = 3;
        else
            num = skillNum == playerskill.skillIndex[0] ? 0 : 1;

        Image currentImage = null;
        if (GameManager.Instance.platForm == Define.PlatForm.Mobile)
        {
            GameObject touchedObj = EventSystem.current.currentSelectedGameObject;
            currentImage = touchedObj.transform.Find("CooltimeImg").GetComponent<Image>();
            if (currentImage.fillAmount > 0)
                return false;
        }
        else
        {
            currentImage = playerPPUI.transform.Find("LeftDown/Btns").GetChild(num).transform.Find("CooltimeImg").GetComponent<Image>();

            if (currentImage.fillAmount > 0)
                return false;
        }
        if(!isCheck)
        StartCoroutine(IESkillCooltime(num, currentImage, skillCoolTime));

        return true;
    }
    public void SkillCoolCalculation(float time, int num)
    {
        if (num == 3) return; 
        int skillNum = playerskill.skillIndex[num];
        currentFillAmount[num] -= time / playerBase.PlayerTransformData.skill[skillNum].skillDelay; 
    }
    public void SkillCoolRedution(int num)
    {
        currentFillAmount[num] = 0.01f;
    }
    public IEnumerator IESkillCooltime(int num, Image cooltimeImg, float skillCooltime)
    {
        currentFillAmount[num] = 1f;
        cooltimeImg.fillAmount = 1f;
        GameManager.Instance.Player.SkillRelatedItemEffects.Invoke(num);
        while (cooltimeImg.fillAmount > 0)
        {
            currentFillAmount[num] -= Time.deltaTime / skillCooltime;
            cooltimeImg.fillAmount = currentFillAmount[num];
            yield return null;
        }
        if (num == 3) yield break;
        CoolDownedParticle.GetComponent<RectTransform>().position = cooltimeImg.rectTransform.position;
        CoolDownedParticle.SetActive(false);
        CoolDownedParticle.SetActive(true);
    }

    public void ResetSkill()
    {
        if (GameManager.Instance.platForm == Define.PlatForm.PC)
        {
            for (int i = 0; i < pcSkillIcons.Length; i++)
            {
                pcSkillIcons[i].sprite = null;
            }
        }
        else
        {
            for (int i = 0; i < skillIcons.Length; i++)
            {
                skillIcons[i].sprite = null;
            }

        }
    }   
    public void SetSkillIcon(PlayerSkillData skilldata, int iconNum, int skillNum, int spriteNum)
    {
        if (GameManager.Instance.platForm == Define.PlatForm.PC)
        {
            if (iconNum == 0 && pcSkillIcons[0].sprite != null)
                iconNum++;


            pcSkillIcons[iconNum].sprite = skilldata.skill[skillNum].skillIcon[spriteNum];
        }
        else
        {

            if (iconNum == 0 && skillIcons[0].sprite != null)
                iconNum++;

            skillIcons[iconNum].sprite = skilldata.skill[skillNum].skillIcon[spriteNum];
        }
    }
    public void MaxHpUpdate()
    {
        maxHpCount = Mathf.CeilToInt((float)GameManager.Instance.Player.playerBase.MaxHp / 4);
        for (int i = 0; i < maxHpCount; i++)
        {
            avcList[i].gameObject.SetActive(true);
        }
        HpUpdate();
    }
    public void HpUpdate()
    {
        for (int i = 0; i < maxHpCount; i++)
        {
            hpbars[i].fillAmount = (GameManager.Instance.Player.playerBase.Hp * 0.25f) - i;
        }

    }
    public void NewHpUpdate()
    {
        if (hpBar == null) return;
        hpBar.fillAmount = ((float)GameManager.Instance.Player.playerBase.Hp) / ((float)GameManager.Instance.Player.playerBase.MaxHp);
        hpText.text = $"{GameManager.Instance.Player.playerBase.Hp}<color=#aeaeae><size=90%>/{GameManager.Instance.Player.playerBase.MaxHp}";
    }

    public void RotateInteractionButton()
    {
        AttackButton.SetActive(false);
        InteractionButton.SetActive(true);
    }

    public void RotateAttackButton()
    {
        AttackButton.SetActive(true);
        InteractionButton.SetActive(false);
    }

    public bool IsActiveAttackBtn()
    {
        return AttackButton.activeSelf;
    }

    public Button GetInteractionButton()
    {
        return InteractionButton.GetComponent<Button>();
    }

    public IEnumerator ShowCurrentStageName()
    {
        if (showCurStageNameObj.gameObject == null)
        {
            Debug.LogWarning("showCurStageNameObj is Null!");
            yield break;
        }
        if (GameManager.Instance.sceneType == Define.Scene.Center || GameManager.Instance.sceneType == Define.Scene.Boss)
        {
            yield break;
        }

        showCurStageNameObj.SetActive(true);

        // 현재 스테이지 이름 표기
        curStageName.SetText(string.Format("{0}Stage_{1}", GameManager.Instance.mapTypeFlag.ToString(), GameManager.Instance.StageMoveCount));

        // 스크린 사이즈의 끝 + 오브젝트 사이즈의 절반의 위치
        Vector3 tmpPos = new Vector3(Screen.width + curStageName.rectTransform.sizeDelta.x, Screen.height / 2 + 25);
        Vector3 linePos = new Vector3((-Screen.width / 2) - curStageNameLine.rectTransform.sizeDelta.x, Screen.height / 2 - 50);
        curStageName.transform.position = tmpPos;
        curStageNameLine.transform.position = linePos;

        // 스크린의 중앙으로 이동
        curStageName.transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2 + 25), 2f).SetEase(Ease.InOutBack);
        curStageNameLine.transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2 - 50), 2f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(showCurStageNameTime);

        // 초기 있었던 위치의 반대편으로 이동
        curStageName.transform.DOMove(new Vector3(-Screen.width / 2 - curStageName.rectTransform.sizeDelta.x, Screen.height / 2 + 25), 1.5f).SetEase(Ease.InOutBack);
        curStageNameLine.transform.DOMove(new Vector3(Screen.width + curStageNameLine.rectTransform.sizeDelta.x, Screen.height / 2 - 50), 1.5f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(showCurStageNameTime);
        showCurStageNameObj.SetActive(false);

        yield return null;

    }

    public void LoadToCenterScene()
    {
        Fade.Instance.FadeInAndLoadScene(Define.Scene.Center);
        //Managers.Scene.LoadScene(Define.Scene.CenterScene);
    }

    public void LoadToTitleScene()
    {
        Fade.Instance.FadeInAndLoadScene(Define.Scene.TitleScene);
        //Managers.Scene.LoadScene(Define.Scene.TitleScene);
    }

    public void LeaveBtn()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance.sceneType == Define.Scene.Boss || GameManager.Instance.sceneType == Define.Scene.Field)
        {
            // 전시용
            SaveManager.DeleteAllData();
            Fade.Instance.FadeInAndLoadScene(Define.Scene.Center);
            //Managers.Scene.LoadScene(Define.Scene.CenterScene);
        }
        //else if (GameManager.Instance.sceneType == Define.Scene.CenterScene)
        //{
        //    // 전시용
        //    SaveManager.DeleteAllData();
        //    GameManager.Instance.GameQuit();
        //}
    }
}
