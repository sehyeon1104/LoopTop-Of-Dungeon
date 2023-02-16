using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thorn : MonoBehaviour
{
    private void OnEnable()
    {
        CinemachineCameraShaking.Instance.CameraShakeOnce();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO : 프로파일링 후 GetComponent를 사용할 것인지 if else문을 써 Singleton을 쓸것인지
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
            //IHittable hittable= collision.GetComponent<IHittable>();
            //hittable.OnDamage(1);
        }
    }
}
