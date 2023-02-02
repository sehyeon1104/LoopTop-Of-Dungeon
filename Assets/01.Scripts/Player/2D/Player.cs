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
}
