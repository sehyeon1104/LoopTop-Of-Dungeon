using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public partial class Player
{
    [Space]
    [Header("변신")]
    [SerializeField]
    private PlayerTransformData[] playerTransformDataSOArr; // 모든 변신 데이터
    [field:SerializeField]
    public PlayerTransformData playerTransformDataSO { private set; get; }      // 현재 변신 데이터


    public void TransformGhost()
    {
        playerTransformDataSO = playerTransformDataSOArr[1];
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerTransformDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }

}
