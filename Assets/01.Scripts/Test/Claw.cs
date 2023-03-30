using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    private void OnEnable()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, transform.lossyScale, 0);
        foreach (var col in cols)
        {
            Debug.Log(col.name);
            if (col.CompareTag("Player"))
            {
                GameManager.Instance.Player.OnDamage(2, gameObject, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
