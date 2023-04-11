using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEntrance : MonoBehaviour
{
    private Define.MapTypeFlag mapTypeFlag;

    private void Start()
    {
        mapTypeFlag = gameObject.name switch
        {
            "Ghost" => Define.MapTypeFlag.Ghost,
            "LavaSlime" => Define.MapTypeFlag.LavaSlime,
            "Electricity" => Define.MapTypeFlag.Electricity,
            "Werewolf" => Define.MapTypeFlag.Werewolf,
            "Lizard" => Define.MapTypeFlag.Lizard,
            "Power" => Define.MapTypeFlag.Power,
            _ => Define.MapTypeFlag.Default,
        };
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SetMapTypeFlag(mapTypeFlag);
        }
    }
}
