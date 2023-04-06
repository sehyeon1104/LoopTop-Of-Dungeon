using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float minDistance;

    protected Vector3 constScale;

    public void Init()
    {
        constScale = transform.localScale;
    }
    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    public void MoveToPlayer()
    {
        if (Boss.Instance.actCoroutine != null || Boss.Instance.isBDead || Boss.Instance.isBInvincible || Boss.Instance.isStartMotionPlaying) return;

        float playerDistance = Vector2.Distance(Boss.Instance.player.position, transform.position);
        if (playerDistance <= minDistance) return;

        Boss.Instance.bossAnim.anim.SetBool(Boss.Instance._hashMove, true);
        Vector2 dir = (Boss.Instance.player.position - transform.position);
        Vector3 scale = transform.localScale;

        CheckFlipValue(dir, scale);

        transform.Translate((Vector2.up * dir.normalized + Vector2.right * Mathf.Sign(scale.x)) * Time.deltaTime * moveSpeed);
    }
    public Vector3 CheckFlipValue(Vector2 dir, Vector3 scale)
    {
        scale.x = Mathf.Sign(dir.x) * constScale.x;

        if (Mathf.Abs(dir.x) > 0.2f)
            transform.localScale = scale;

        return scale;
    }
}
