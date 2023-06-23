using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRoom : EventRoom
{
    private int randStatue = 0;
    private GameObject statue = null;
    private GameObject minimapIcon = null;

    private void Start()
    {
        SpawnStatue();
        // 임시
        minimapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/StatueRoomIcon.prefab", transform);
        minimapIcon.transform.position = transform.position;
        minimapIcon.SetActive(false);
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
        // TODO : 랜덤한 확률로 신상 소환
        // 임시
        randStatue = Random.Range(0, 2);
        switch (randStatue)
        {
            case 0:
                statue = Managers.Resource.Instantiate("Assets/03.Prefabs/Statue/DiceStatue.prefab", transform);
                statue.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 3f);
                break;
            case 1:
                statue = Managers.Resource.Instantiate("Assets/03.Prefabs/Statue/SlotStatue.prefab", transform);
                statue.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 3f);
                break;
            case 2:
                statue = Managers.Resource.Instantiate("Assets/03.Prefabs/Statue/SkillShuffleStatue.prefab", transform);
                statue.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 3f);
                break;
        }

    }

    protected override void ShowIcon()
    {
        minimapIcon.SetActive(true);
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
