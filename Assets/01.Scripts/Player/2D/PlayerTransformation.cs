using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformation : MonoSingleton<PlayerTransformation>
{
    [field:SerializeField]
    public PlayerTransformData playerTransformDataSO { private set; get; }      // ���� ���� ������

    [SerializeField]
    private PlayerTransformData[] playerTransformDataSOArr; // ��� ���� ������



}
