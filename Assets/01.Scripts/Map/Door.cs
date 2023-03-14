using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public static Door Instance;

    private TilemapCollider2D tilemapCollider2D = null;
    private TilemapRenderer tilemapRenderer = null;

    public bool isEnableDoor { private set; get; } = false;
    public bool isDisableDoor { private set; get; } = false;

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
    }

    private void Start()
    {
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        isFirst = true;

        EnableDoors();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave || isFirst)
            {
                Debug.Log("Can Move");
                EnableDoors();
            }
            else
            {
                Debug.Log("Can't Move");
                DisableDoors();
            }
        }
    }

    public void EnableDoors()
    {
        isEnableDoor = true;
        isDisableDoor = false;
        tilemapCollider2D.isTrigger = true;
        tilemapRenderer.enabled = false;
    }
    public void DisableDoors()
    {
        isEnableDoor = false;
        isDisableDoor = true;
        tilemapCollider2D.isTrigger = false;
        tilemapRenderer.enabled = true;
    }

}
