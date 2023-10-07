using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField]
    private Sprite[] bossImgSprite = null;
    [SerializeField]
    private GameObject hpBar = null;
    [SerializeField]
    private Image Icon = null;
    [SerializeField]
    private Image bossImg;
    [SerializeField]
    private Image BackgroundImg = null;
    [SerializeField]
    private TextMeshProUGUI startNameTxt = null;
    [SerializeField]
    private TextMeshProUGUI startDescTxt = null;

    [SerializeField]
    private TextMeshProUGUI hpTxt = null;

    [SerializeField]
    private GameObject shieldBar = null;

    [SerializeField]
    private SignalReceiver bossStartSignal = null;
    [SerializeField]
    private SignalAsset bossStartSignalAsset = null;

    private Slider hpBarSlider = null;
    private Slider shieldBarSlider = null;
    public Material tmpMat = null;

    private void Awake()
    {
        hpBarSlider = hpBar.GetComponentInChildren<Slider>();
        shieldBarSlider = shieldBar.GetComponentInChildren<Slider>();

        tmpMat = startNameTxt.font.material;
    }

    private void Start()
    {
        bossImg.sprite = bossImgSprite[(int)GameManager.Instance.mapTypeFlag];
        switch(GameManager.Instance.mapTypeFlag)
        {
            case Define.MapTypeFlag.Ghost:

                tmpMat.SetColor("_GlowColor", new Color(3f, 1.5f, 4f, 0.5f));
                tmpMat.SetColor("_OutlineColor", new Color(2f, 0.5f, 3f, 1f));

                startNameTxt.text = "<size=70%>카타클리즘";
                startNameTxt.color = new Color(0.85f, 0.75f, 1f);

                startDescTxt.text = "잊혀진 왕의 주검";
                startDescTxt.color = new Color(0.9f, 0.7f, 1f);

                BackgroundImg.color = new Color(0.7f, 0.3f, 0.85f);
                break;
            case Define.MapTypeFlag.Power:

                tmpMat.SetColor("_GlowColor", new Color(5.75f, 1.5f, 0f, 0.5f));
                tmpMat.SetColor("_OutlineColor", new Color(1.75f, 0.5f, 0f, 1f));

                startNameTxt.text = "아틀라스";
                startNameTxt.color = new Color(1f, 0.75f, 0.2f);

                startDescTxt.text = "대지를 지는 거신";
                startDescTxt.color = new Color(1f, 0.8f, 0f);

                BackgroundImg.color = new Color(0.5f, 0.35f, 0f);
                break;
        }
        StartCoroutine(UIManager.Instance.ShowCurrentStageName());
        
        UnityEvent SignalEvent = new UnityEvent();
        SignalEvent.AddListener(Boss.Instance.StartBossAct);
        bossStartSignal.AddReaction(bossStartSignalAsset, SignalEvent);
    }

    public void UpdateHpBar()
    {
        hpBarSlider.value = (float)Boss.Instance.Base.Hp / (float)Boss.Instance.Base.MaxHp;
        hpTxt.text = $"{Mathf.RoundToInt(Boss.Instance.Base.Hp)}/{Boss.Instance.Base.MaxHp} <size=85%>({Mathf.RoundToInt(hpBarSlider.value * 100f)}%)";
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
