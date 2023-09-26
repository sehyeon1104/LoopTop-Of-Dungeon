using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [Header("Ǯ ��� ����")]
    public bool IsUsing = false;
    [Header("Ǯ�� ���� �ð�")]
    public float PushTime = 0f;
    private void OnEnable()
    {
        StopAllCoroutines();
        if(PushTime != 0)
            StartCoroutine(Push());
    }
    private IEnumerator Push()
    {
        yield return new WaitForSeconds(PushTime);
        Managers.Pool.Push(this);
    }
}
