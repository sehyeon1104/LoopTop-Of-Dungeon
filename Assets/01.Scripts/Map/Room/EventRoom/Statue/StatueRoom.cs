using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRoom : EventRoom
{
    private void Start()
    {
        SpawnStatue();
    }

    protected override void IsClear()
    {
        isClear = true;
    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.EventRoom;
    }

    public void SpawnStatue()
    {
        // TODO : ·£´ýÇÑ È®·ü·Î ½Å»ó ¼ÒÈ¯

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
