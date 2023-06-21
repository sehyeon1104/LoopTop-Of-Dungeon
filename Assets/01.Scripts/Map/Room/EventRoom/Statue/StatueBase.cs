using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatueBase : MonoBehaviour
{
    protected bool isUseable = true;

    protected Button button;

    protected virtual void Start()
    {
        button = UIManager.Instance.GetInteractionButton();
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
        UIManager.Instance.RotateInteractionButton();
    }

    protected virtual void StandBy()
    {
        UIManager.Instance.RotateAttackButton();
    }

    protected virtual void StatueFunc()
    {
        if (!isUseable)
            return;
        isUseable = false;
    }
}
