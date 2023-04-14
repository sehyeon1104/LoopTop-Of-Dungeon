using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        transform.localScale += new Vector3(Time.deltaTime * 7, Time.deltaTime * 7);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.Player.OnDamage(2, 0);
        }
    }
}
