using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoDashObj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.Player.IsInvincibility)
                return;

            GameManager.Instance.Player.OnDamage(0, 0);
            GameManager.Instance.Player.transform.Translate(new Vector3(0, -3, 0));
        }
    }
}
