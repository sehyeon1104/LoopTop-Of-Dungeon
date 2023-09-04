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
            case EventRoomTypeFlag.ChestRoom:
                gameObject.AddComponent<ChestRoom>();
                break;
            case EventRoomTypeFlag.BrokenItemRoom:
                gameObject.AddComponent<BrokenItemRoom>();
                break;
            //case EventRoomTypeFlag.DiceRoom:
            //    gameObject.AddComponent<DiceRoom>();
            //    break;
            //case EventRoomTypeFlag.DevilSwordRoom:
            //    gameObject.AddComponent<DevilSwordRoom>();
            //    break;
            case EventRoomTypeFlag.BloodyAltarRoom:
                gameObject.AddComponent<BloodyAltarRoom>();
                break;
            case EventRoomTypeFlag.BloodDonationRoom:
                gameObject.AddComponent<BloodDonationRoom>();
                break;
            case EventRoomTypeFlag.RedFountainRoom:
                gameObject.AddComponent<RedFountainRoom>();
                break;
        }

    }
}
