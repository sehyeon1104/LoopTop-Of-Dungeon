using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        circleCollider.radius = circleSizeCurve.Evaluate(timer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IHittable>().OnDamage(attack * 3 +20);
        }
    }
}
