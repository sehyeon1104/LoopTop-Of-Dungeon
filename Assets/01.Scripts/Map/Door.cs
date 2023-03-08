using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool canMove { private set; get; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (EnemySpawnManager.Instance.curEnemies.Count == 0 && EnemySpawnManager.Instance.isNextWave)
            {
                canMove = true;
                Debug.Log("Can Move");
            }
            else
            {
                Debug.Log("Can't Move");
            }
        }
    }

    public bool CanMove()
    {
        return canMove;
    }
}
