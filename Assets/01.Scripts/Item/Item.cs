using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item
{
    public Define.ItemType itemType;        // ������ Ÿ��
    public Define.ItemRating itemRating;    // ������ ���
    public int itemNumber;                  // ������ ��ȣ
    public string itemName;                 // �����۸�
    public string itemNameEng;              // ������ ������
    public string itemEffectDescription;    // ������ ȿ�� ����
    public string itemDescription;          // ������ ����
    public long price;                      // ������ ����
}
