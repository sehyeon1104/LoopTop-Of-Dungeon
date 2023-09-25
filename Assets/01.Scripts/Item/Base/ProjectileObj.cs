using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObj : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 10f;
    [SerializeField]
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
        StartCoroutine(Pool(moveTime));
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    protected virtual IEnumerator Pool(float PoolTime)
    {
        if (PoolTime != 0)
            yield return new WaitForSeconds(PoolTime);
        else
            yield return null;
        Managers.Pool.Push(poolable);
    }
}
