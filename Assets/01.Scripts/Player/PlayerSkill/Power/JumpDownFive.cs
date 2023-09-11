using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JumpDownFive : MonoBehaviour
{
    float attack;
    float timer = 0;
    [SerializeField] AnimationCurve circleSizeCurve;
    CircleCollider2D circleCollider;
    private void OnEnable()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        attack = GameManager.Instance.Player.playerBase.Attack;
        StartCoroutine(StartCircle());
    }

    // Update is called once per frame
    IEnumerator StartCircle()
    {
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            circleCollider.radius = 3 * circleSizeCurve.Evaluate(timer) - 0.5f; ;
            yield return null;
        }
        timer = 0;
        circleCollider.radius = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IHittable>().OnDamage(attack * 3 + 20);
        }
    }
}
