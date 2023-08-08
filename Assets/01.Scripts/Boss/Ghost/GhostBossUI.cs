using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostBossUI : MonoBehaviour
{
    [SerializeField] private Image bossUltGageImages;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    public static float fillTime { get; set; } = 50f;

    private void Start()
    {
        InitUltGage();
        StartCoroutine(FillBossUltGage(100f));
    }

    public void InitUltGage()
    {
        bossUltGageImages.fillAmount = 0;
    }

    public IEnumerator FillBossUltGage(float time)
    {
        while (true)
        {
            bossUltGageImages.fillAmount = fillTime / time;

            if (fillTime > 0)
            {
                fillTime -= 0.01f;
            }
            if (fillTime > 100)
                fillTime = 100;
            else if (fillTime < 0)
                fillTime = 0;

            yield return waitForFixedUpdate;
        }

    }
}
