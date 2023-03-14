using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerBossSkillHit : MonoBehaviour
{

    WaitForSeconds DotDamage = new WaitForSeconds(0.8f);

 
     

    IEnumerator DotDamageFunc()
    {
        while (true)
        {
            gameObject.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
            yield return DotDamage;
        }
    }

}
