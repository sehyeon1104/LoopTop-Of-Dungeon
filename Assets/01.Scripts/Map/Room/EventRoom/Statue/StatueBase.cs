using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public abstract class StatueBase : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI effectTmp = null;

    protected bool isUseable = true;

    protected Button button;

    private float waitMoveTmp = 0.3f;
    private WaitForSeconds waitForMoveTmp;
    private WaitForSeconds waitForDisableTmp = new WaitForSeconds(1.2f);

    protected virtual void Start()
    {
        button = UIManager.Instance.GetInteractionButton();
        waitForMoveTmp = new WaitForSeconds(waitMoveTmp);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isUseable)
        {
            InteractiveWithPlayer();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StandBy();
        }
    }

    protected virtual void InteractiveWithPlayer()
    {
        Debug.Log("상호작용");
        UIManager.Instance.RotateInteractionButton();
    }

    protected virtual void StandBy()
    {
        UIManager.Instance.RotateAttackButton();
    }

    protected abstract void StatueFunc();

    protected IEnumerator IETextAnim()
    {
        effectTmp.transform.DOScale(Vector3.one, 0f);
        effectTmp.gameObject.SetActive(true);

        effectTmp.transform.position = GameManager.Instance.Player.transform.position;
        effectTmp.transform.DOMoveY(transform.position.y + 0.5f, waitMoveTmp).SetEase(Ease.OutCubic);
        // tmp가 올라가는 시간
        yield return waitForMoveTmp;

        // tmp를 읽을 수 있게끔 대기하는 시간
        yield return waitForDisableTmp;

        effectTmp.transform.DOScale(Vector3.zero, waitMoveTmp).SetEase(Ease.InBack);
        yield return waitForMoveTmp;

        effectTmp.gameObject.SetActive(false);
    }
}
