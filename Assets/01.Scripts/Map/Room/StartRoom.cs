using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : RoomBase
{
    private void Start()
    {
        minimapIconSpriteRenderer.gameObject.SetActive(true);
        minimapIconSpriteRenderer.color = Color.white;
        GameManager.Instance.minimapCamera.MoveMinimapCamera(transform.position);
        curLocatedMapIcon.SetActive(true);
    }

    protected override void IsClear()
    {

    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.StartRoom;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        isClear = true;
    }
}
