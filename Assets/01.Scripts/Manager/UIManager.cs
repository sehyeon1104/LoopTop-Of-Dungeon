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

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject playerUI;
    public GameObject playerPCUI;
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

    public GameObject skillSelect;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject checkOneMorePanel;

    [SerializeField]
    private GameObject showCurStageNameObj;
    [SerializeField]
    private TextMeshProUGUI curStageName;
    [SerializeField]
    private Image curStageNameLine;
    [SerializeField]
    private float showCurStageNameTime = 3f;
    // [Header("RightUp")]
    [Header("RightDown")]
    public GameObject skill1Button;
    public GameObject skill2Button;
    public GameObject ultButton;
    public GameObject dashButton;
    public Button reviveButton;
    public Button leaveButton;
    Image[] skillIcons = new Image[4];
    Image[] pcSkillIcons = new Image[4];
    GameObject AttackButton;
    GameObject InteractionButton;
    [SerializeField]
    private GameObject blurPanel;
    public List<Image> hpbars = new List<Image>();
    PlayerSkill playerSkill;

    private WaitForEndOfFrame waitForEndOfFrame;
    private void Awake()
    {
        playerSkill = FindObjectOfType<PlayerSkill>();
        playerUI = GameObject.Find("PlayerUI").gameObject;
        hpPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/Heart.prefab");
        AttackButton = playerUI.transform.Find("RightDown/Btns/AttackBtn").gameObject;
        skill1Button = playerUI.transform.Find("RightDown/Btns/Skill1_Btn").gameObject;
        skill2Button = playerUI.transform.Find("RightDown/Btns/Skill2_Btn").gameObject;
        ultButton = playerUI.transform.Find("RightDown/Btns/UltimateSkill_Btn").gameObject;
        dashButton = playerUI.transform.Find("RightDown/Btns/Dash_Btn").gameObject;
        InteractionButton = playerUI.transform.Find("RightDown/Btns/Interaction_Btn").gameObject;
        if (GameManager.Instance.platForm == Define.PlatForm.Mobile)
        {
            hpSpace = playerUI.transform.Find("LeftUp/PlayerHP").gameObject;
            fragmentAmountTMP = playerUI.transform.Find("LeftUp/Goods/ExperienceFragmentUI/FragmentAmountTMP").GetComponent<TextMeshProUGUI>();
            bossFragmentAmountTMP = playerUI.transform.Find("LeftUp/Goods/BossFragmentUI/BossFragmentAmountTMP").GetComponent<TextMeshProUGUI>();
            pausePanel = playerUI.transform.Find("All/PausePanel").gameObject;
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
        }
        else
        {
            playerPCUI = GameObject.Find("PCPlayerUI").gameObject;
            skillSelect = playerPCUI.transform.Find("SkillSelect").gameObject;
            hpSpace = playerPCUI.transform.Find("LeftDown/PlayerHP").gameObject;
            playerItemListUI = playerPCUI.transform.Find("LeftDown/PlayerItemList");
            fragmentAmountTMP = playerPCUI.transform.Find("RightUp/Goods/ExperienceFragmentUI/FragmentAmountTMP").GetComponent<TextMeshProUGUI>();
            bossFragmentAmountTMP = playerPCUI.transform.Find("RightUp/Goods/BossFragmentUI/BossFragmentAmountTMP").GetComponent<TextMeshProUGUI>();
            pausePanel = playerPCUI.transform.Find("All/PausePanel").gameObject;
            gameOverPanel = playerPCUI.transform.Find("All/GameOverPanel").gameObject;
            checkOneMorePanel = playerPCUI.transform.Find("Middle/CheckOneMorePanel").gameObject;
            showCurStageNameObj = playerPCUI.transform.Find("Middle/ShowCurStageName").gameObject;
            curStageName = showCurStageNameObj.transform.Find("CurStageName").GetComponent<TextMeshProUGUI>();
            blurPanel = playerPCUI.transform.Find("All/BlurPanel").gameObject;
            curStageNameLine = showCurStageNameObj.transform.Find("Line").GetComponent<Image>();
            reviveButton = playerPCUI.transform.Find("All/GameOverPanel/Panel/Btns/Revive").GetComponent<Button>();
            leaveButton = playerPCUI.transform.Find("All/GameOverPanel/Panel/Btns/Leave").GetComponent<Button>();
            pcSkillIcons[0] = playerPCUI.transform.Find("RightDown/Btns/Skill1_Btn/ShapeFrame/Icon").GetComponent<Image>();
            pcSkillIcons[1] = playerPCUI.transform.Find("RightDown/Btns/Skill2_Btn/ShapeFrame/Icon").GetComponent<Image>();
            pcSkillIcons[2] = playerPCUI.transform.Find("RightDown/Btns/UltimateSkill_Btn/ShapeFrame/Icon").GetComponent<Image>();
            pcSkillIcons[3] = playerPCUI.transform.Find("RightDown/Btns/Dash_Btn/ShapeFrame/Icon").GetComponent<Image>();
        }
        itemUITemplate = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/UI/ItemUI.prefab");

        leaveButton.onClick.AddListener(LeaveBtn);

    }

    private void Start()
    {
        waitForEndOfFrame = new WaitForEndOfFrame();

        SkillSelectButtonInit();
        HPInit();
        UpdateUI();
        DisActiveAllPanels();
    }

    public void UpdateUI()
    {
        HpUpdate();
        UpdateGoods();
    }

    public void HPInit()
    {
        foreach (var avc in hpSpace.GetComponentsInChildren<Heart>())
        {
            hpbars.Add(avc.GetComponent<Image>());
        }
    }

    public void TogglePlayerAttackUI()
    {
        AttackButton.SetActive(!AttackButton.activeSelf);
        skill1Button.SetActive(!skill1Button.activeSelf);
        skill2Button.SetActive(!skill2Button.activeSelf);
        ultButton.SetActive(!ultButton.activeSelf);
    }

    #region Panels
    public void DisActiveAllPanels()
    {
        blurPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        checkOneMorePanel.SetActive(false);
        InventoryUI.Instance.transform.Find("Background").gameObject.SetActive(false);
    }

    public void TogglePausePanel()
    {
        blurPanel.SetActive(!pausePanel.activeSelf);
        pausePanel.SetActive(!pausePanel.activeSelf);

        if (pausePanel.activeSelf)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void Resume()
    {
        TogglePausePanel();
    }
    public void SkillSelectButtonInit()
    {
        for(int i=0; i<skillSelect.transform.childCount; i++)
        {
            GameObject button = skillSelect.transform.GetChild(i).gameObject;
            button.GetComponent<Button>().onClick.AddListener(() => button.SetActive(false));
        }
    }
   
    public void ToggleGameOverPanel()
    {
        TogglePlayerAttackUI();
        //blurPanel.SetActive(!blurPanel.activeSelf);
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
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
        bossFragmentAmountTMP.SetText(GameManager.Instance.Player.playerBase.BossFragmentAmount.ToString());
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

    public void AddItemListUI(Item item)
    {
        if(item.itemType == Define.ItemType.heal)
        {
            return;
        }

        GameObject itemUI = Instantiate(itemUITemplate, playerItemListUI);
        Image itemIcon = itemUI.transform.Find("ItemIcon").GetComponent<Image>();
        itemIcon.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/Icon/Item/{item.itemRating}/{item.itemNameEng}.png");
    }

    public bool SkillCooltime(PlayerSkillData skillData, int skillNum,bool isCheck =false)
    {
        int num;
        if (skillNum ==7)
            num = 2;
        else if (skillNum == 8)
            num = 3;
        else
        {
            if (playerSkill.skillIndex[0] == skillNum)
                num = 0;
            else
                num = 1;
        }
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
            currentImage = playerPCUI.transform.Find("RightDown/Btns").GetChild(num).transform.Find("CooltimeImg").GetComponent<Image>();

            if (currentImage.fillAmount > 0)
                return false;
        }
        if (!isCheck)
           StartCoroutine(IESkillCooltime(currentImage, skillData.skill[skillNum].skillDelay));
 
        return true;
    }
    public IEnumerator IESkillCooltime(Image cooltimeImg, float skillCooltime)
    {
        cooltimeImg.fillAmount = 1f;
        while (cooltimeImg.fillAmount > 0)
        {
            cooltimeImg.fillAmount -= Time.deltaTime / skillCooltime;
            yield return waitForEndOfFrame;
        }
        yield break;
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

    public void HpUpdate()
    {
        for (int i = 0; i < hpbars.Count; i++)
        {
            hpbars[i].fillAmount = (GameManager.Instance.Player.playerBase.Hp * 0.25f) - i;
        }

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
        if (GameManager.Instance.sceneType == Define.Scene.CenterScene || GameManager.Instance.sceneType == Define.Scene.BossScene)
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
        Fade.Instance.FadeInAndLoadScene(Define.Scene.CenterScene);
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
        if (GameManager.Instance.sceneType == Define.Scene.BossScene || GameManager.Instance.sceneType == Define.Scene.StageScene)
        {
            SaveManager.DeleteAllData();
            Fade.Instance.FadeInAndLoadScene(Define.Scene.CenterScene);
            //Managers.Scene.LoadScene(Define.Scene.CenterScene);
        }
        else if (GameManager.Instance.sceneType == Define.Scene.CenterScene)
        {
            SaveManager.DeleteAllData();
            GameManager.Instance.GameQuit();
        }
    }
}
