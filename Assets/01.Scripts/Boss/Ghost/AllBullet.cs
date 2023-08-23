using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBullet : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(OnRotate());
    }
    private IEnumerator OnRotate()
    {
        float timer = 0f;
        transform.position = GameManager.Instance.Player.transform.position;
        yield return null;

        while (timer <= 4.5f)
        {
            transform.position = Vector2.Lerp(transform.position, GameManager.Instance.Player.transform.position, Time.deltaTime * 10f);
            transform.Rotate(Vector3.forward * (4.5f - timer) * 0.15f);
            timer += Time.deltaTime;

            yield return null;
        }

    }
}
