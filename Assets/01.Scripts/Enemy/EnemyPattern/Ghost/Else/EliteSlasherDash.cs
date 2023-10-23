using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSlasherDash : MonoBehaviour
{
    BoxCollider2D getCol;
    [SerializeField] GameObject warning;

    private void Awake()
    {
        getCol = GetComponent<BoxCollider2D>();
    }

    private void OnDisable()
    {
        Collider2D col = Physics2D.OverlapBox(transform.position, new Vector2(50,1f), warning.transform.eulerAngles.z, 1<<8);
        if (col != null)
            GameManager.Instance.Player.OnDamage(10, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(50, 0.5f));
    }
}
