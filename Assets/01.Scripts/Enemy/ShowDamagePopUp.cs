using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ShowDamagePopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject displayDamageTMP = null;

    private WaitForSeconds waitForHalfSeconds = new WaitForSeconds(0.5f);

    public IEnumerator IEShowDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        // 텍스트 소환
        Poolable damageTMP = Managers.Pool.Pop(displayDamageTMP);
        TextMeshProUGUI damageText = damageTMP.GetComponentInChildren<TextMeshProUGUI>();
        damageText.text = damage.ToString();
        if (isCrit)
        {
            // 텍스트 색 변경
            damageText.color = Color.red;
        }
        else
        {
            damageText.color = Color.white;
        }

        // 초기화
        damageTMP.transform.position = damagedObj.transform.position;
        damageTMP.transform.DOScale(0.01f, 0.1f);

        // 오브젝트의 위로 이동
        damageTMP.transform.position = new Vector3(damagedObj.transform.position.x + Random.Range(-damagedObj.transform.localScale.x, damagedObj.transform.localScale.x), damagedObj.transform.position.y, 0);
        damageTMP.transform.DOMoveY(damagedObj.transform.position.y + Random.Range(1.5f, 2.5f), 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);

        // 0.5초후 풀링
        yield return waitForHalfSeconds;
        StartCoroutine(PoolDamageTMP(damageTMP, damageText));
        yield return null;
    }

    public IEnumerator PoolDamageTMP(Poolable damageTMP, TextMeshProUGUI damageText)
    {
        damageTMP.transform.DOMoveY(damageTMP.transform.position.y - Random.Range(1.5f, 2.5f), 1f).SetEase(Ease.InBack).SetUpdate(true);
        damageTMP.transform.DOScale(0.005f, 0.5f);
        damageText.DOFade(0f, 0.5f);
        yield return waitForHalfSeconds;
        Managers.Pool.Push(damageTMP);
    }
}
