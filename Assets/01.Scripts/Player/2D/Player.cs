using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    public PlayerBase pBase;

    private void Awake()
    {
        pBase = new PlayerBase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Boss.Instance.isDead)
            {
                PlayerTransformation.Instance.TransformGhost();
                Boss.Instance.gameObject.SetActive(false);
            }
        }
    }
}
