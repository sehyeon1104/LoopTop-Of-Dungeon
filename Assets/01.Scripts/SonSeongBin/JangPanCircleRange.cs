using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JangPanCircleRange : MonoBehaviour
{
    IEnumerator PlayerDamaged = null;

    public TestGameManager GameManager = null;

    private bool iscoroutinestart = false;

    public float range;
    public LayerMask Layer;

    WaitForSeconds checkTime = new WaitForSeconds(0.1f);

    private void OnEnable()
    {
        PlayerDamaged = GameManager.DotDamageFunc();
        StartCoroutine(CheckPlayer());
    }

    private void OnDisable()
    {
        StopCoroutine(CheckPlayer());
        StopCoroutine(PlayerDamaged);
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
            else if (hit1?.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                StopCoroutine(PlayerDamaged);
                iscoroutinestart = false;
            }
            yield return checkTime;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }


}

