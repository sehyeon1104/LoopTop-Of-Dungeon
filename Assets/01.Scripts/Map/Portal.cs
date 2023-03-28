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

            nextSceneNum = SceneManager.GetActiveScene().buildIndex + 1;

            sceneType = nextSceneNum switch
            {
                0 => Define.Scene.TitleScene,
                //1 => Define.Scene.CenterScene,
                1 => Define.Scene.Ghost_Stage1,
                2 => Define.Scene.Ghost_Stage2,
                3 => Define.Scene.Ghost_Boss,

                _ => Define.Scene.Unknown
            };

            Managers.Scene.LoadScene(sceneType);

        }
    }

}
