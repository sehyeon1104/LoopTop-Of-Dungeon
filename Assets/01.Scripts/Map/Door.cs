using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public static Door Instance;

    private TilemapCollider2D tilemapCollider2D = null;
    private TilemapRenderer tilemapRenderer = null;

    public bool isOpenDoor { private set; get; } = false;
    public bool isCloseDoor { private set; get; } = false;

    private bool isFirst = false;
    public bool IsFirst
    {
        get
        {
            return isFirst;
        }
        set
        {
            isFirst = value;
        }
    }

    private void Awake()
    {
        Instance = this;
        isFirst = true;
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void Start()
    {
        OpenDoors();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave || isFirst)
    //        {
    //            Debug.Log("Can Move");
    //            OpenDoors();
    //        }
    //        else
    //        {
    //            Debug.Log("Can't Move");
    //            CloseDoors();
    //        }
    //    }
    //}

    public void OpenDoors()
    {
        isOpenDoor = true;
        isCloseDoor = false;
        if(tilemapCollider2D == null)
        {
            Rito.Debug.Log("tilemapCollider2D is null!");
            return;
        }
        tilemapCollider2D.isTrigger = true;
        tilemapRenderer.enabled = false;
    }
    public void CloseDoors()
    {
        isOpenDoor = false;
        isCloseDoor = true;
        tilemapCollider2D.isTrigger = false;
        tilemapRenderer.enabled = true;
    }

}
