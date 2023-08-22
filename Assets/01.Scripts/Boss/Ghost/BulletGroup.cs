using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : MonoBehaviour
{
    [SerializeField] private GameObject[] warning;
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        StartCoroutine(OnShoot());
    }

    private IEnumerator OnShoot()
    {
        yield return new WaitForSeconds(4.5f);

        for(int i = 0; i < warning.Length; i++)
            warning[i].SetActive(true);

        transform.localScale += Vector3.right * 0.1f;
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < warning.Length; i++)
            warning[i].SetActive(false);

        while (transform.localScale.x >= -10)
        {
            transform.localScale -= Vector3.right * 0.05f;
            yield return null;
        }
        yield return null;
    }
}
