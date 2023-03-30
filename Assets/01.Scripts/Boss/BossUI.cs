using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField]
    private GameObject hpBar = null;
    [SerializeField]
    private GameObject bossUltGage = null;
    [SerializeField]
    private Image Icon = null;

    private Image[] bossUltGageImages = null;

    private Slider hpBarSlider = null;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    public static float fillTime { get; set; } = 0f;
    float firstFill = 0f;
    float secondFill = 0f;
    float thirdFill = 0f;

    private void Awake()
    {
        bossUltGageImages = bossUltGage.GetComponentsInChildren<Image>();
        hpBarSlider = hpBar.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        InitUltGage();
        StartCoroutine(UIManager.Instance.ShowCurrentStageName());
        StartCoroutine(FillBossUltGage(100f));
    }
    public void InitUltGage()
    {
        for(int i = 0; i < bossUltGageImages.Length; ++i)
        {
            bossUltGageImages[i].fillAmount = 0;
        }
        // bossUltGage.SetActive(false);
    }


    /// <summary>
    /// ±Ã±Ø±â ÆÄÈÑ ½Ã°£ ³Ö¾îÁÖ±â
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FillBossUltGage(float time)
    {
        // bossUltGage.SetActive(true);

        firstFill = time * 0.4f;
        secondFill = time - (2 * firstFill);
        thirdFill = time * 0.6f;

        while (true)
        {
            if(fillTime <= firstFill)
            {
                bossUltGageImages[0].fillAmount = 1 * (fillTime / firstFill);
            }
            else if(fillTime > firstFill && fillTime <= thirdFill)
            {
                bossUltGageImages[1].fillAmount = 1 * ((fillTime - firstFill) / secondFill);
            }
            else if(fillTime > thirdFill)
            {
                bossUltGageImages[2].fillAmount = 1 * ((fillTime - (firstFill + secondFill)) / firstFill);
            }
            
            
            yield return waitForFixedUpdate;
        }
        
    }

    public void UpdateHpBar()
    {
        hpBarSlider.value = (float)Boss.Instance.Base.Hp / (float)Boss.Instance.Base.MaxHp;
    }

    public void TogglePhase2Icon()
    {
        Icon.gameObject.GetComponent<Animator>().enabled = true;
    }

}
