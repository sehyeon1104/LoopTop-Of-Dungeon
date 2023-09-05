using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool isRed = true;
    private string takeEffect = "";
    private Color partColor = Color.white;

    private void Awake()
    {
        partColor = isRed ? new Color(56.25f, 0, 2.5f) : new Color(0, 18f, 56.25f);
        takeEffect = isRed ?
            "Assets/10.Effects/ghost/Bubble&Bullet/TakeRageBubble.prefab" :
            "Assets/10.Effects/ghost/Bubble&Bullet/TakeFearBubble.prefab";
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Managers.Pool.Push(GetComponent<Poolable>());
            GhostBossUI.Instance.fillTime += isRed ? 10 : -10;

            GhostBossUI.Instance.partMat.SetColor("_SetColor", partColor);
            GhostBossUI.Instance.ultParticle.Play();
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_EatBubble.wav");
            Managers.Pool.PoolManaging(takeEffect, transform.position, Quaternion.identity);
        }
    }
}
