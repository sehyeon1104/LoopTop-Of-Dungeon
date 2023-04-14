using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEntrance : MonoBehaviour
{
    private Define.MapTypeFlag mapTypeFlag;

    private bool isLoadScene = false;
    Define.Scene sceneType;

    private void Start()
    {
        isLoadScene = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mapTypeFlag = gameObject.name switch
            {
                "Ghost" => Define.MapTypeFlag.Ghost,
                "LavaSlime" => Define.MapTypeFlag.LavaSlime,
                "Electricity" => Define.MapTypeFlag.Electricity,
                "Werewolf" => Define.MapTypeFlag.Werewolf,
                "Lizard" => Define.MapTypeFlag.Lizard,
                "Power" => Define.MapTypeFlag.Power,
                _ => Define.MapTypeFlag.Default,
            };

            GameManager.Instance.SetMapTypeFlag(mapTypeFlag);
            MoveNextStage();
        }
    }

    public void MoveNextStage()
    {
        if (!isLoadScene)
        {
            isLoadScene = true;

            GameManager.Instance.StageMoveCount++;

            if (GameManager.Instance.StageMoveCount == 0 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.BossScene;
            }
            else if (GameManager.Instance.StageMoveCount < 3 && GameManager.Instance.sceneType != Define.Scene.BossScene)
            {
                sceneType = Define.Scene.StageScene;
            }
            else if (GameManager.Instance.sceneType == Define.Scene.BossScene)
            {
                sceneType = Define.Scene.CenterScene;
            }
            GameManager.Instance.SetSceneType(sceneType);
            GameManager.Instance.SaveData();

            Managers.Scene.LoadScene(sceneType);

        }
    }
}
