using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShatterExplosion : MonoBehaviour
{
    private void Awake()
    {
        foreach(Transform child in transform)
        {
            Vector3 explosionPositon = new Vector3(0, 6.34f, -7.896055f);
            if(child.TryGetComponent(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(10, explosionPositon, 10f);
            }
        }

    }
}
