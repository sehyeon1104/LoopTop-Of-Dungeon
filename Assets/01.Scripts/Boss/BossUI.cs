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
    private Image[] bossUltGageImages = null;

    private Slider hpBarSlider = null;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private static BossUI _instance;
    public static BossUI Instance = _instance;

    private void Awake()
    {
        _instance = this;
        bossUltGageImages = bossUltGage.GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        hpBarSlider = hpBar.GetComponentInChildren<Slider>();   

        InitUltGage();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FillBossUltGage(10f));
        }
    }

    public void InitUltGage()
    {
        for(int i = 0; i < bossUltGageImages.Length; ++i)
        {
            bossUltGageImages[i].fillAmount = 0;
        }
    }


    /// <summary>
    /// ±Ã±Ø±â ÆÄÈÑ ½Ã°£ ³Ö¾îÁÖ±â
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FillBossUltGage(float time)
    {
        float fillTime = 0f;
        float firstFill = time * 0.25f;
        float secondFill = time - (2 * firstFill);
        float thirdFill = time * 0.75f;

        int index = 0;

        while (fillTime <= time)
        {
            if(fillTime <= firstFill)
            {
                index = 0;
                bossUltGageImages[index].fillAmount = 1 * (fillTime / firstFill);
            }
            else if(fillTime > firstFill && fillTime <= thirdFill)
            {
                index = 1;
                bossUltGageImages[index].fillAmount = 1 * ((fillTime - firstFill) / secondFill);
            }
            else if(fillTime > thirdFill)
            {
                index = 2;
                bossUltGageImages[index].fillAmount = 1 * ((fillTime - (firstFill + secondFill)) / firstFill);
            }

            fillTime += Time.deltaTime;
            yield return waitForFixedUpdate;
        }

        InitUltGage();
    }

    public void UpdateHpBar()
    {
        hpBarSlider.value = Boss.Instance.Base.Hp / Boss.Instance.Base.MaxHp;
    }

}
