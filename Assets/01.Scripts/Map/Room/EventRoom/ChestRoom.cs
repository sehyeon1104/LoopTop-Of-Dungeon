using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRoom : EventRoom
{
    private void Start()
    {
        SpawnChest();
    }

    protected override void IsClear()
    {
        isClear = true;
    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.ChestRoom;
    }

    public void SpawnChest()
    {
        // TODO : ������ Ȯ���� ���� ��ȯ
        Debug.Log($"���� ��ȯ. ��ǥ : {transform.position}");
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
