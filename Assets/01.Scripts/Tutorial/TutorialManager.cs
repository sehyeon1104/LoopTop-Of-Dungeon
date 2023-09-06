using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    private TutoObjSpawn tutoObjSpawn = null;
    private TutoEnemySpawn tutoEnemySpawn = null;

    private void Awake()
    {
        tutoObjSpawn = FindObjectOfType<TutoObjSpawn>();
        tutoEnemySpawn = FindObjectOfType<TutoEnemySpawn>();
    }

    public void Start()
    {
        GameManager.Instance.Player.transform.position = Vector3.zero;
    }
}
