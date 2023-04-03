using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerVisual : MonoSingleton<PlayerVisual>
{
    SpriteRenderer playerSprite;
    Animator playerAnimator;
    Volume hitVolume;
    Volume weakHitVolume;
    private void Awake()
    {
        hitVolume = GameObject.Find("HitVolume").GetComponent<Volume>();
        weakHitVolume = GameObject.Find("HitVolume_Weak").GetComponent<Volume>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }

    public void UpdateVisual(PlayerSkillData data)
    {
        UIManager.Instance.playerUI.transform.Find("LeftUp/PlayerImg/PlayerIcon").GetComponent<Image>().sprite = data.playerImg;
        playerAnimator.runtimeAnimatorController = data.playerAnim;
        playerSprite.sprite = data.playerImg;
    }
    public void StartHitMotion(float damage = 0)
    {
        StartCoroutine(IEHitMotion(damage));
    }
    public IEnumerator IEHitMotion(float damage = 0)
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

            if(damage >= 2)
                hitVolume.weight += 0.05f; 
            else
                weakHitVolume.weight += 0.05f;
            
            yield return null;
        }
        Time.timeScale = 1f;
        hitVolume.weight = 0;
        weakHitVolume.weight = 0;
        playerSprite.color = Color.white;
    }
    public void VelocityChange(float VelocityX)
    {
        playerSprite.flipX = VelocityX < 0 ? false : true;
    }
    
}
