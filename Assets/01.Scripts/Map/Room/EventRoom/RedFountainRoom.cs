using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFountainRoom : RoomBase
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player") && !isClear)
        {
            IsClear();
        }
    }

    protected override void IsClear()
    {
        isClear = true;
        ItemManager.Instance.RoomClearRelatedItemEffects?.Invoke();
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
        statue = Managers.Resource.Instantiate("Assets/03.Prefabs/Statue/RedFountainStatue.prefab", transform);
    }
}
