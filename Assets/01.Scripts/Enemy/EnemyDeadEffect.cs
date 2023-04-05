using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadEffect : MonoBehaviour
{
    private Poolable poolable = null;

    private void OnEnable()
    {
        if(poolable == null)
        {
            poolable = GetComponent<Poolable>();
        }
    }

    private void Start()
    {
        poolable = GetComponent<Poolable>();
    }

    public void PushInPool()
    {
        Managers.Pool.Push(poolable);
    }


}
