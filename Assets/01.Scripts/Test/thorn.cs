using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thorn : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
            //IHittable hittable= collision.GetComponent<IHittable>();
            //hittable.OnDamage(1);
        }
    }
}
