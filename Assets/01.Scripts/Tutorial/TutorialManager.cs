using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [SerializeField]
    private GameObject movePortal = null;

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
    }

    public void Start()
    {
        GameManager.Instance.Player.transform.position = Vector3.zero;
    }

    public void ClearTuto()
    {

    }
}
