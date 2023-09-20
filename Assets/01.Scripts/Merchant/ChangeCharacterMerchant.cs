using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacterMerchant : MerchantBase
{
    Button button;

    private void Start()
    {
        button = UIManager.Instance.GetInteractionButton();
    }

    protected override void SetDialogueText()
    {
        dialogueText.Clear();
    }


    protected override void InteractiveWithPlayer()
    {
        base.InteractiveWithPlayer();

        isInteractive = true;
        interactionTMP.gameObject.SetActive(false);
        if (GameManager.Instance.sceneType == Define.Scene.Field)
            StageManager.Instance.shop.isInteractive = isInteractive;
        UIManager.Instance.RotateInteractionButton();
        button.onClick.RemoveListener(MerchantFunc);
        button.onClick.AddListener(MerchantFunc);
    }

    protected override void MerchantFunc()
    {
        PlayerMovement.Instance.IsControl = false;
        UIManager.Instance.shopUI.ToggleChangeCharacterPanel();
    }

    protected override void StandBy()
    {
        if (!isInteractive)
            return;

        isInteractive = false;
        if (GameManager.Instance.sceneType == Define.Scene.Field)
            StageManager.Instance.shop.isInteractive = isInteractive;

        UIManager.Instance.RotateAttackButton();
        button.onClick.RemoveListener(MerchantFunc);
        if (UIManager.Instance.shopUI.isChangeCharacterPanelActive)
            UIManager.Instance.shopUI.ToggleChangeCharacterPanel();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}