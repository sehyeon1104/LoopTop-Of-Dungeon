using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMerchant : MerchantBase
{
    protected override void SetDialogueText()
    {
        dialogueText.Clear();
        dialogueText.Append("�������! ������ ��ȭ�ϰ� �����Ű���?");
    }

    protected override void InteractiveWithPlayer()
    {
        DialogueManager.Instance.SetContentNPos(dialogueText.ToString(), gameObject);
    }
}
