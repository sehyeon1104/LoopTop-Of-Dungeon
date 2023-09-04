using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBullet : MonoBehaviour
{
    CircleCollider2D col;
    float nowPosition = 0f;
    float waitTime = 5f;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        if(nowPosition != 0f)
            transform.localPosition = Vector3.right * nowPosition;

        if (waitTime != 1f && Boss.Instance.bossPattern.NowPhase == 2)
            waitTime = 1f;

        if (col != null)
            col.enabled = false;

        StartCoroutine(OnShoot());
    }

    private IEnumerator OnShoot()
    {
        float timer = 0;

        nowPosition = transform.localPosition.x;
        yield return null;

        if (waitTime != 1f)
            while (timer <= waitTime)
            {
                timer += Time.deltaTime;
                transform.localPosition = Vector3.right * (Mathf.Sin(Time.time * 2f) * Mathf.Sign(nowPosition) + nowPosition);
                yield return null;
            }
        else
            yield return new WaitForSeconds(waitTime);

        transform.localPosition = Vector3.right * nowPosition;
        if(col != null)
            col.enabled = true;
        yield return new WaitForSecondsRealtime(0.25f);

        int random = waitTime == 1f ? 1 : Random.Range(0, 2);
        if(random == 1)
        {
            if (col != null)
                Managers.Pool.PoolManaging("10.Effects/ghost/Bubble", transform.position, Quaternion.identity);
            else
                Managers.Pool.PoolManaging("Assets/10.Effects/ghost/BubbleBlue.prefab", transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
            GameManager.Instance.Player.OnDamage(15,0);
    }
}
