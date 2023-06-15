using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 浇吩 惑牢
public class SlotMerchant : MerchantBase
{
    protected override void SetDialogueText()
    {
        dialogueText.Clear();
        //dialogueText.Append("快利快利..");
    }

    protected override void InteractiveWithPlayer()
    {
        MerchantFunc();
        //DialogueManager.Instance.SetContentNPos(dialogueText.ToString(), gameObject);
        //if (!DialogueManager.Instance.isDialogue)
        //{
        //    MerchantFunc();
        //}
    }

    protected override void MerchantFunc()
    {
        UIManager.Instance.shopUI.ToggleSkillBookPanel();
    }
}
