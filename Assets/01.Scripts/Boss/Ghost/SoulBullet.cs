using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBullet : MonoBehaviour
{
    CircleCollider2D col;
    float nowPosition = 0f;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

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
        Poolable clone;

        nowPosition = transform.localPosition.x;
        yield return null;

        while (timer <= 5f)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.right * (Mathf.Sin(Time.time * 2f) * Mathf.Sign(nowPosition) + nowPosition);
            yield return null;
        }
        transform.localPosition = Vector3.right * nowPosition;


        yield return new WaitForSeconds(0.25f);

        if(col != null)
            clone = Managers.Pool.PoolManaging("10.Effects/ghost/Bubble", transform.position, Quaternion.identity);
        else
            clone = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/BubbleBlue.prefab", transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
            GameManager.Instance.Player.OnDamage(15,0);
    }
}
