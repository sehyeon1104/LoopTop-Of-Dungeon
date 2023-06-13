using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EnemyUIManager : MonoSingleton<EnemyUIManager>
{
    private ShowDamagePopUp showDamagePopUp = null;

    private void Awake()
    {
        showDamagePopUp = FindObjectOfType<ShowDamagePopUp>();
    }

    public void ShowDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        StartCoroutine(showDamagePopUp.IEShowDamage(damage, damagedObj, isCrit));
    }
}