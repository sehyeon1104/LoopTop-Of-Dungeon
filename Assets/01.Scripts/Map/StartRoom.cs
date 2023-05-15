using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : RoomBase
{
    private void Start()
    {
        minimapIconSpriteRenderer.color = Color.white;
        GameManager.Instance.minimapCamera.MoveMinimapCamera(transform.position);
    }

    protected override void IsClear()
    {

    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.StartRoom;
    }
}
