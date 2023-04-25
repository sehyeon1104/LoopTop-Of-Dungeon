using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoSingleton<Fade>
{
    [SerializeField]
    private Image fadeImg;

    private WaitForEndOfFrame waitFrame;

    private void Start()
    {
        //Init();
    }

    private void Init()
    {
        if(GameManager.Instance.sceneType != Define.Scene.BossScene)
        {
            fadeImg.gameObject.SetActive(true);
            fadeImg.fillAmount = 1f;
        }
    }

    /// <summary>
    /// È­¸é °¡¸²
    /// </summary>
    public void FadeIn(float speed = 1f)
    {
        StartCoroutine(IEFadeIn(speed));
    }

    public IEnumerator IEFadeIn(float speed)
    {
        fadeImg.gameObject.SetActive(true);

        fadeImg.fillOrigin = (int)Image.OriginHorizontal.Right;

        while (fadeImg.fillAmount >= 1f)
        {
            fadeImg.fillAmount += Time.deltaTime * speed;

            if(fadeImg.fillAmount > 1f)
            {
                fadeImg.fillAmount = 1f;
            }

            yield return waitFrame;
        }

        fadeImg.gameObject.SetActive(false);

        yield break;
    }

    /// <summary>
    /// È­¸é ¹àÈû
    /// </summary>
    public void FadeOut(float speed = 1f)
    {
        StartCoroutine(IEFadeOut(speed));
    }

    public IEnumerator IEFadeOut(float speed)
    {
        fadeImg.gameObject.SetActive(true);

        fadeImg.fillOrigin = (int)Image.OriginHorizontal.Left;

        while (fadeImg.fillAmount <= 0f)
        {
            fadeImg.fillAmount -= Time.deltaTime * speed;

            if (fadeImg.fillAmount < 0f)
            {
                fadeImg.fillAmount = 0f;
            }

            yield return waitFrame;
        }

        fadeImg.gameObject.SetActive(false);

        yield break;
    }

}
