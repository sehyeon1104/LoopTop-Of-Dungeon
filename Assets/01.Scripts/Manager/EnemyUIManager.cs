using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EnemyUIManager : MonoSingleton<EnemyUIManager>
{
    [SerializeField]
    private EnemyUI enemyUI = null;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (enemyUI == null)
        {
            enemyUI = FindObjectOfType<EnemyUI>();
        }
    }

    public void showDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        StartCoroutine(enemyUI.IEShowDamage(damage, damagedObj, isCrit));
    }
}
