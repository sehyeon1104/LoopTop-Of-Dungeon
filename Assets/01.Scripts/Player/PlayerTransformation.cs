using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Transformation Class
public class PlayerTransformation : MonoBehaviour 
{
    [Space]
    [Header("변신")]
    PlayerBase playerBase;
    public PlayerSkillData[] playerTransformDataSOArr; // 모든 변신 데이터
    [field:SerializeField]
    public PlayerSkillData playerTransformDataSO { private set; get; }      // 현재 변신 데이터
    private void Awake()
    {
        playerBase = GameManager.Instance.Player.playerBase;
    }
    private void Start()
    {
        playerTransformDataSO = playerTransformDataSOArr[(int)playerBase.PlayerTransformTypeFlag];
    }

    public void TransformGhost()
    {
        playerBase.PlayerTransformTypeFlag = Define.PlayerTransformTypeFlag.Ghost;
        playerTransformDataSO = playerTransformDataSOArr[1];
        RuntimeAnimatorController animator = GameManager.Instance.Player.playerBase.SkillData.playerAnim;
        animator = playerTransformDataSO.playerAnim;
        UIManager.Instance.UpdateUI();
    }
    public void TransformAilen()
    {

        Time.timeScale = 0;
        Boss.Instance.gameObject.SetActive(false);
        UIManager.Instance.pressF.gameObject.SetActive(false);
    }

}
