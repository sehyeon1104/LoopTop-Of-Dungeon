using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField]
    private GameObject hpBar = null;
    [SerializeField]
    private Image Icon = null;

    [SerializeField]
    private GameObject shieldBar = null;

    private Slider hpBarSlider = null;
    private Slider shieldBarSlider = null;

    private void Awake()
    {
        hpBarSlider = hpBar.GetComponentInChildren<Slider>();
        shieldBarSlider = shieldBar.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        StartCoroutine(UIManager.Instance.ShowCurrentStageName());
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
