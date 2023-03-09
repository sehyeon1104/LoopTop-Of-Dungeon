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
        // TODO : 오브젝트 풀 사용

        // 텍스트 소환
        GameObject damageTMP = InstantiateOrSpawnDamageTMP(); //Instantiate(displayDamageTMP, damagedObj.transform.position, Quaternion.identity);
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

        // 오브젝트의 위로 이동
        damageTMP.transform.position = new Vector3(damagedObj.transform.position.x + Random.Range(-damagedObj.transform.localScale.x, damagedObj.transform.localScale.x), damagedObj.transform.position.y, 0);
        damageTMP.transform.DOMoveY(damagedObj.transform.position.y + Random.Range(1.5f, 2.5f), 1f);
        // 1초후 삭제
        //StartCoroutine(PoolDamageTMP(damageTMP));
        yield return null;
    }

    // 임시 오브젝트 풀링

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

    public IEnumerator PoolDamageTMP(GameObject damageTMP)
    {
        yield return new WaitForSeconds(1f);
        damageTMP.transform.SetParent(gameObject.transform);
        damageTMP.SetActive(false);
    }
}
