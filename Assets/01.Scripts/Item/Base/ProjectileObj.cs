using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObj : MonoBehaviour
{
    protected float moveSpeed = 10f;
    protected float moveTime = 5f;

    private Poolable poolable = null;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        poolable = GetComponent<Poolable>();
    }

    protected virtual void OnEnable()
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
