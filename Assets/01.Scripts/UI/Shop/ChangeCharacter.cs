using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public void SelectCharacter(int playerTransformTypeFlag)
    {
        PlayerMovement.Instance.IsControl = true;
        GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag = (Define.PlayerTransformTypeFlag)playerTransformTypeFlag;
        GameManager.Instance.UpdatePlayerTransformData();
        UIManager.Instance.shopUI.ToggleChangeCharacterPanel();
    }

    public void Cancle()
    {
        UIManager.Instance.shopUI.ToggleChangeCharacterPanel();
    }
}
