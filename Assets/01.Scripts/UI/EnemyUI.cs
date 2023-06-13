using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EnemyUI : MonoBehaviour
{
    [SerializeField]
    private Transform damagePopUpRoot = null;
    [SerializeField]
    private TextMeshProUGUI damagePopUpTemplate = null;

    private WaitForSeconds waitForHalfSeconds = new WaitForSeconds(0.5f);

    private Camera mainCamera = null;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //TextMeshProUGUI damageText = GetOrInstantiateDamagePopUp();
            //damageText.gameObject.SetActive(true);
            //damageText.transform.DOScale(1f, 0.1f);
            StartCoroutine(IEShowDamage(Random.Range(0, 20), GameManager.Instance.Player.gameObject));
        }
    }

    public IEnumerator IEShowDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        // 텍스트 소환
        TextMeshProUGUI damageText = GetOrInstantiateDamagePopUp();
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

        // TODO : 데미지 텍스트의 초기 위치를 damagedObj의 위치로


        // 오브젝트의 위로 이동
        damageText.rectTransform.position = new Vector3(damagedObj.transform.position.x + Random.Range(-damagedObj.transform.localScale.x, damagedObj.transform.localScale.x), damagedObj.transform.position.y, 0);
        damageText.rectTransform.DOMoveY(damagedObj.transform.position.y + Random.Range(1.5f, 2.5f), 1f).SetEase(Ease.OutQuart).SetUpdate(true);
        // 1초후 삭제
        StartCoroutine(PoolDamageTMP(damageText));
        yield return null;
    }

    private TextMeshProUGUI GetOrInstantiateDamagePopUp()
    {
        TextMeshProUGUI damagePopUp = null;
        if (damagePopUpRoot.childCount == 1)
        {
            damagePopUp = Instantiate(damagePopUpTemplate, transform);
        }
        else
        {
            damagePopUp = damagePopUpRoot.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        damagePopUp.gameObject.SetActive(true);
        damagePopUp.transform.SetParent(transform);

        return damagePopUp;
    }

    public IEnumerator PoolDamageTMP(TextMeshProUGUI damageText)
    {
        yield return waitForHalfSeconds;
        damageText.rectTransform.DOMoveY(damageText.rectTransform.position.y - Random.Range(1.5f, 2.5f), 1f).SetEase(Ease.InQuart).SetUpdate(true);
        damageText.rectTransform.DOScale(0.5f, 0.5f);
        damageText.DOFade(0f, 0.5f);
        yield return waitForHalfSeconds;
        damageText.rectTransform.SetParent(damagePopUpRoot);
        damageText.gameObject.SetActive(false);
    }
}
