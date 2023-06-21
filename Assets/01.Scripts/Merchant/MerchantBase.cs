using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class MerchantBase : MonoBehaviour
{
    private float interactiveDis = 2f;

    protected StringBuilder dialogueText = new StringBuilder();

    protected bool isInteractive = false;

    protected abstract void SetDialogueText();
    protected abstract void InteractiveWithPlayer();
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
