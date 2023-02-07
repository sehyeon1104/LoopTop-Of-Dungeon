using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiGage : MonoSingleton<MultiGage>
{
    [System.Serializable]
    public class TargetGageValue
        // 다른 스크립트에서 게이지 value 조정을 하기 위함
    {
        public float value;

        public TargetGageValue(float value = 0f)
        {
            this.value = value;
        }
    }

    [Tooltip("게이지 타입")]
    [SerializeField]
    private Image.FillMethod fillMethod = Image.FillMethod.Horizontal;
    [Tooltip("MultiGageColor와 무관하게 0줄 이하가 되었을 때 그릴 색")]
    [SerializeField]
    private Color nonValueColor = Color.black;
    [Tooltip("순서대로 그려질 색")]
    [SerializeField]
    private Color[] multiGageColor = { Color.red, Color.yellow };
    [Tooltip("한 줄의 게이지 값")]
    [SerializeField]
    private float gageLineValue = 10f;
    [Tooltip("피격 후 딤 게이지가 줄어들기 시작하는 시간")]
    [SerializeField]
    private float dimWaitTime = 0.1f;
    [Tooltip("딤 게이지가 줄어들기 시작 후 다 줄어들때까지 걸리는 시간")]
    [SerializeField]
    private float dimDeleteTime = 0.3f;
    [Tooltip("게이지 잔상 연출")]
    [SerializeField]
    private bool dimEffectOn = true;

    [Tooltip("남은 HP바 줄")]
    [SerializeField]
    private TextMeshProUGUI remainHPLines;

    private GameObject uiCanvasObject;                  // SetActive 용도
    private RectTransform gage1RectTransform = null;
    private RectTransform gage2RectTransform = null;
    private RectTransform gageDim1RectTransform = null;
    private RectTransform gageDim2RectTransform = null;
    private Canvas gage1Canvas;                         // SortOrder 변경 용도
    private Canvas gage2Canvas;
    private Canvas gageDim1Canvas;
    private Canvas gageDim2Canvas;
    private Image gage1Image;                           // FillAmount 조절 용도
    private Image gage2Image;
    private Image gageDim1Image;
    private Image gageDim2Image;
    private IEnumerator gageEffectIE;
    private IEnumerator gageDimEffectIE;
    private TargetGageValue targetGageValue;
    private float prevGageValue;
    private float dimGageValue;
    private int colorIndex = 0;

    private bool isInit = false;

    private void Awake()
    {
        InitProperty();
    }

    public void ObserveStart(TargetGageValue target)
    {
        targetGageValue = target;
        InitProperty();
        InitSetting();
        CalcGage();
        uiCanvasObject.SetActive(true);
        dimGageValue = prevGageValue = targetGageValue.value;

        if (gageEffectIE != null)
            StopCoroutine(gageEffectIE);
        gageEffectIE = IEObserve();
        StartCoroutine(gageEffectIE);
    }

    public void ObserveEnd()
    {
        if (gageDimEffectIE != null)
            StopCoroutine(gageDimEffectIE);
        if (gageEffectIE != null)
            StopCoroutine(gageEffectIE);
        uiCanvasObject.SetActive(false);
    }

    private IEnumerator IEObserve()
    {
        while (true)
        {
            if(prevGageValue != targetGageValue.value)
            {
                CalcGage();
                if (dimEffectOn)
                {
                    if(prevGageValue < targetGageValue.value)
                    {
                        dimGageValue = targetGageValue.value;
                        gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
                        gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
                        gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
                        gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
                        gageDim1Image.fillAmount = targetGageValue.value % gageLineValue / gageLineValue;
                    }
                    if (gageDimEffectIE != null)
                        StopCoroutine(gageDimEffectIE);
                    gageDimEffectIE = IEGageEffect();
                    StartCoroutine(gageDimEffectIE);
                }
            }
            prevGageValue = targetGageValue.value;
            yield return null;
        }
    }

    private IEnumerator IEGageEffect()
    {
        yield return new WaitForSeconds(dimWaitTime);

        float timer = dimDeleteTime;
        int dimColorIndex;
        while(0 < timer)
        {
            dimGageValue = Mathf.Lerp(targetGageValue.value, dimGageValue, timer / dimDeleteTime);
            dimColorIndex = Mathf.FloorToInt(dimGageValue / gageLineValue);

            gageDim1Canvas.sortingOrder = 10001 + dimColorIndex * 2;
            gageDim2Canvas.sortingOrder = 10001 + dimColorIndex * 2 - 2;
            gageDim1Image.color = 0 <= dimColorIndex ? multiGageColor[dimColorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim2Image.color = 1 <= dimColorIndex ? multiGageColor[(dimColorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
            gageDim1Image.fillAmount = targetGageValue.value % gageLineValue / gageLineValue;

            yield return null;
            timer -= Time.deltaTime;
        }

        gageDim1Canvas.sortingOrder = 10001 + colorIndex * 2;
        gageDim2Canvas.sortingOrder = 10001 + colorIndex * 2 - 2;
        gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] * 0.5f : nonValueColor;
        gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] * 0.5f : nonValueColor;
        gageDim1Image.fillAmount = targetGageValue.value % gageLineValue / gageLineValue;
    }




    private void InitProperty()
    {
        if (isInit) return;

        uiCanvasObject = transform.GetChild(0).gameObject;
        gage1RectTransform = uiCanvasObject.transform.Find("Gage1").GetComponent<RectTransform>();
        gage2RectTransform = uiCanvasObject.transform.Find("Gage2").GetComponent<RectTransform>();
        gageDim1RectTransform = uiCanvasObject.transform.Find("GageDim1").GetComponent<RectTransform>();
        gageDim2RectTransform = uiCanvasObject.transform.Find("GageDim2").GetComponent<RectTransform>();
        gage1Canvas = gage1RectTransform.GetComponent<Canvas>();
        gage2Canvas = gage2RectTransform.GetComponent<Canvas>();
        gageDim1Canvas = gageDim1RectTransform.GetComponent<Canvas>();
        gageDim2Canvas = gageDim2RectTransform.GetComponent<Canvas>();
        gage1Image = gage1RectTransform.GetComponent<Image>();
        gage2Image = gage2RectTransform.GetComponent<Image>();
        gageDim1Image = gageDim1RectTransform.GetComponent<Image>();
        gageDim2Image = gageDim2RectTransform.GetComponent<Image>();

        isInit = true;
    }

    private void InitSetting()
    {
        gage1Canvas.overrideSorting = gage2Canvas.overrideSorting = gageDim1Canvas.overrideSorting = gageDim2Canvas.overrideSorting = true;
        gage1Image.fillMethod = gage2Image.fillMethod = gageDim1Image.fillMethod = gageDim2Image.fillMethod = fillMethod;

        gageDim1Canvas.sortingOrder = gageDim2Canvas.sortingOrder = 10000;
        gageDim1Image.color = gageDim2Image.color = nonValueColor;
        gageDim1Image.fillAmount = gageDim2Image.fillAmount = 0;
        gage2Image.fillAmount = gageDim2Image.fillAmount = 1;
    }

    private void CalcGage()
    {
        colorIndex = Mathf.FloorToInt(targetGageValue.value / gageLineValue);

        gage1Canvas.sortingOrder = 10002 + colorIndex * 2;
        gage2Canvas.sortingOrder = 10002 + colorIndex * 2 - 2;
        gage1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] : nonValueColor;
        gage2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex  - 1) % multiGageColor.Length] : nonValueColor;
        gage1Image.fillAmount = targetGageValue.value % gageLineValue / gageLineValue;
        remainHPLines.text = string.Format("x{0}", Mathf.FloorToInt(Boss.Instance.Base.Hp / gageLineValue));
    }

    private void CalcGageDim()
    {
        gageDim1Canvas.sortingOrder = 10002 + colorIndex * 2;
        gageDim2Canvas.sortingOrder = 10002 + colorIndex * 2 - 2;
        gageDim1Image.color = 0 <= colorIndex ? multiGageColor[colorIndex % multiGageColor.Length] : nonValueColor;
        gageDim2Image.color = 1 <= colorIndex ? multiGageColor[(colorIndex - 1) % multiGageColor.Length] : nonValueColor;
        gageDim1Image.fillAmount = targetGageValue.value % gageLineValue / gageLineValue;
    }

}
