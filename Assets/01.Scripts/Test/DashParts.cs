using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DashParts : MonoBehaviour
{
    Vector3 startPos;

    GameObject warning;

    int xDir = 0;

    private void Awake()
    {
        startPos = transform.position;

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

        yield return new WaitForSeconds(2f);
        warning.SetActive(false);

        transform.DOMoveX(transform.position.x + xDir * 50f, 0.3f);

        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            GameManager.Instance.Player.OnDamage(20, 0);
        }
    }
}
