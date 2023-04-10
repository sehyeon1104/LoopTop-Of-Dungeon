using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    private void OnEnable()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(transform.lossyScale.y * 2, transform.lossyScale.x * 2), transform.rotation.z);
        foreach (var col in cols)
        {
            if (col.CompareTag("Player"))
            {
                GameManager.Instance.Player.OnDamage(2, 0);
            }
        }
    }
}
