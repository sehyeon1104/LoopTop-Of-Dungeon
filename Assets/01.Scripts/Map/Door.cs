using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider2D = null;
    private TilemapRenderer tilemapRenderer = null;

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
        tilemapRenderer = GetComponent<TilemapRenderer>();
        isFirst = true;
        tilemapCollider2D.isTrigger = true;
        tilemapRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave || isFirst)
            {
                Debug.Log("Can Move");
                tilemapCollider2D.isTrigger = true;
                tilemapRenderer.enabled = false;
            }
            else
            {
                Debug.Log("Can't Move");
                tilemapCollider2D.isTrigger = false;
                tilemapRenderer.enabled = true;
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
        }
    }

}
