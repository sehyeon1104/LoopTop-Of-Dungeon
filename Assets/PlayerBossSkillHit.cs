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
            if(isenter == false)
            {
                isenter = true;
                DotDamageCor =  StartCoroutine(DotDamageFunc());
            }
            else if (isenter == true)
            {
                isenter = false;
                StopCoroutine(DotDamageCor);
            }
        }
        
    }

    public IEnumerator DotDamageFunc()
    {
        
            while (true)
            {
                gameObject.GetComponentInParent<IHittable>().OnDamage(1, gameObject, 0);
                yield return DotDamage;

            }
    }

}
