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

public class PCUIManager : MonoSingleton<PCUIManager>
{
    public GameObject playerUI;
    public GameObject skillSelect;
    [Header("LeftUp")]
    [SerializeField]
    private GameObject hpPrefab;
    [SerializeField]
    private GameObject hpSpace;
    [SerializeField]
    private TextMeshProUGUI fragmentAmountTMP = null;
    [SerializeField]
    private TextMeshProUGUI bossFragmentAmountTMP = null;

    // [Header("LeftDown")]

    [Header("Middle")]
 
    public GameObject skillSelectObj;
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

    float timer = 0;
    int num = 0;
    // [Header("RightUp")]
    [Header("RightDown")]
    Image[] skillIcons = new Image[4];
    GameObject AttackButton;
    GameObject InteractionButton;
    [SerializeField]
    private GameObject blurPanel;

    public List<Image> hpbars = new List<Image>();

    private void Awake()
    { 
    }

    private void Start()
    {
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

    public void ToggleGameOverPanel()
    {   
        TogglePlayerAttackUI();
        blurPanel.SetActive(!blurPanel.activeSelf);
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
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

    public bool SkillCooltime(PlayerSkillData skillData, Define.SkillNum skillNum)
    {
        GameObject touchedObj = EventSystem.current.currentSelectedGameObject;
        Image currentImage = touchedObj.transform.Find("CooltimeImg").GetComponent<Image>();
        if (currentImage.fillAmount > 0)
            return false;

        StartCoroutine(IESkillCooltime(currentImage, skillData.skill[(int)skillNum].skillDelay));
        return true;
    }
    public void SkillNum(List<int> skillList)
    {
         Button[] selectTexts = skillSelectObj.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < selectTexts.Length; i++)
        {
            selectTexts[i] .GetComponentInChildren<TextMeshProUGUI>().text = skillList[i].ToString();
        }
    }
    public IEnumerator IESkillCooltime(Image cooltimeImg, float skillCooltime)
    {
        cooltimeImg.fillAmount = 1f;
        while (cooltimeImg.fillAmount > 0)
        {
            cooltimeImg.fillAmount -= Time.deltaTime / skillCooltime;
            yield return null;
        }
    }
    public void ResetSkill()
    {
        for(int i=0; i<skillIcons.Length; i++)
        {
            skillIcons[i].sprite = null;
        }
    }
    public void SetSkillIcon(PlayerSkillData skilldata,int iconNum,int skillNum,int spriteNum)
    {
        if (iconNum == 0 && skillIcons[0].sprite != null)
            iconNum++;

            skillIcons[iconNum].sprite = skilldata.skill[skillNum].skillIcon[spriteNum];
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
        if(showCurStageNameObj.gameObject == null)
        {
            Debug.LogWarning("showCurStageNameObj is Null!");
            yield break;
        }
        if(GameManager.Instance.sceneType == Define.Scene.CenterScene || GameManager.Instance.sceneType == Define.Scene.BossScene)
        {
            yield break;
        }

        showCurStageNameObj.SetActive(true);

        curStageName.SetText(string.Format("{0}Stage_{1}", GameManager.Instance.mapTypeFlag.ToString(), GameManager.Instance.StageMoveCount));

        Vector3 tmpPos = new Vector3(Screen.width + curStageName.rectTransform.sizeDelta.x, Screen.height / 2 + 25);
        Vector3 linePos = new Vector3((-Screen.width / 2) - curStageNameLine.rectTransform.sizeDelta.x, Screen.height / 2 - 50);
        curStageName.transform.position = tmpPos;
        curStageNameLine.transform.position = linePos;

        curStageName.transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2 + 25), 2f).SetEase(Ease.InOutBack);
        curStageNameLine.transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2 - 50), 2f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(showCurStageNameTime);

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
        if(GameManager.Instance.sceneType == Define.Scene.BossScene || GameManager.Instance.sceneType == Define.Scene.StageScene)
        {
            Fade.Instance.FadeInAndLoadScene(Define.Scene.CenterScene);
            //Managers.Scene.LoadScene(Define.Scene.CenterScene);
        }
        else if(GameManager.Instance.sceneType == Define.Scene.CenterScene)
        {
            GameManager.Instance.GameQuit();
        }
    }
}
