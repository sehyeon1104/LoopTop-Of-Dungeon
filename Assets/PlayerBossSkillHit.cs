using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerBossSkillHit : MonoBehaviour
{

    WaitForSeconds DotDamage = new WaitForSeconds(0.8f);

    private bool isenter = false;

    public Coroutine DotDamageCor = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BossSkill"))
        {
            if (isenter == false)
            {
                Debug.Log("内风凭 矫累");
                isenter = true;
                DotDamageCor = StartCoroutine(DotDamageFunc());
            }
            else if (isenter == true)
            {
                Debug.Log("内风凭 场");
                isenter = false;
                StopCoroutine(DotDamageCor);
            }
        }
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Managers.Pool.Push(collision.gameObject.GetComponent<Poolable>());
            BossUI.fillTime += 10;
            Managers.Pool.PoolManaging("10.Effects/ghost/EatBubble", collision.transform.position, Quaternion.identity);

        }

    }

    public IEnumerator DotDamageFunc()
    {
        while (true)
        {
            GameManager.Instance.Player.OnDamage(3, 0);
            yield return DotDamage;
        }
    }

}
