using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformation : MonoSingleton<PlayerTransformation>
{
    [field:SerializeField]
    public PlayerTransformData playerTransformDataSO { private set; get; }      // 현재 변신 데이터

    [SerializeField]
    private PlayerTransformData[] playerTransformDataSOArr; // 모든 변신 데이터

    public void TransformGhost()
    {
        playerTransformDataSO = playerTransformDataSOArr[1];
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerTransformDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }

}
