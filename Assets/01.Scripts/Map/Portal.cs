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

            GameManager.Instance.StageMoveCount++;
            Debug.Log($"StageMoveCount : {GameManager.Instance.StageMoveCount}");

            if (GameManager.Instance.StageMoveCount == 0 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.BossScene;
            }
            else if(GameManager.Instance.StageMoveCount < 3 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.StageScene;
            }
            else if (GameManager.Instance.sceneType == Define.Scene.BossScene)
            {
                sceneType = Define.Scene.CenterScene;
            }
            GameManager.Instance.SetSceneType(sceneType);

            Managers.Scene.LoadScene(sceneType);

        }
    }

}
