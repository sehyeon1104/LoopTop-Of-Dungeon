using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public partial class Player
{
    [Space]
    [Header("����")]
    [SerializeField]
    private PlayerTransformData[] playerTransformDataSOArr; // ��� ���� ������
    [field:SerializeField]
    public PlayerTransformData playerTransformDataSO { private set; get; }      // ���� ���� ������


    public void TransformGhost()
    {
        playerTransformDataSO = playerTransformDataSOArr[1];
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerTransformDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }

}
