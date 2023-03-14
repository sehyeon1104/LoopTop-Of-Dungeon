using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomBase
{

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.BossRoom;
    }
    protected override void IsClear()
    {
        // TODO : 맵이 클리어 되었는지 체크
    }
}
