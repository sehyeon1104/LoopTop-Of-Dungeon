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

    [SerializeField]
    private GameObject shieldBar = null;

    [SerializeField]
    private Image bossUltGageImages = null;

    private Slider hpBarSlider = null;
    private Slider shieldBarSlider = null;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    public static float fillTime { get; set; } = 20f;

    private void Awake()
    {
        hpBarSlider = hpBar.GetComponentInChildren<Slider>();
        shieldBarSlider = shieldBar.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        InitUltGage();
        StartCoroutine(UIManager.Instance.ShowCurrentStageName());
        StartCoroutine(FillBossUltGage(100f));
    }
    public void InitUltGage()
    {
        bossUltGageImages.fillAmount = 0;
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

        while (true)
        {
            bossUltGageImages.fillAmount = fillTime / time;

            if (fillTime > 0)
            {
                fillTime -= Time.deltaTime;
            }
            
            yield return waitForFixedUpdate;
        }
        
    }

    public void UpdateHpBar()
    {
        hpBarSlider.value = (float)Boss.Instance.Base.Hp / (float)Boss.Instance.Base.MaxHp;
    }

    public void UpdateShieldBar()
    {
        shieldBarSlider.value = (float)Boss.Instance.Base.Shield / (float)Boss.Instance.Base.MaxShield;
    }

    public void TogglePhase2Icon()
    {
        Icon.gameObject.GetComponent<Animator>().enabled = true;
    }

}
