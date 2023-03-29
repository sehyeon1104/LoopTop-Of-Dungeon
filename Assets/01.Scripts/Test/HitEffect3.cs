using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect3 : MonoBehaviour
{
    private Animator anim;
    private Poolable poolable;

    private void Start()
    {
        anim = GetComponent<Animator>();
        poolable = GetComponent<Poolable>();
    }

    private void OnEnable()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }

        anim.SetTrigger("Hit");

        Invoke("PopEffect", 0.5f);
    }

    private void PopEffect()
    {
        Managers.Pool.Push(poolable);
    }
}
