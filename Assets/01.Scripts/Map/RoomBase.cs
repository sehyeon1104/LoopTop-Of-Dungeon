using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomBase : MonoBehaviour
{
    [SerializeField]
    protected Define.MapTypeFlag mapTypeFlag;

    [SerializeField]
    protected Define.RoomTypeFlag roomTypeFlag;
    protected bool isClear = false;

    protected abstract void SetRoomTypeFlag();
    protected abstract void IsClear();
    
}
