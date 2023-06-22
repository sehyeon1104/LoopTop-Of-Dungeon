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
        SetPlayerSpawnPos();
    }

    public void SetMapTypeFlag()
    {
        mapTypeFlag = GameManager.Instance.mapTypeFlag;
    }

    public void SetAndInstantiateRoom()
    {
        if(mapTypeFlag == Define.MapTypeFlag.Default)
        {
            SetMapTypeFlag();
        }

        if(_roomTypeFlag == Define.RoomTypeFlag.StartRoom)
        {
            isStartRoom = true;
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Start", transform);
        }
        else if(_roomTypeFlag == Define.RoomTypeFlag.EnemyRoom)
        {
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.0", transform);
        }
        else if(_roomTypeFlag == Define.RoomTypeFlag.EliteMobRoom)
        {
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Elite", transform);
        }
        else if(_roomTypeFlag == Define.RoomTypeFlag.EventRoom)
        {
            SetAndInstantiateEventRoom();
            // �ӽ�
            //Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Event", transform);
        }

        //    // �׽�Ʈ
        //    Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.0", transform);
        //    //Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.{Random.Range(1, 8)}", transform);
        //    //Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length)], transform);
    }

    public void SetPlayerSpawnPos()
    {
        if (IsStartRoom)
        {
            GameManager.Instance.Player.transform.position = this.transform.position;
        }
    }

    public RoomBase GetSummonedRoom()
    {
        RoomBase room = GetComponentInChildren<RoomBase>();
        return room;
    }

    private void SetAndInstantiateEventRoom()
    {
        // ���� ��ġ�� �ȵǾ��ִٸ� Ȯ����ġ
        if (StageManager.Instance.eventRoomCountDic[Define.EventRoomTypeFlag.ShopRoom] == 1)
        {
            Debug.Log("���� ��ġ");
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Shop", transform);
            StageManager.Instance.eventRoomCountDic[Define.EventRoomTypeFlag.ShopRoom]--;
            return;
        }

        // ENUM�� ������ rand
        randEventRoom = Random.Range(0, System.Enum.GetValues(typeof(Define.EventRoomTypeFlag)).Length);

        // ���� �ߺ��̶��
        if(StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom] == 0)
        {
            // �ߺ��� �ƴ� ������ rand
            while(StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom] == 0)
                randEventRoom = Random.Range(0, System.Enum.GetValues(typeof(Define.EventRoomTypeFlag)).Length);
        }

        // �̺�Ʈ�� ��ġ
        if(StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom] == 1)
        {
            Debug.Log($"{(Define.EventRoomTypeFlag)randEventRoom} ��ġ");
            //eventRoomName = randEventRoom.ToString();
            //Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.{randEventRoom.ToString().Substring(eventRoomName.Length - 4, eventRoomName.Length)}", transform);
            GameObject eventRoom = Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Event", transform);
            eventRoom.GetComponent<EventRoom>().SetEventRoomType((Define.EventRoomTypeFlag)randEventRoom);

            StageManager.Instance.eventRoomCountDic[(Define.EventRoomTypeFlag)randEventRoom]--;
        }
    }

}
