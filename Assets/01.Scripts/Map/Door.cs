using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoSingleton<Door>
{
    private TilemapCollider2D tilemapCollider2D = null;

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

    private void Start()
    {
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
        isFirst = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave || isFirst)
            {
                Debug.Log("Can Move");
                tilemapCollider2D.isTrigger = true;
            }
            else
            {
                tilemapCollider2D.isTrigger = false;
                Debug.Log("Can't Move");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isFirst)
            {
                return;
            }

            tilemapCollider2D.isTrigger = false;
        }
    }
}
