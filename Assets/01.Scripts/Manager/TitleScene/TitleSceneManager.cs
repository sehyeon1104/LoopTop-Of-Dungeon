using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Load");
            LoadToMainScene();
        }
    }

    public void LoadToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
