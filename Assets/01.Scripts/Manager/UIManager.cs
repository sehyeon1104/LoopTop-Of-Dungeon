using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}
