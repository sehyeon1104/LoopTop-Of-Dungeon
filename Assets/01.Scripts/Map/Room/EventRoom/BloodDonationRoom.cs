using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonationRoom : RoomBase
{
    private GameObject statue = null;
    private GameObject minimapIcon = null;

    protected override void Awake()
    {
        base.Awake();

        SpawnStatue();
        minimapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/StatueRoomIcon.prefab", transform);
        minimapIcon.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 1.5f);
        minimapIcon.SetActive(false);
    }

    protected override void IsClear()
    {
        isClear = true;
    }

    protected override void ShowIcon()
    {
        base.ShowIcon();
        minimapIcon.SetActive(true);
    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.EventRoom;
    }

    public void SpawnStatue()
    {
        statue = Managers.Resource.Instantiate("Assets/03.Prefabs/Statue/BloodDonationStatue.prefab", transform);
    }
}
