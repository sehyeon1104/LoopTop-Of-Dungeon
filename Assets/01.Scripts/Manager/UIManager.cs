using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Linq;

public class UIManager : MonoSingleton<UIManager>
{ 
    Player playerCompo = null;
    int skillSelectNum = 0;
    public GameObject skillSelect;
    [Header("LeftUp")]
    [SerializeField]
    private Image playerIcon = null;
    [SerializeField]
    private GameObject hpPrefab;
    [SerializeField]
    private GameObject hpSpace;
    [SerializeField]
    private Image fragmentImage = null;
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

    [Tooltip("게임 입장 시 현재 스테이지 이름 출력")]
    [SerializeField]
    private GameObject showCurStageNameObj;
    [SerializeField]
    private TextMeshProUGUI curStageName;
    [SerializeField]
    private Image curStageNameLine;
    [SerializeField]
    private float showCurStageNameTime = 3f;

    //[Header("RightUp")]
    // [Header("RightDown")]

    [SerializeField]
    private GameObject blurPanel;

    public TextMeshProUGUI pressF = null;
    public List<Image> hpbars = new List<Image>();
    private void Awake()
    {
        playerCompo = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // playerIcon = GameManager.Instance.Player.
        DisActiveAllPanels();
        HPInit();
        HpUpdate();
    }
    public void HPInit()
    {
        foreach (var avc in hpSpace.GetComponentsInChildren<Heart>())
        {
            hpbars.Add(avc.GetComponent<Image>());
        }
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
        blurPanel.SetActive(!pausePanel.activeSelf);
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
        // TODO : 이후 광고시청 후 부활 or 재화 소모 후 부활 구현

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
        // TODO : 이후 중앙 맵 씬 생성 시 중앙맵으로 이동

        LoadToCenterScene();
    }

    public void CheckOneMorePanelNo()
    {
        ToggleCheckOneMorePanel();
    }
    #endregion

    public void SkillCooltime(PlayerSkillData skillData,int skillNum)
    {
        GameObject touchedObj = EventSystem.current.currentSelectedGameObject;
        Image currentImage = touchedObj.GetComponent<Image>();
        if (currentImage.fillAmount != 0f)
        {
            return;
        }
        StartCoroutine(IESkillCooltime(currentImage, skillData.skill[skillNum-1].skillDelay));
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

        while (cooltimeImg.fillAmount > 0f)
        {
            cooltimeImg.fillAmount -= Time.deltaTime / skillCooltime;
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
                if(hpbars[i + 1].fillAmount > float.Epsilon) continue; //자신보다 한 칸 위에 HP가 있고 fillAmount가 0보다 크다면 넘기기
            hpbars[i].fillAmount = (GameManager.Instance.Player.playerBase.Hp * 0.25f) - i;
        }

    }

    public IEnumerator ShowCurrentStageName()
    {
        showCurStageNameObj.SetActive(true);
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
        // TODO : 중앙 맵 씬 생성시 이동

        LoadToTitleScene(); // 임시
    }

    public void LoadToTitleScene()
    {
        Managers.Scene.LoadScene(Define.Scene.TitleScene);
        //SceneManager.LoadScene("TitleScene");
    }

    
}
