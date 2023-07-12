using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class ItemBase : MonoBehaviour
{
    public abstract ItemType itemType { get; }
    public abstract ItemRating itemRating { get; }
    public abstract bool isPersitantItem { get; }
    public abstract void Init();
    public abstract void Use();
    public abstract void Disabling();
    public virtual void LastingEffect()
    {

    }
    public virtual bool isOneOff { get; } = false;
}