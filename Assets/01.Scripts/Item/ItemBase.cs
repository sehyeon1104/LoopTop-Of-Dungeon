using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class ItemBase
{
    // ������ Ÿ��
    public abstract ItemType itemType { get; }
    // ������ ���
    public abstract ItemRating itemRating { get; }
    // ������ �������ΰ�?
    public abstract bool isPersitantItem { get; }
    // ��Ʈ ������ ����ΰ�?
    public virtual bool isSetElement { get; }
    // ������ �������ΰ�?
    public virtual bool isStackItem { get; } = false;
    // �ʱ�ȭ
    public abstract void Init();
    // ������
    public abstract void Use();
    // Ż����
    public abstract void Disabling();
    // �������� ���
    public virtual void LastingEffect() { }
    public virtual bool isOneOff { get; } = false;
    public virtual void ShowStack()
    {
        if (!isStackItem)
            return;
    }
}