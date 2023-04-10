using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeamColCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            collision.GetComponent<IHittable>().OnDamage(2, GameManager.Instance.Player.playerBase.CritChance);
        }
    }
}
