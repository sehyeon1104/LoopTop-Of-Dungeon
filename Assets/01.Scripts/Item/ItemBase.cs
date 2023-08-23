using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class ItemBase : MonoBehaviour
{
    // ������ Ÿ��
    public abstract ItemType itemType { get; }
    // ������ ���
    public abstract ItemRating itemRating { get; }
    // ������ �������ΰ�?
    public abstract bool isPersitantItem { get; }
    // ��Ʈ ������ ����ΰ�?
    public virtual bool isSetElement { get; }
    // �ʱ�ȭ
    public abstract void Init();
    // ������
    public abstract void Use();
    // Ż����
    public abstract void Disabling();
    // �������� ���
    public virtual void LastingEffect() { }
    // ��Ʈ�������� ���
    public virtual void SetItemCheck() { }
    public virtual bool isOneOff { get; } = false;
}