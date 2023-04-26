using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    private GameObject warning;
    private GameObject shockWave;
    private WaitForSeconds waitTime = new WaitForSeconds(2.5f);
    private WaitForSeconds delay = new WaitForSeconds(1.5f);
    
    private void Awake()
    {
        warning = transform.Find("Warning").gameObject;
        shockWave = transform.Find("ShockWave").gameObject;
    }

    private void OnEnable()
    {
        StopCoroutine(Attack());

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
            //이펙트 나오고

            shockWave.SetActive(true);
            Collider2D col = Physics2D.OverlapCircle(transform.position, 4.5f, 1 << 8);
            if (col != null) GameManager.Instance.Player.OnDamage(1, 0);
        }
        yield return waitTime;
        //땅으로 들어가는 애니메이션 재생하고
        Managers.Pool.Push(GetComponent<Poolable>());
    }
}
