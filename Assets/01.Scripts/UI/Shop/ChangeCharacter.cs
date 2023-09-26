using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public void SelectCharacter(int playerTransformTypeFlag)
    {
        PlayerMovement.Instance.IsControl = true;

        PlayerManager.Instance.PrePlayerTransformChangeEffects.AddListener(PlayerSkill.Instance.skillData[GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag].ToOtherForm);
        PlayerManager.Instance.PrePlayerTransformChangeEffects.Invoke();
        GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag = (Define.PlayerTransformTypeFlag)playerTransformTypeFlag;
        GameManager.Instance.UpdatePlayerTransformData();
        UIManager.Instance.shopUI.ToggleChangeCharacterPanel();
    }

    public void Cancle()
    {
        UIManager.Instance.shopUI.ToggleChangeCharacterPanel();
    }
}
