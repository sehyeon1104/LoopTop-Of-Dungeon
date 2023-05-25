using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMerchant : MerchantBase
{
    protected override void SetDialogueText()
    {
        dialogueText.Clear();
        dialogueText.Append("��������..");
    }

    protected override void InteractiveWithPlayer()
    {
        DialogueManager.Instance.SetContentNPos(dialogueText.ToString(), gameObject);
        if (!DialogueManager.Instance.isDialogue)
        {
            // TODO : ���� ��� ����
        }
    }
}
