using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class G_Elite_DemonSlasher : EnemyElite
{
    [SerializeField] private GameObject warning;
    private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    public override void Init()
    {
        base.Init();
        warning.SetActive(false);
    }
    protected override IEnumerator Attack1()
    {
        anim.SetBool(_move, true);

        Vector2 dir = Vector2.zero;

        while (distanceToPlayer >= 2f)
        {
            distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

            dir = (playerTransform.position - transform.position).normalized;
            sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);
            rigid.velocity = dir * speed * 2f;

            yield return null;
        }

        anim.SetBool(_move, false);
        rigid.velocity = Vector2.zero;

        dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);

        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.8f);

        Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/EliteSlasherSlash.prefab", transform.position, Quaternion.AngleAxis(-90, Vector3.right));
        clone.transform.localScale = new Vector3(0.75f * Mathf.Sign(dir.x), 0.75f, 0.65f);
        CinemachineCameraShaking.Instance.CameraShake(10, 0.1f);

        Collider2D colSlash = Physics2D.OverlapCircle(clone.transform.position + Vector3.right * 0.75f * Mathf.Sign(dir.x), 3f, 1 << 8);
        Collider2D colRip = Physics2D.OverlapBox(clone.transform.position + new Vector3(3.55f * Mathf.Sign(dir.x), 0.5f), new Vector3(0.75f, 9f), 22 * Mathf.Sign(dir.x), 1 << 8);

        if (colSlash != null || colRip != null)
            GameManager.Instance.Player.OnDamage(20, 0);

        yield return new WaitForSeconds(2f);
    }
    protected override IEnumerator Attack2()
    {
        anim.SetTrigger(_attack);
        Poolable[] clones = new Poolable[3] { null, null, null};
        for (int i = 0; i < 3; i++)
        {
            clones[i] = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Elite/EliteSlasherRip.prefab", playerTransform.position, Quaternion.AngleAxis(Random.Range(0f, 180f), Vector3.forward));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < 3; i++)
        {
            CinemachineCameraShaking.Instance.CameraShake(5, 0.1f);
            Collider2D col = Physics2D.OverlapBox(clones[i].transform.position, new Vector2(1f, 50), clones[i].transform.eulerAngles.z, 1 << 8);
            if (col != null)
                GameManager.Instance.Player.OnDamage(10, 0);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
    }
    protected override IEnumerator Attack3()
    {
        warning.SetActive(true);

        float timer = 0f;
        float angle = 0f;
        Vector3 dir = Vector3.zero;

        while (timer <= 0.5f)
        {
            timer += Time.deltaTime;

            dir = (playerTransform.position - transform.position).normalized;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            warning.transform.localRotation = Quaternion.Lerp(warning.transform.localRotation, Quaternion.AngleAxis(angle - 180, Vector3.forward), Time.deltaTime * 10f);

            yield return endOfFrame;
        }

        yield return new WaitForSeconds(0.5f);

        CinemachineCameraShaking.Instance.CameraShake(8f, 0.1f);
        rigid.DOMove(transform.position + dir * 10f, 0.2f);
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Elite/EliteSlasherTP.prefab", transform.position + dir * 10f, Quaternion.identity);
        warning.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        bool ispFlip = playerTransform.GetComponentInChildren<SpriteRenderer>().flipX;
        int pDir = ispFlip ? 1 : -1;

        transform.position = playerTransform.position + Vector3.right * 3f * pDir;
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Elite/EliteSlasherTP.prefab", transform.position, Quaternion.identity);

        dir = (playerTransform.position - transform.position).normalized;
        sprite.flipX = isFlip != (Mathf.Sign(dir.x) > 0);

        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.8f);

        Poolable clone = Managers.Pool.PoolManaging("Assets/10.Effects/ghost/EliteSlasherSlash.prefab", transform.position, Quaternion.AngleAxis(-90, Vector3.right));
        clone.transform.localScale = new Vector3(0.75f * Mathf.Sign(dir.x), 0.75f, 0.65f);
        CinemachineCameraShaking.Instance.CameraShake(10, 0.1f);

        Collider2D colSlash = Physics2D.OverlapCircle(clone.transform.position + Vector3.right * 0.75f * Mathf.Sign(dir.x), 3f, 1 << 8);
        Collider2D colRip = Physics2D.OverlapBox(clone.transform.position + new Vector3(3.55f * Mathf.Sign(dir.x), 0.5f), new Vector3(0.75f, 9f), 22 * Mathf.Sign(dir.x), 1 << 8);

        if (colSlash != null || colRip != null)
            GameManager.Instance.Player.OnDamage(20, 0);

        yield return new WaitForSeconds(2f);
    }
}
