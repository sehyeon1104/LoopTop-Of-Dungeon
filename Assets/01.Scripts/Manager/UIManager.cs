using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UIManager : MonoSingleton<UIManager>
{
    // [Header("LeftUp")]
    // [Header("LeftDown")]

    [Header("Middle")]
    [SerializeField]
    private GameObject pausePanel;

    // [Header("RightUp")]
    // [Header("RightDown")]

    [SerializeField]
    private GameObject blurPanel;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {

        DisActiveAllPanels();
    }

    public void DisActiveAllPanels()
    {
        blurPanel.SetActive(false);
        pausePanel.SetActive(false);
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

    public void SkillCooltime(Image cooltimeImg)
    {
        if (cooltimeImg.fillAmount != 0f)
        {
            return;
        }

        StartCoroutine(IESkillCooltime(cooltimeImg));
    }

    public IEnumerator IESkillCooltime(Image cooltimeImg)
    {
        cooltimeImg.fillAmount = 1f;

        float skillCooltime = 2f;   // µð¹ö±ë¿ë ÄðÅ¸ÀÓ

        while(cooltimeImg.fillAmount > 0f)
        {
            cooltimeImg.fillAmount -= Time.deltaTime / skillCooltime;
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }
}
