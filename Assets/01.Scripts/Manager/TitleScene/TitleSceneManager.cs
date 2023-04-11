using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    private bool isLoading = false;

    private void Start()
    {
        isLoading = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isLoading)
        {
            // Debug.Log("Load");
            LoadToMainScene();
        }
    }

    public void LoadToMainScene()
    {
        isLoading = true;
        GameManager.Instance.SetMapTypeFlag(Define.MapTypeFlag.Ghost);
        Managers.Scene.LoadScene(Define.Scene.Ghost_Stage1);
    }
}
