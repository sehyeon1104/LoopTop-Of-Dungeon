using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    private GameObject movePortal = null;
    private Vector3 portalPos;

    public TutoObjSpawn tutoObjSpawn { get; private set; } = null;
    public TutoEnemyRoom tutoEnemyRoom { get; private set; } = null;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        tutoObjSpawn = FindObjectOfType<TutoObjSpawn>();
        tutoEnemyRoom = FindObjectOfType<TutoEnemyRoom>();

        portalPos = tutoEnemyRoom.transform.position;
        portalPos.x -= 0.5f;
    }

    public void Start()
    {
        GameManager.Instance.SetMapTypeFlag(Define.MapTypeFlag.Tutorial);
        GameManager.Instance.SetSceneType(Define.Scene.Tutorial);
        GameManager.Instance.Player.transform.position = Vector3.zero;
    }

    public void ClearTuto()
    {
        movePortal = Managers.Resource.Instantiate("Assets/03.Prefabs/Maps/Magic_Circle_Move.prefab");
        movePortal.transform.position = portalPos;
    }
}
