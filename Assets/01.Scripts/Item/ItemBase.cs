using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class ItemBase : MonoBehaviour
{
    // 아이템 타입
    public abstract ItemType itemType { get; }
    // 아이템 등급
    public abstract ItemRating itemRating { get; }
    // 지속형 아이템인가?
    public abstract bool isPersitantItem { get; }
    // 세트 아이템 재료인가?
    public virtual bool isSetElement { get; }
    // 초기화
    public abstract void Init();
    // 장착시
    public abstract void Use();
    // 탈착시
    public abstract void Disabling();
    // 지속형일 경우
    public virtual void LastingEffect() { }
    // 세트아이템일 경우
    public virtual void SetItemCheck() { }
    public virtual bool isOneOff { get; } = false;
}