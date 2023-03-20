using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : PlayerBase 
{
    [Space]
    [Header("변신")]
    public PlayerSkillData[] playerAllDataSOArr; // 모든 변신 데이터
    [field:SerializeField]
    public PlayerSkillData playerCurrentDataSO { private set; get; }      // 현재 변신 데이터

    private void Awake()
    {
        SkillData = playerCurrentDataSO;
    }
    public void TransformGhost()
    {
        PlayerTransformTypeFlag = Define.PlayerTransformTypeFlag.Ghost;
        playerCurrentDataSO = playerAllDataSOArr[1];
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerCurrentDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }

}
