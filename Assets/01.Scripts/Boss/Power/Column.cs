using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    private GameObject warning;
    private GameObject shockWave;
    private Animator columnAnim;

    private WaitForSeconds waitTime = new WaitForSeconds(2.5f);
    private WaitForSeconds delay = new WaitForSeconds(1.5f);

    private int hashDisappear = Animator.StringToHash("Disappear");
    private int hashBoom = Animator.StringToHash("Boom");

    private static int nowBossPhase = 1;
    
    private void Awake()
    {
        warning = transform.Find("Warning").gameObject;
        shockWave = transform.Find("ShockWave").gameObject;
        columnAnim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        StopCoroutine(Attack());

        nowBossPhase = Boss.Instance.bossPattern.NowPhase;

        warning.SetActive(false);
        shockWave.SetActive(false);

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return waitTime;
            shockWave.SetActive(false);
            warning.SetActive(true);

            yield return delay;
            warning.SetActive(false);

            shockWave.SetActive(true);
            Collider2D col = Physics2D.OverlapCircle(transform.position, 4.5f, 1 << 8);
            if (col != null) GameManager.Instance.Player.OnDamage(1, 0);
        }
        yield return waitTime;
        switch (nowBossPhase)
        {
            case 1:
                columnAnim.SetTrigger(hashDisappear);
                break;
            case 2:
                columnAnim.SetTrigger(hashBoom);
                yield return delay;

                break;
        }

        yield return delay;
        Managers.Pool.Push(GetComponent<Poolable>());
    }
}
