using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeamCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy")||collision.CompareTag("Boss"))
        {
            collision.GetComponent<IHittable>().OnDamage(5, 0);
        }
    }
}
