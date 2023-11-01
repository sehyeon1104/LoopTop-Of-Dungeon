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
    [SerializeField]
    protected TextMeshProUGUI interactiveTmp = null;

    protected string acceptText = string.Empty;
    protected string refuseText = "그만두기";
    protected string dialogueText = string.Empty;

    protected bool isUseable = true;

    protected Button button;

    private float waitMoveTmp = 0.3f;
    private WaitForSeconds waitForMoveTmp;
    private WaitForSeconds waitForDisableTmp = new WaitForSeconds(1.2f);

    protected virtual void Start()
    {
        isUseable = true;
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
        ToggleInteractiveTMP();
        UIManager.Instance.RotateInteractionButton();
    }

    protected virtual void ToggleInteractiveTMP()
    {
        interactiveTmp.gameObject.SetActive(false);

        if (!isUseable)
            return;

        interactiveTmp.gameObject.SetActive(!interactiveTmp.gameObject.activeSelf);
        interactiveTmp.SetText($"{KeySetting.keys[KeyAction.INTERACTION]} 상호작용");
    }

    protected virtual void StandBy()
    {
        interactiveTmp.gameObject.SetActive(false);
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

    public void ToggleDialogue()
    {
        DialogueManager.Instance.SetContentNPos(dialogueText, gameObject);

        StartCoroutine(DialogueManager.Instance.IEStartDialogue());
    }
}
