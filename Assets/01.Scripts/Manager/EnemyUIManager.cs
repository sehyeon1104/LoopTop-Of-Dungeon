using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EnemyUIManager : MonoSingleton<EnemyUIManager>
{
    [SerializeField]
    private GameObject displayDamageTMP = null;

    private WaitForSeconds waitForHalfSeconds = new WaitForSeconds(0.5f);

    public IEnumerator showDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        // �ؽ�Ʈ ��ȯ
        GameObject damageTMP = InstantiateOrSpawnDamageTMP(); //Instantiate(displayDamageTMP, damagedObj.transform.position, Quaternion.identity);
        TextMeshProUGUI damageText = damageTMP.GetComponentInChildren<TextMeshProUGUI>();
        damageText.text = damage.ToString();
        if (isCrit)
        {
            // �ؽ�Ʈ �� ����
            damageText.color = Color.red;
        }
        else
        {
            damageText.color = Color.white;
        }
        
        // �ʱ�ȭ
        damageTMP.transform.position = damagedObj.transform.position;
        damageTMP.transform.DOScale(0.01f, 0.1f);

        // ������Ʈ�� ���� �̵�
        damageTMP.transform.position = new Vector3(damagedObj.transform.position.x + Random.Range(-damagedObj.transform.localScale.x, damagedObj.transform.localScale.x), damagedObj.transform.position.y, 0);
        damageTMP.transform.DOMoveY(damagedObj.transform.position.y + Random.Range(1.5f, 2.5f), 1f).SetEase(Ease.OutQuart).SetUpdate(true);
        // 1���� ����
        StartCoroutine(PoolDamageTMP(damageTMP, damageText));
        yield return null;
    }

    // �ӽ� ������Ʈ Ǯ��

    private GameObject InstantiateOrSpawnDamageTMP()
    {
        GameObject damageTMP = null;

        if (transform.childCount == 0)
        {
            damageTMP = Instantiate(displayDamageTMP, transform.position, Quaternion.identity);
            displayDamageTMP.transform.SetParent(null);
        }
        else
        {
            damageTMP = gameObject.transform.GetChild(0).gameObject;
            damageTMP.transform.SetParent(null);
            damageTMP.SetActive(true);
        }

        return damageTMP;
    }

    public IEnumerator PoolDamageTMP(GameObject damageTMP, TextMeshProUGUI damageText)
    {
        yield return waitForHalfSeconds;
        damageTMP.transform.DOMoveY(damageTMP.transform.position.y - Random.Range(1.5f, 2.5f), 1f).SetEase(Ease.InQuart).SetUpdate(true);
        damageTMP.transform.DOScale(0.005f, 0.5f);
        damageText.DOFade(0f, 0.5f);
        yield return waitForHalfSeconds;
        damageTMP.transform.SetParent(gameObject.transform);
        damageTMP.SetActive(false);
    }
}
