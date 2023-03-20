using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoBehaviour 
{
    [Space]
    [Header("����")]
   
    public PlayerSkillData[] playerTransformDataSOArr; // ��� ���� ������
    [field:SerializeField]
    public PlayerSkillData playerTransformDataSO { private set; get; }      // ���� ���� ������

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
