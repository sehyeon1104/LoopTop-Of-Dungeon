using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GhostBossUI : MonoSingleton<GhostBossUI>
{
    [SerializeField] private Image bossUltGageImages;

    [SerializeField] private CanvasGroup atkGroup;
    [SerializeField] private CanvasGroup defGroup;
    [SerializeField] private TextMeshProUGUI conditionTxt;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    public float fillTime { get; set; } = 50f;

    public Image ChangeFillTrail;
    public Image ultArrow;

    public ParticleSystem ultParticle;
    public Material partMat;

    private void Start()
    {
        InitUltGage();
        StartCoroutine(FillBossUltGage(100f));
    }

    public void InitUltGage()
    {
        GetComponent<Canvas>().worldCamera = GameObject.Find("UICam").GetComponent<Camera>();
        partMat = ultParticle.GetComponent<ParticleSystemRenderer>().material;
        bossUltGageImages.fillAmount = 0;

        atkGroup.alpha = 0.2f;
        defGroup.alpha = 0.2f;
    }

    public IEnumerator FillBossUltGage(float time)
    {
        while (true)
        {
            bossUltGageImages.fillAmount = fillTime / time;
            ultArrow.rectTransform.localPosition = new Vector2((bossUltGageImages.fillAmount - 1) * 1400 + 700, 0f);

            if (fillTime > 0)
            {
                fillTime -= 0.01f;
            }
            if (fillTime > 100)
                fillTime = 100;
            else if (fillTime < 0)
                fillTime = 0;

            switch(fillTime)
            {
                case var a when a >= 70:
                    defGroup.alpha = 0.2f;
                    atkGroup.alpha = 1;
                    conditionTxt.text = "<color=#CF0200>RAGE";
                    break;
                case var a when a <= 30:
                    atkGroup.alpha = 0.2f;
                    defGroup.alpha = 1;
                    conditionTxt.text = "<color=#6180C0>FEAR";
                    break;
                default:
                    atkGroup.alpha = 0.2f;
                    defGroup.alpha = 0.2f;
                    conditionTxt.text = "<color=#E5A2FF>CALM";
                    break;
            }

            yield return waitForFixedUpdate;
        }

    }
}
