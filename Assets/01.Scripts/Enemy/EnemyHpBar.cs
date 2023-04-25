using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField]
    private EnemyDefault enemy = null;
    [SerializeField]
    private Slider slider = null;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<EnemyDefault>();
        slider = transform.Find("EnemyHpBar").GetComponent<Slider>();
    }

    public void UpdateHpBar()
    {
        slider.value = enemy.hp / enemy.maxHp;
    }
}
