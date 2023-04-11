using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerBossSkillHit : MonoBehaviour
{
    private Collider2D col;

    WaitForSeconds DotDamage = new WaitForSeconds(0.8f);

    public Coroutine DotDamageCor = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Bubble"))
        {
            Managers.Pool.Push(collision.gameObject.GetComponent<Poolable>());
            BossUI.fillTime += 10;
            Managers.Pool.PoolManaging("10.Effects/ghost/EatBubble", collision.transform.position, Quaternion.identity);

        }

    }

}
