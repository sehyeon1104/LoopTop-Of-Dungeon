using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    private void OnEnable()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(transform.lossyScale.y * 2, transform.lossyScale.x * 2), transform.rotation.z, 1<<8);
        foreach (var col in cols)
        {
            GameManager.Instance.Player.OnDamage(2, 0);
        }
    }
}
