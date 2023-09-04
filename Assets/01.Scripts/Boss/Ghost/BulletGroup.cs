using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : MonoBehaviour
{
    [SerializeField] private GameObject[] warning;
    private float waitTime = 4.5f;

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        StartCoroutine(OnShoot());
        if(waitTime != 0.5f && Boss.Instance.bossPattern.NowPhase == 2)
        {
            waitTime = 0.5f;
        }
    }

    private IEnumerator OnShoot()
    {
        float timer = 0f;
        if (waitTime == 0.5f)
        {
            while (timer < waitTime)
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * 300);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
            yield return new WaitForSecondsRealtime(waitTime);

        for (int i = 0; i < warning.Length; i++)
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
