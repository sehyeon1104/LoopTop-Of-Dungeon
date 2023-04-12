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
    public GameObject skillSelect;
    [Header("LeftUp")]
    [SerializeField]
    private GameObject hpPrefab;
    [SerializeField]
    private GameObject hpSpace;
    [SerializeField]
    private TextMeshProUGUI fragmentAmountTMP = null;

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

    TextMeshProUGUI fpsText;
    float timer = 0;
    int num = 0;
    //[Header("RightUp")]
    [Header("RightDown")]
    private GameObject Skill1Button;
    private GameObject Skill2Button;
    private GameObject UltButton;
    GameObject AttackButton;
    GameObject InteractionButton;
    [SerializeField]
    private GameObject blurPanel;

    public TextMeshProUGUI pressF = null;
    public List<Image> hpbars = new List<Image>();
    private void Awake()
    {
        AttackButton = playerUI.transform.Find("RightDown/Btns/AttackBtn").gameObject;
        Skill1Button = playerUI.transform.Find("RightDown/Btns/Skill1_Btn").gameObject;
        Skill2Button = playerUI.transform.Find("RightDown/Btns/Skill2_Btn").gameObject;
        UltButton = playerUI.transform.Find("RightDown/Btns/UltimateSkill_Btn").gameObject;
        InteractionButton = playerUI.transform.Find("RightDown/Btns/Interaction_Btn").gameObject;
        fpsText = playerUI.transform.Find("RightUp/FPS").GetComponent<TextMeshProUGUI>();   
    }
    private void Start()
    {
        HPInit();
        UpdateUI();
        DisActiveAllPanels();
    }
    private void Update()
    {
        FPSUpdate();
    }
    public void UpdateUI()
    {
        HpUpdate();
        UpdateGoods();
    }
    public void FPSUpdate()
    {

        num++;
        timer += Time.deltaTime;
        if(timer>1)
        {
            fpsText.text =  $"{Convert.ToString(num)}FPS";
            timer = 0;
            num = 0;
        }
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
        Skill1Button.SetActive(!Skill1Button.activeSelf);
        Skill2Button.SetActive(!Skill2Button.activeSelf);
        UltButton.SetActive(!UltButton.activeSelf);
    }


    #region Panels
    public void DisActiveAllPanels()
    {
        blurPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        checkOneMorePanel.SetActive(false);
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
    }

    #region GameOver
    public void Revive()
    {
        // TODO : ±¤°í½ÃÃ» ÈÄ ºÎÈ° or ÀçÈ­¼Ò¸ð ÈÄ ºÎÈ° ±¸Çö

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
        // TODO : ï¿½ï¿½ï¿½ï¿½ ï¿½ß¾ï¿½ ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ß¾Ó¸ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ìµï¿½

        LoadToCenterScene();
    }

    public void CheckOneMorePanelNo()
    {
        ToggleCheckOneMorePanel();
    }
    #endregion

    public bool SkillCooltime(PlayerSkillData skillData,Define.SkillNum skillNum)
    {
        GameObject touchedObj = EventSystem.current.currentSelectedGameObject;
        Image currentImage = touchedObj.GetComponent<Image>();
        if (currentImage.fillAmount != 1f)
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
        cooltimeImg.fillAmount = 0f;
        while (cooltimeImg.fillAmount < 1f)
        {
            cooltimeImg.fillAmount += Time.deltaTime / skillCooltime;
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    public void TransformUITest()
    {
        pressF.gameObject.SetActive(true);
    }

    public void HpUpdate()
    {
        for (int i = 0; i < hpbars.Count; i++)
        {
            if (i + 1 < hpbars.Count)
                if(hpbars[i + 1].fillAmount > float.Epsilon) continue;
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
    public IEnumerator ShowCurrentStageName()
    {
        if(showCurStageNameObj.gameObject == null)
        {
            Debug.LogWarning("showCurStageNameObj is Null!");
            yield break;
        }

        showCurStageNameObj.SetActive(true);

        curStageName.SetText(string.Format("{0}Stage", GameManager.Instance.mapTypeFlag.ToString()));

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
        // TODO : ï¿½ß¾ï¿½ ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ìµï¿½

        LoadToTitleScene(); // ï¿½Ó½ï¿½
    }

    public void LoadToTitleScene()
    {
        Managers.Scene.LoadScene(Define.Scene.TitleScene);
        //SceneManager.LoadScene("TitleScene");
    }

    
}
