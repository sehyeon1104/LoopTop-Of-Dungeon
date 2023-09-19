using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallDamage : MonoBehaviour
{
    PlayerBase playerBase;
    private void Awake()
    {
        playerBase = GameManager.Instance.Player.playerBase;
    }

}
