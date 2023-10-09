using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Elite_DemonSlasher : EnemyElite
{
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

        yield return new WaitForSeconds(2f);
    }
    protected override IEnumerator Attack2()
    {
        anim.SetTrigger(_attack);
        for (int i = 0; i < 3; i++)
        {
            Managers.Pool.PoolManaging("Assets/10.Effects/ghost/Elite/EliteSlasherRip.prefab", playerTransform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < 3; i++)
        {
            CinemachineCameraShaking.Instance.CameraShake(5, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3f);
    }
    protected override IEnumerator Attack3()
    {
        anim.SetTrigger(_attack);
        yield return new WaitForSeconds(3f);
    }
}
