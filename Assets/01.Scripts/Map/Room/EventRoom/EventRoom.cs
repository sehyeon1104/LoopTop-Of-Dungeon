using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EventRoom : MonoBehaviour
{
    protected Define.EventRoomTypeFlag _eventRoomTypeFlag;
    protected GameObject minimapIcon = null;

    public void SetEventRoomType(Define.EventRoomTypeFlag eventRoomTypeFlag)
    {
        _eventRoomTypeFlag = eventRoomTypeFlag;
        SetEvent();
    }

    protected void SetEvent()
    {
        switch (_eventRoomTypeFlag)
        {
            // TODO : 미니맵 아이콘 생성
            case EventRoomTypeFlag.StatueRoom:
                gameObject.AddComponent<StatueRoom>();
                break;
            //case EventRoomTypeFlag.ChestRoom:
                //gameObject.AddComponent<ChestRoom>();
                //break;
            case EventRoomTypeFlag.BrokenItemRoom:
                gameObject.AddComponent<BrokenItemRoom>();
                break;
            //case EventRoomTypeFlag.BattleRoom:
                //break;
            //case EventRoomTypeFlag.SurvivalRoom:
                //break;
        }

    }
}
