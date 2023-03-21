using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmPattern : MonoBehaviour
{
    public Vector2 size1;



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size1);
    }
}
