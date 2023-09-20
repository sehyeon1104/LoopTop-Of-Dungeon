using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public abstract class MerchantBase : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI interactionTMP = null;

    private float interactiveDis = 2f;

    protected StringBuilder dialogueText = new StringBuilder();

    protected bool isInteractive = false;

    protected abstract void SetDialogueText();
    protected virtual void InteractiveWithPlayer()
    {
        interactionTMP.SetText($"{KeySetting.keys[KeyAction.INTERACTION]} 상호작용");
        interactionTMP.gameObject.SetActive(true);
    }
    protected abstract void StandBy();
    protected abstract void MerchantFunc();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
}
