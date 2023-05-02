using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashParts : MonoBehaviour
{
    GameObject warning;
    GameObject bar;

    SpriteRenderer barRenderer;
    Color mainColor;

    private void Awake()
    {
        warning = transform.Find("Warning").gameObject;
        bar = warning.transform.Find("Bar").gameObject;

        barRenderer = bar.GetComponentInChildren<SpriteRenderer>();
        mainColor = barRenderer.color;
    }

    private void OnEnable()
    {
        StartCoroutine(OnDash());
    }

    private IEnumerator OnDash()
    {
        yield return null;
        warning.SetActive(true);
        bar.transform.localScale = new Vector3(1, 0);

        bar.transform.DOScaleY(1, 2f);

        yield return new WaitForSeconds(2f);
        barRenderer.color = Color.white;
        
        yield return new WaitForSeconds(0.1f);

        barRenderer.color = mainColor;
        warning.SetActive(false);


    }
}
