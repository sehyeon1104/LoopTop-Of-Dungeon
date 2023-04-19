using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTo : MonoBehaviour
{
    private void OnEnable()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.25f, 0f), new Vector2(1.5f,2f), 0);
        foreach (var col in cols)
        {
            if (col.CompareTag("Player"))
            {
                GameManager.Instance.Player.OnDamage(1, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(0.25f, 0f), new Vector2(3f, 4f));
    }
}
