using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : PlayerBase 
{
    [Space]
    [Header("����")]
    public PlayerSkillData[] playerAllDataSOArr; // ��� ���� ������
    [field:SerializeField]
    public PlayerSkillData playerCurrentDataSO { private set; get; }      // ���� ���� ������

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
