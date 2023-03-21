using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.Experimental.GraphView.GraphView; // 빌드오류로 인해 일시적 주석
public class BossArmPattern : MonoBehaviour
{
    public Player player = null;

    public GameObject AttackRange = null;


    public Vector2 size1;
    public LayerMask Layer;

    private bool iscoroutinestart = false;

    WaitForSeconds checkTime = new WaitForSeconds(0.1f);


    public void CheckPlayer()
    {
            Managers.Pool.PoolManaging("SummonArm",transform.position, Quaternion.identity);
            Collider2D hit1 = Physics2D.OverlapBox(AttackRange.transform.position, size1, 0, Layer);

            if ((hit1?.gameObject.layer == LayerMask.NameToLayer("Player")  && iscoroutinestart == false))
            {
                player.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
            }
            else if (hit1?.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                iscoroutinestart = false;
            }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackRange.transform.position, size1);
    }
}
