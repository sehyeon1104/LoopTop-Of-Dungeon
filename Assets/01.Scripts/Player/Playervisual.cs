using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerVisual : MonoSingleton<PlayerVisual>
{
    SpriteRenderer playerSprite;
    Animator playerAnimator;
    Volume hitVolume;
    private void Awake()
    {
        hitVolume = GameObject.FindGameObjectWithTag("HitVolume").GetComponent<Volume>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
       
    }

    public void StartHitMotion()
    {
        StartCoroutine(IEHitMotion());
    }
    public IEnumerator IEHitMotion()
    {
        float timer = 0f;

        playerSprite.color = Color.red;
        Managers.Pool.PoolManaging("10.Effects/player/Hit_main", transform.position, Quaternion.identity);
        Managers.Pool.PoolManaging("10.Effects/player/Hit_sub", transform.position, Quaternion.identity);
        Managers.Sound.Play("SoundEffects/Player/Damaged.wav");
        while (timer <= 0.25f)
        {
            timer += Time.unscaledDeltaTime;

            Time.timeScale -= 0.015f;
            hitVolume.weight += 0.05f;

            yield return null;
        }
        Time.timeScale = 1f;
        hitVolume.weight = 0;
        playerSprite.color = Color.white;
    }
    public void VelocityChange(float VelocityX)
    {
        playerSprite.flipX = VelocityX < 0 ? false : true;
    }
}
