using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool isRed = true;
    private Color partColor = Color.white;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Managers.Pool.Push(GetComponent<Poolable>());
            GhostBossUI.Instance.fillTime += isRed ? 10 : -10;
            partColor = isRed ? new Color(56.25f, 0, 2.5f) : new Color(0, 18f, 56.25f);

            GhostBossUI.Instance.partMat.SetColor("_SetColor", partColor);
            GhostBossUI.Instance.ultParticle.Play();
            Managers.Pool.PoolManaging("10.Effects/ghost/EatBubble", transform.position, Quaternion.identity);
        }
    }
}
