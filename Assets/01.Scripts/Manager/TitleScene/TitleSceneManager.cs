using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class TitleSceneManager : MonoBehaviour
{
    private bool isLoading = false;
    private bool isClickScreen = false;

    private Transform titleUI = null;
    private TextMeshProUGUI titleTmp = null;

    private GameObject blurPanel = null;

    #region Middle
    // Middle
    private Transform selectionList = null;

    private GameObject options = null;
    private Button gameStartBtn = null;
    private Button dataInitBtn = null;
    private Button settingBtn = null;
    private Button gameQuitBtn = null;

    private Image backgroundPanel = null;
    [SerializeField]
    private float backgroundPanelSpace = 110;

    private int index = 0;
    #endregion


    private void Awake()
    {
        Init();
    }

    #region Init
    private void Init()
    {
        titleUI = GameObject.Find("TitleUI").transform;
        titleTmp = titleUI.Find("MiddleUp/Title/TitleTMP").GetComponent<TextMeshProUGUI>();

        blurPanel = titleUI.Find("Blur/BlurPanel").gameObject;

        // Middle
        selectionList = titleUI.Find("MiddleMiddle/SelectionList").transform;
        options = selectionList.Find("Options").gameObject;
        backgroundPanel = selectionList.transform.Find("BackgroundPanel").GetComponent<Image>();

        gameStartBtn = options.transform.Find("GameStartBtn").GetComponent<Button>();
        dataInitBtn = options.transform.Find("DataInitBtn").GetComponent<Button>();
        settingBtn = options.transform.Find("SettingBtn").GetComponent<Button>();
        gameQuitBtn = options.transform.Find("GameQuitBtn").GetComponent<Button>();

        InitBtns();

        backgroundPanel.rectTransform.position = options.transform.position;
    }

    private void InitBtns()
    {
        // 리스너 추가
        gameStartBtn.onClick.RemoveListener(GameStart);
        gameStartBtn.onClick.AddListener(GameStart);
        dataInitBtn.onClick.RemoveListener(InitData);
        dataInitBtn.onClick.AddListener(InitData);
        settingBtn.onClick.RemoveListener(Setting);
        settingBtn.onClick.AddListener(Setting);
        gameQuitBtn.onClick.RemoveListener(GameQuit);
        gameQuitBtn.onClick.AddListener(GameQuit);
    }
    #endregion

    #region Start
    private void Start()
    {
        isLoading = false;
        OnStart();
    }

    private void OnStart()
    {
        selectionList.gameObject.SetActive(false);
        blurPanel.SetActive(false);
    }
    #endregion

    #region Update
    private void Update()
    {
        InputKeyOrButton();
    }

    private void InputKeyOrButton()
    {
        if(!isClickScreen && Input.anyKeyDown)
        {
            isClickScreen = true;

            StartCoroutine(ShowListOnTouch());
        }

        if (isClickScreen)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                index += 1;
                if(index <= options.transform.childCount - 1)
                    Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Title/Select.mp3");

                index = Mathf.Clamp(index, 0, options.transform.childCount - 1);
                MoveBackgroundPanel();
            }

            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                index -= 1;
                if (index >= 0)
                    Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Title/Select.mp3");

                index = Mathf.Clamp(index, 0, options.transform.childCount - 1);
                MoveBackgroundPanel();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                InvokeBtnOnClick();
            }
        }
    }
    #endregion

    private IEnumerator ShowListOnTouch()
    {
        blurPanel.SetActive(true);
        // titleTmp를 위로 올린 후
        titleTmp.rectTransform.DOMoveY(titleTmp.rectTransform.position.y + 150f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        // 게임 선택 목록 활성화
        selectionList.gameObject.SetActive(true);
    }

    private void MoveBackgroundPanel()
    {
        backgroundPanel.transform.DOMoveY(options.transform.GetChild(index).position.y, 0.15f);
    }

    private void InvokeBtnOnClick()
    {
        options.transform.GetChild(index).GetComponent<Button>().onClick.Invoke();
    }

    public void GameStart()
    {
        if (!isLoading)
        {
            isLoading = true;

            Fade.Instance.FadeInAndLoadScene(Define.Scene.CenterScene);
        }
    }

    public void Setting()
    {

    }

    public void InitData()
    {
        SaveManager.DeleteAllData();
    }

    public void GameQuit()
    {
        Rito.Debug.Log("게임 종료");
        Application.Quit();
    }

    //public void LoadToMainScene()
    //{
    //    isLoading = true;
    //    GameManager.Instance.SetMapTypeFlag(Define.MapTypeFlag.CenterMap);
    //    GameManager.Instance.SetSceneType(Define.Scene.CenterScene);
    //    //GameManager.Instance.SaveData();
    //    Managers.Scene.LoadScene(Define.Scene.CenterScene);
    //}
}
