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

    [SerializeField]
    private GameObject Options = null;
    private List<Button> btns = new List<Button>();
    private Button gameStartBtn = null;
    private Button dataInitBtn = null;
    private Button settingBtn = null;
    private Button gameQuitBtn = null;

    [SerializeField]
    private Image backgroundPanel = null;
    [SerializeField]
    private float backgroundPanelSpace = 110;

    private int index = 0;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //TODO : 가독성 UP

        gameStartBtn = Options.transform.Find("GameStartBtn").gameObject.GetComponent<Button>();
        dataInitBtn = Options.transform.Find("DataInitBtn").gameObject.GetComponent<Button>();
        settingBtn = Options.transform.Find("SettingBtn").gameObject.GetComponent<Button>();
        gameQuitBtn = Options.transform.Find("GameQuitBtn").gameObject.GetComponent<Button>();

        gameStartBtn.onClick.RemoveListener(GameStart);
        gameStartBtn.onClick.AddListener(GameStart);
        dataInitBtn.onClick.RemoveListener(InitData);
        dataInitBtn.onClick.AddListener(InitData);
        settingBtn.onClick.RemoveListener(Setting);
        settingBtn.onClick.AddListener(Setting);
        gameQuitBtn.onClick.RemoveListener(GameQuit);
        gameQuitBtn.onClick.AddListener(GameQuit);

        backgroundPanel.rectTransform.position = Vector3.zero;
    }

    private void Start()
    {
        isLoading = false;
    }

    private void Update()
    {
        InputKey();

        //if (Input.GetMouseButtonDown(0) && !isLoading)
        //{
        //    // Debug.Log("Load");
        //    LoadToMainScene();
        //}
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            index += 1;
            index = Mathf.Clamp(index, 0, Options.transform.childCount - 1);
            MoveBackgroundPanel();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            index -= 1;
            index = Mathf.Clamp(index, 0, Options.transform.childCount - 1);
            MoveBackgroundPanel();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            InvokeBtnOnClick();
        }
    }

    private void MoveBackgroundPanel()
    {
        backgroundPanel.transform.DOMoveY(Options.transform.GetChild(index).position.y, 0.15f);
    }

    private void InvokeBtnOnClick()
    {
        Options.transform.GetChild(index).GetComponent<Button>().onClick.Invoke();
    }

    public void GameStart()
    {
        if (!isLoading)
        {
            isLoading = true;

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
