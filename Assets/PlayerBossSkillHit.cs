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
                Debug.Log("�ڷ�ƾ ����");
                isenter = true;
                DotDamageCor =  StartCoroutine(DotDamageFunc());
            }
            else if (isenter == true)
            {
                Debug.Log("�ڷ�ƾ ��");
                isenter = false;
                StopCoroutine(DotDamageCor);
            }
        }
        if(collision.gameObject.CompareTag("Bubble"))
        {
            //
        }
        
    }

    public IEnumerator DotDamageFunc()
    {
        
            while (true)
            {
            Debug.Log("ƽ Ƚ��");
                gameObject.GetComponentInParent<IHittable>().OnDamage(3, gameObject, 0);
                yield return DotDamage;

            }
    }

}
