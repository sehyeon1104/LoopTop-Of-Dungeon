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

    public IEnumerator showDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        // TODO : ������Ʈ Ǯ ���

        // ������Ʈ ��ȯ
        GameObject damageTMP = Instantiate(displayDamageTMP, damagedObj.transform.position, Quaternion.identity);
        TextMeshProUGUI damageText = damageTMP.GetComponentInChildren<TextMeshProUGUI>();
        damageText.text = damage.ToString();
        if (isCrit)
        {
            // ������Ʈ�� �� ����
            damageTMP.GetComponentInChildren<Image>().color = Color.red;
            damageText.color = Color.white;
        }

        // ������Ʈ�� ���� �̵�
        damageTMP.transform.position = new Vector3(damagedObj.transform.position.x + Random.Range(-damagedObj.transform.localScale.x, damagedObj.transform.localScale.x), damagedObj.transform.position.y, 0);
        damageTMP.transform.DOMoveY(damagedObj.transform.position.y + Random.Range(1.5f, 2f), 1f);
        yield return new WaitForSeconds(1f);
        // 1���� ����
        Destroy(damageTMP);
    }
}
