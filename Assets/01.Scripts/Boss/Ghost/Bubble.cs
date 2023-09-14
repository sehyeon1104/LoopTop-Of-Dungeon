using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bubble : MonoBehaviour
{
    private Camera mainCam;
    public bool isRed = true;
    private string takeEffect = "";
    private Color partColor = Color.white;

    private void Awake()
    {
        mainCam = Camera.main;
        partColor = isRed ? new Color(56.25f, 0, 2.5f) : new Color(0, 18f, 56.25f);
        takeEffect = isRed ?
            "Assets/10.Effects/ghost/Bubble&Bullet/TakeRageBubble.prefab" :
            "Assets/10.Effects/ghost/Bubble&Bullet/TakeFearBubble.prefab";
    }
    private IEnumerator trailMove()
    {
        GhostBossUI.Instance.ChangeFillTrail.gameObject.SetActive(true);
        GhostBossUI.Instance.FillTrail.Clear();
        GhostBossUI.Instance.ChangeFillTrail.transform.position = transform.position;
        GhostBossUI.Instance.ChangeFillTrail.transform.DOLocalMove(Vector3.zero, 0.4f);
        
        yield return new WaitForSeconds(0.4f);

        GhostBossUI.Instance.ChangeFillTrail.gameObject.SetActive(false);
        Managers.Pool.Push(GetComponent<Poolable>());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            GhostBossUI.Instance.fillTime += isRed ? 10 : -10;
            StartCoroutine(trailMove());
            GhostBossUI.Instance.partMat.SetColor("_SetColor", partColor);
            GhostBossUI.Instance.ultParticle.Play();
            Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_EatBubble.wav");
            Managers.Pool.PoolManaging(takeEffect, transform.position, Quaternion.identity);
        }
    }
}
