using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public GameObject Player;

    WaitForSeconds DotTime = new WaitForSeconds(0.8f);
    

    public IEnumerator DotDamageFunc()
    {
        while (true)
        {
            Player.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
            yield return DotTime;
        }
    }
}
