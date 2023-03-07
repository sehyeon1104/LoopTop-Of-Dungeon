using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyUIManager : MonoSingleton<EnemyUIManager>
{
    [SerializeField]
    private GameObject displayDamageTMP = null;


    public IEnumerator showDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        // TODO : 오브젝트 풀 사용

        GameObject damageTMP = Instantiate(displayDamageTMP, damagedObj.transform.position, Quaternion.identity);
        TextMeshProUGUI damageText = damageTMP.GetComponentInChildren<TextMeshProUGUI>();
        damageText.text = damage.ToString();
        if (isCrit)
        {
            damageTMP.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            damageText.color = Color.white;
        }

        damageTMP.transform.DOMove(new Vector3(0, damagedObj.transform.position.y + 1f, 0), 3f);
        yield return new WaitForSeconds(1f);
        Destroy(damageTMP);
    }
}
