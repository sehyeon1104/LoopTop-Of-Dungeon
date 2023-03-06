using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thorn : MonoBehaviour
{
    private void OnEnable()
    {
        CinemachineCameraShaking.Instance.CameraShake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
            //IHittable hittable= collision.GetComponent<IHittable>();
            //hittable.OnDamage(1);
        }
    }
}
