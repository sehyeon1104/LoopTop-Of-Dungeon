using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObj : MonoBehaviour
{
    protected float moveSpeed = 5f;
    protected float moveTime = 2f;

    private Poolable poolable = null;

    protected virtual void Init()
    {
        poolable = GetComponent<Poolable>();
    }

    private void OnEnable()
    {
        Invoke("Pool", moveTime);
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    protected virtual void Pool()
    {
        Managers.Pool.Push(poolable);
    }
}
