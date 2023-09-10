using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 슬롯 상인
public class SlotMerchant : MerchantBase
{
    [SerializeField]
    private int slotNum = 0;
    Button button;

    private void Start()
    {
        button = UIManager.Instance.GetInteractionButton();
    }

    protected override void SetDialogueText()
    {
        dialogueText.Clear();
        //dialogueText.Append("우적우적..");
    }

    // TODO : 상호작용 연결
    protected override void InteractiveWithPlayer()
    {
        isInteractive = true;

        if (GameManager.Instance.sceneType == Define.Scene.Field)
            StageManager.Instance.shop.isInteractive = isInteractive;
        UIManager.Instance.RotateInteractionButton();
        button.onClick.RemoveListener(MerchantFunc);
        button.onClick.AddListener(MerchantFunc);
        //DialogueManager.Instance.SetContentNPos(dialogueText.ToString(), gameObject);
        //if (!DialogueManager.Instance.isDialogue)
        //{
        //    MerchantFunc();
        //}
    }

    protected override void MerchantFunc()
    {
        UIManager.Instance.shopUI.ToggleSkillBookPanel(slotNum);
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
        UIManager.Instance.shopUI.ToggleSkillBookPanel(slotNum, false);
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
