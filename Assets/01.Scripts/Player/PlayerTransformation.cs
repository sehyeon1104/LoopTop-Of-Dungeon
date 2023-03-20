using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoBehaviour 
{
    [Space]
    [Header("변신")]
   
    public PlayerSkillData[] playerTransformDataSOArr; // 모든 변신 데이터
    [field:SerializeField]
    public PlayerSkillData playerTransformDataSO { private set; get; }      // 현재 변신 데이터

    private void Start()
    {
        playerTransformDataSO = playerTransformDataSOArr[(int)PlayerBase.Instance.PlayerTransformTypeFlag];
    }

    public void TransformGhost()
    {
        PlayerBase.Instance.PlayerTransformTypeFlag = Define.PlayerTransformTypeFlag.Ghost;
        playerTransformDataSO = playerTransformDataSOArr[1];
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerTransformDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }

}
