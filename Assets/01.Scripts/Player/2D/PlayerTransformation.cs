using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public partial class Player
{
    [field:SerializeField]
    public PlayerTransformData playerTransformDataSO { private set; get; }      // ���� ���� ������

    [SerializeField]
    private PlayerTransformData[] playerTransformDataSOArr; // ��� ���� ������

    public void TransformGhost()
    {
        playerTransformDataSO = playerTransformDataSOArr[1];
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerTransformDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }

}
