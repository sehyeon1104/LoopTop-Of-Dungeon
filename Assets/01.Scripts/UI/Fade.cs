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

    private void Awake()
    {
        if(fadeImg == null)
            fadeImg = GameManager.Instance.platForm == Define.PlatForm.PC ? UIManager.Instance.playerPCUI.transform.Find("Fade/FadeImage").GetComponent<Image>() : UIManager.Instance.playerUI.transform.Find("Fade/FadeImage").GetComponent<Image>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if(GameManager.Instance.sceneType != Define.Scene.BossScene)
        {
            fadeImg.gameObject.SetActive(true);
            fadeImg.fillAmount = 1f;
            FadeOut();
        }
    }

    /// <summary>
    /// È­¸é °¡¸²
    /// </summary>
    public void FadeInAndLoadScene(Define.Scene sceneType, float speed = 2f)
    {
        StartCoroutine(IEFadeIn(sceneType, speed));
    }

    public IEnumerator IEFadeIn(Define.Scene sceneType, float speed)
    {
        fadeImg.gameObject.SetActive(true);
        fadeImg.fillAmount = 0f;

        fadeImg.fillOrigin = (int)Image.OriginHorizontal.Right;

        while (fadeImg.fillAmount < 1f)
        {
            fadeImg.fillAmount += Time.deltaTime * speed;

            if(fadeImg.fillAmount > 1f)
            {
                fadeImg.fillAmount = 1f;
            }

            yield return waitFrame;
        }

        //fadeImg.gameObject.SetActive(false);

        Managers.Scene.LoadScene(sceneType);

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
        fadeImg.fillAmount = 1f;

        fadeImg.fillOrigin = (int)Image.OriginHorizontal.Left;

        while (fadeImg.fillAmount > 0f)
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
