using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EventRoom : RoomBase
{
    protected Define.EventRoomTypeFlag _eventRoomTypeFlag;

    public void SetEventRoomType(Define.EventRoomTypeFlag eventRoomTypeFlag)
    {
        _eventRoomTypeFlag = eventRoomTypeFlag;
        SetEvent();
    }

    protected override void IsClear()
    {
        throw new System.NotImplementedException();
    }

    protected void SetEvent()
    {
        if(_eventRoomTypeFlag == EventRoomTypeFlag.ChestRoom)
        {
            gameObject.AddComponent<ChestRoom>();
        }
    }
}
