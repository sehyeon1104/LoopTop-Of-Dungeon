using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isLoadScene = false;
    private int nextSceneNum = 0;
    Define.Scene sceneType;

    private void Start()
    {
        isLoadScene = false;
    }

    private void FixedUpdate()
    {
        if(Vector2.Distance(GameManager.Instance.Player.transform.position, transform.position) < 1f)
        {
            MoveNextStage();
        }
    }

    public void MoveNextStage()
    {
        if (!isLoadScene)
        {
            isLoadScene = true;
            GameManager.Instance.SavePlayerStat();

            GameManager.Instance.StageClearCount++;

            if (GameManager.Instance.StageClearCount == 0 && GameManager.Instance.mapTypeFlag != Define.MapTypeFlag.CenterMap)
            {
                sceneType = Define.Scene.BossScene;
            }
            else if(GameManager.Instance.StageClearCount < 2 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.StageScene;
            }
            else if (GameManager.Instance.sceneType == Define.Scene.BossScene)
            {
                sceneType = Define.Scene.CenterScene;
            }

            Managers.Scene.LoadScene(sceneType);

        }
    }

}
