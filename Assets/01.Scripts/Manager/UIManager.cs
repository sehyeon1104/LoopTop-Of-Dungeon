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
    int skillSelectNum = 0;
    [Header("LeftUp")]
    [SerializeField]
    private Image playerIcon = null;
    [SerializeField]
    private GameObject hpPrefab;
    [SerializeField]
    private GameObject hpSpace;
    [SerializeField]
    GameObject skillSelect;
    // [Header("LeftDown")]

    [Header("Middle")]
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject checkOneMorePanel;

    //[Header("RightUp")]
    // [Header("RightDown")]

    [SerializeField]
    private GameObject blurPanel;

    public TextMeshProUGUI pressF = null;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        playerIcon.sprite = Player.Instance.playerTransformDataSO.playerImg;

        DisActiveAllPanels();
        HpUpdate();
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

    #region GameOver
    public void Revive()
    {
        // TODO : 이후 광고시청 후 부활 or 재화 소모 후 부활 구현

        Debug.Log("Revive");
        ToggleGameOverPanel();
        Player.Instance.RevivePlayer();
    }

    public void Leave()
    {
        ToggleCheckOneMorePanel();
    }

    public void CheckOneMorePanelYes()
    {
        // TODO : 이후 중앙 맵 씬 생성 시 중앙맵으로 이동

        MoveToCenterMap();
    }

    public void CheckOneMorePanelNo()
    {
        ToggleCheckOneMorePanel();
    }
    #endregion

    public void SkillCooltime(Image cooltimeImg)
    {
        if (cooltimeImg.fillAmount != 0f)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject.CompareTag("Skill1"))
        {
            Player.Instance.Skill1();
        }
        else if (EventSystem.current.currentSelectedGameObject.CompareTag("Skill2"))
        {
            Player.Instance.Skill2();
        }
        else if (EventSystem.current.currentSelectedGameObject.CompareTag("UltimateSkill"))
        {
            Player.Instance.UltimateSkill();
        }

        StartCoroutine(IESkillCooltime(cooltimeImg, Player.Instance.skillCooltime));
    }
    public void SkillNum(List<int> skillList)
    {
         Button[] selectTexts = skillSelect.GetComponentsInChildren<Button>(true);
        print($"리스트 수{selectTexts.Length}");
        for (int i = 0; i < selectTexts.Length; i++)
        {
            print(skillList[i]);
            selectTexts[i].GetComponentInChildren<TextMeshProUGUI>().text = skillList[i].ToString();
        }
    }
    public void SkillSelecet()
    {
       skillSelectNum++;
       GameObject selectObj = EventSystem.current.currentSelectedGameObject;
       int selectNum = int.Parse(selectObj.GetComponentInChildren<TextMeshProUGUI>().text);
        if(skillSelectNum>1)
        {
            skillSelect.SetActive(false);   
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

        Transform[] hpbars = hpSpace.GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < hpbars.Length; i++)
        {
            hpbars[i].gameObject.SetActive(false);
            // Destroy(hpbars[i].gameObject);
        }
        for (int i = 1; i <= Player.Instance.pBase.Hp; i++)
        {
            hpbars[i].gameObject.SetActive(true);
            //Debug.Log("Current Hp : " + Player.Instance.pBase.Hp);
            ////Instantiate(hpPrefab, hpSpace.transform);
            //Instantiate(hpPrefab, new Vector3(255 + 105 * i, 1030, 0), Quaternion.identity, hpSpace.transform);
        }

    }


    public void MoveToCenterMap()
    {
        // TODO : 중앙 맵 씬 생성시 이동

        MoveToTitleScene(); // 임시
    }

    public void MoveToTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
