using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [Header("풀 사용 여부")]
    public bool IsUsing = false;
    [Header("풀링 유예 시간")]
    public float PushTime = 3f;
    private void Awake()
    {
        StartCoroutine(Push());
    }
    private IEnumerator Push()
    {
        yield return new WaitForSeconds(PushTime);
        Managers.Pool.Push(this);
    }
}
