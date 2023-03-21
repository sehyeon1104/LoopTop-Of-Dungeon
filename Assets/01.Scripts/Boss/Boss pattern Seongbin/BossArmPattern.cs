using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BossArmPattern : MonoBehaviour
{
    IEnumerator PlayerDamaged = null;

    public TestGameManager GameManager = null;

    public Vector2 size1;
    public Vector2 size2;
    public LayerMask Layer;

    private bool iscoroutinestart = false;

    WaitForSeconds checkTime = new WaitForSeconds(0.1f);

    private void OnEnable()
    {
        PlayerDamaged = GameManager.DotDamageFunc();
        StartCoroutine(CheckPlayer());
    }

    private void OnDisable()
    {
        
    }

    IEnumerator CheckPlayer()
    {

        while (true)
        {
            Collider2D hit1 = Physics2D.OverlapBox(transform.position, size1, 0, Layer);
            Collider2D hit2 = Physics2D.OverlapBox(transform.position, size2, 0, Layer);

            if ((hit1?.gameObject.layer == LayerMask.NameToLayer("Player") || hit2?.gameObject.layer == LayerMask.NameToLayer("Player")) && iscoroutinestart == false)
            {
                Debug.Log("¤·¤·");
                StopCoroutine(PlayerDamaged);


            }
            else if (hit1?.gameObject.layer != LayerMask.NameToLayer("Player") && hit2?.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                StartCoroutine(PlayerDamaged);
                iscoroutinestart = false;
            }
            yield return checkTime;
        }
    }
}
