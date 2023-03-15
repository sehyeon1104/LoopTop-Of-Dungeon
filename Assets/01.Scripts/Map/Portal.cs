using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private int nextSceneNum = 0;
    Define.Scene sceneType;

    public void MoveNextStage()
    {
        nextSceneNum = SceneManager.GetActiveScene().buildIndex + 1;

        sceneType = nextSceneNum switch
        {
            1 => Define.Scene.TitleScene,
            2 => Define.Scene.MainScene,
            3 => Define.Scene.Ghost_Stage1,
            4 => Define.Scene.Ghost_Stage2,
            5 => Define.Scene.GhostSceneBoss,

            _ => Define.Scene.Unknown
        };

        Managers.Scene.LoadScene(sceneType);
    }

}
