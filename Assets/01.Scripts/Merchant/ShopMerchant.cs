using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMerchant : MerchantBase
{
    protected override void SetDialogueText()
    {
        dialogueText.Clear();
        dialogueText.Append("어서오세요! 슬롯을 강화하고 싶으신가요?");
    }

    protected override void InteractiveWithPlayer()
    {
        DialogueManager.Instance.SetContentNPos(dialogueText.ToString(), gameObject);
    }
}
