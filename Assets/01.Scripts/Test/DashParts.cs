using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DashParts : MonoBehaviour
{
    Vector3 startPos;

    GameObject warning;
    GameObject bar;

    SpriteRenderer barRenderer;
    Color mainColor;

    int xDir = 0;

    private void Awake()
    {
        startPos = transform.position;

        warning = transform.Find("Warning").gameObject;
        bar = warning.transform.Find("Bar").gameObject;

        barRenderer = bar.GetComponentInChildren<SpriteRenderer>();
        mainColor = barRenderer.color;

        string name = gameObject.name;
        string result = Regex.Replace(name, @"[^0-9]", "");

        for(int i = 0; i < result.Length; i++)
        {
            xDir = (result[i] - '0') % 2 == 1 ? 1 : -1;

        }
    }

    private void OnEnable()
    {
        StartCoroutine(OnDash());
    }

    private IEnumerator OnDash()
    {
        yield return null;
        transform.position = startPos;

        warning.SetActive(true);
        bar.transform.localScale = new Vector3(1, 0);

        bar.transform.DOScaleY(1, 2f);

        yield return new WaitForSeconds(2f);
        barRenderer.color = Color.white;
        
        yield return new WaitForSeconds(0.1f);

        barRenderer.color = mainColor;
        warning.SetActive(false);

        transform.DOMoveX(transform.position.x + xDir * 50f, 0.5f);
        yield return new WaitForSeconds(1f);
    }
}
