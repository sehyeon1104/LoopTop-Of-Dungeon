using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MapSpawner Script
public class SpawnRoom : MonoBehaviour
{
    private Define.MapTypeFlag mapTypeFlag;
    private Define.RoomTypeFlag _roomTypeFlag = Define.RoomTypeFlag.Default;
    public Define.RoomTypeFlag RoomTypeFlag
    {
        get => _roomTypeFlag;
        set => _roomTypeFlag = value;
    }

    private bool isStartRoom = false;
    public bool IsStartRoom
    {
        get => isStartRoom;
        set => isStartRoom = value;
    }

    private bool isShopRoom = false;
    public bool IsShopRoom
    {
        get => isStartRoom;
        set => isShopRoom = value;
    }

    int randEventRoom = 0;
    string eventRoomName = "";

    private void Start()
    {
        SetMapTypeFlag();
        SetRoomTypeRoom();
        InstantiateRoom();
        SetPlayerSpawnPos();
    }

    public void SetMapTypeFlag()
    {
        mapTypeFlag = GameManager.Instance.mapTypeFlag;
    }

    public void SetRoomTypeRoom()
    {
        Vector3 parentPos = transform.parent.position;
        int[,] mapArr = StageManager.Instance.GetMapArr();

        _roomTypeFlag = (Define.RoomTypeFlag)mapArr[(int)(-parentPos.y / 28f), (int)(parentPos.x / 28f)];
    }

    public void InstantiateRoom()
    {
        if (mapTypeFlag == Define.MapTypeFlag.Default)
            SetMapTypeFlag();

        if (_roomTypeFlag == Define.RoomTypeFlag.StartRoom)
        {
            isStartRoom = true;
            GameObject room = Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Start", transform.parent);
            room.transform.Translate(12, 10, 0);
        }
        else if (_roomTypeFlag == Define.RoomTypeFlag.EnemyRoom)
        {
            GameObject room = Managers.Resource.Instantiate($"Assets/03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.{Random.Range(0, 4)}.prefab", transform.parent);
            room.transform.Translate(12, 10, 0);
        }
        else if (_roomTypeFlag == Define.RoomTypeFlag.EliteMobRoom)
        {
            GameObject room = Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Elite", transform.parent);
            room.transform.Translate(12, 10, 0);
        }
        else if (_roomTypeFlag == Define.RoomTypeFlag.EventRoom)
            SetAndInstantiateEventRoom();
    }

    private void SetAndInstantiateEventRoom()
    {
        // ���� ��ġ�� �ȵǾ��ִٸ� Ȯ����ġ
        if (StageManager.Instance.eventRoomCountDic[Define.EventRoomTypeFlag.ShopRoom] == 1)
        {
            //Debug.Log("���� ��ġ");
            GameObject room = Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Shop", transform.parent);
            room.transform.Translate(12, 10, 0);
            StageManager.Instance.eventRoomCountDic[Define.EventRoomTypeFlag.ShopRoom]--;
            return;
        }

        // ENUM�� ������ rand
        randEventRoom = Random.Range(0, System.Enum.GetValues(typeof(Define.EventRoomTypeFlag)).Length);

        // ���� �ߺ��̶��
        if (StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom] == 0)
        {
            // �ߺ��� �ƴ� ������ rand
            while (StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom] == 0)
                randEventRoom = Random.Range(0, System.Enum.GetValues(typeof(Define.EventRoomTypeFlag)).Length);
        }

        // �̺�Ʈ�� ��ġ
        if (StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom] == 1)
        {
            //Debug.Log($"{(Define.EventRoomTypeFlag)randEventRoom} ��ġ");
            GameObject eventRoom = Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Event", transform.parent);
            eventRoom.transform.Translate(12, 10, 0);
            eventRoom.GetComponent<EventRoom>().SetEventRoomType((Define.EventRoomTypeFlag)randEventRoom);

            StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom]--;
        }
    }

    public void SetPlayerSpawnPos()
    {
        if (IsStartRoom)
        {
            GameManager.Instance.Player.transform.position = this.transform.position;
            GameManager.Instance.Player.transform.Translate(12, 10, 0);
        }
    }

    public RoomBase GetSummonedRoom()
    {
        RoomBase room = GetComponentInChildren<RoomBase>();
        return room;
    }

}
