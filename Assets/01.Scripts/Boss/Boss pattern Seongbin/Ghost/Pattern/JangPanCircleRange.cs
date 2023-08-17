using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JangPanCircleRange : MonoBehaviour
{
    IEnumerator PlayerDamaged = null;

    private bool iscoroutinestart = false;

    public float range;
    public LayerMask Layer;

    WaitForSeconds DotTime = new WaitForSeconds(0.5f);

    private void OnEnable()
    {
        PlayerDamaged = DotDamageFunc();
        StartCoroutine(CheckPlayer());
        
        
    }

    private void OnDisable()
    {
        StopCoroutine(PlayerDamaged);
        StopCoroutine(CheckPlayer());
        
    }

    IEnumerator CheckPlayer()
    {
        yield return null;

        while (true)
        {
            Collider2D hit1 = Physics2D.OverlapCircle(transform.position, range, Layer);

            if (hit1?.gameObject.layer == LayerMask.NameToLayer("Player") && iscoroutinestart == false)
            {
                StartCoroutine(PlayerDamaged);
                iscoroutinestart = true;
               

            }
            else if (hit1?.gameObject.layer != LayerMask.NameToLayer("Player") && iscoroutinestart == true)
            {
                StopCoroutine(PlayerDamaged);
                iscoroutinestart = false;
            }
            yield return null;
        }
    }

    public IEnumerator DotDamageFunc()
    {
        while (true)
        {
            GameManager.Instance.Player.OnDamage(5, 0);
            yield return DotTime;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}

