using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class MerchantBase : MonoBehaviour
{
    protected StringBuilder dialogueText = new StringBuilder();

    protected abstract void SetDialogueText();
    protected abstract void InteractiveWithPlayer();
    protected abstract void MerchantFunc();
}
