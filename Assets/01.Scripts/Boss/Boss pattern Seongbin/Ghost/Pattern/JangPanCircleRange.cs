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

    WaitForSeconds checkTime = new WaitForSeconds(0.1f);

    WaitForSeconds DotTime = new WaitForSeconds(0.8f);

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
            yield return checkTime;
        }
    }

    public IEnumerator DotDamageFunc()
    {
        while (true)
        {
            GameManager.Instance.Player.OnDamage(1, 0);
            yield return DotTime;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}

