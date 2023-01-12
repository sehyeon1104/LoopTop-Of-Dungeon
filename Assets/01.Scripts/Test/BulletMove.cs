using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        transform.Translate(transform.right * Time.deltaTime * 2);
    }
}
