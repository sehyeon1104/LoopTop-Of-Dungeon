using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : PlayerBase 
{
    [Space]
    [Header("����")]
   
    public PlayerSkillData[] playerTransformDataSOArr; // ��� ���� ������
    [field:SerializeField]
    public PlayerSkillData playerTransformDataSO { private set; get; }      // ���� ���� ������

    private void Start()
    {
        playerTransformDataSO = playerTransformDataSOArr[(int)PlayerTransformTypeFlag];
    }

    public void TransformGhost()
    {
        PlayerTransformTypeFlag = Define.PlayerTransformTypeFlag.Ghost;
        playerTransformDataSO = playerTransformDataSOArr[1];
        Animator animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = playerTransformDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }
    public void TransformAilen()
    {

        Time.timeScale = 0;
        Boss.Instance.gameObject.SetActive(false);
        UIManager.Instance.pressF.gameObject.SetActive(false);
    }

}
