using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JangPanRecRange : MonoBehaviour
{
    IEnumerator PlayerDamaged = null;

    private bool iscoroutinestart = true;

    public Vector2 size1;
    public Vector2 size2;
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
            Collider2D hit1 = Physics2D.OverlapBox(transform.position, size1, 0, Layer);
            Collider2D hit2 = Physics2D.OverlapBox(transform.position, size2, 0, Layer);

            if ((hit1?.gameObject.layer == LayerMask.NameToLayer("Player") || hit2?.gameObject.layer == LayerMask.NameToLayer("Player")) && iscoroutinestart == false)
            {
                StopCoroutine(PlayerDamaged);
                iscoroutinestart = true;
                

            }
            else if ((hit1?.gameObject.layer != LayerMask.NameToLayer("Player") && hit2?.gameObject.layer != LayerMask.NameToLayer("Player")) && iscoroutinestart == true)
            {
                StartCoroutine(PlayerDamaged);
                iscoroutinestart = false;
            }
            yield return checkTime;
        }
    }

    public IEnumerator DotDamageFunc()
    {
        while (true)
        {
            GameManager.Instance.Player.OnDamage(1, gameObject, 0);
            yield return DotTime;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, size1);
        Gizmos.DrawWireCube(transform.position, size2);
    }
}
