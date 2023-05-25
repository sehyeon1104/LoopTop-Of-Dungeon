using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MerchantBase : MonoBehaviour
{
    protected List<string> dialogueText = new List<string>();

    protected abstract void SetDialogueText();

}
