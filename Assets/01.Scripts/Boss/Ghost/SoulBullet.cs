using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBullet : MonoBehaviour
{
    float nowPosition = 0f;
    private void OnEnable()
    {
        if(nowPosition != 0f)
        {
            transform.localPosition = Vector3.right * nowPosition;
        }
        StartCoroutine(OnShoot());
    }

    private IEnumerator OnShoot()
    {
        float timer = 0;
        nowPosition = transform.localPosition.x;
        yield return null;

        while (timer <= 5f)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.right * (Mathf.Sin(Time.time * 2f) * Mathf.Sign(nowPosition) + nowPosition);
            yield return null;
        }
        transform.localPosition = Vector3.right * nowPosition;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
            GameManager.Instance.Player.OnDamage(15,0);
    }
}
