using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool isRed = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Managers.Pool.Push(GetComponent<Poolable>());
            GhostBossUI.fillTime += isRed ? 10 : -5;
            Managers.Pool.PoolManaging("10.Effects/ghost/EatBubble", transform.position, Quaternion.identity);
        }
    }
}
