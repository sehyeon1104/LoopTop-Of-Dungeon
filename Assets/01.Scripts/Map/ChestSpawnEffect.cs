using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnEffect : MonoBehaviour
{

    private Poolable poolable;
    private void Start()
    {
        poolable = GetComponent<Poolable>();
    }

    public void PushEffect()
    {
        Managers.Pool.Push(poolable);
    }

}
