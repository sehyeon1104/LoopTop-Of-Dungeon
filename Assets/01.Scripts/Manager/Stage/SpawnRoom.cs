using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoSingleton<SpawnRoom>
{
    [SerializeField]
    private Define.MapTypeFlag mapTypeFlag;

    [SerializeField]
    private GameObject[] mapPrefabs;
    private int mapCount = 0;

    private bool isStartRoom = false;
    public bool IsStartRoom
    {
        get
        {
            return isStartRoom;
        }
        set
        {
            isStartRoom = value;
        }
    }

    private void SpawnRooms()
    {
        SetRoomPrefabs();
        SetRoom();
    }

    private void SetRoomPrefabs()
    {
        mapCount = (int)((Directory.GetFiles($"03.Prefabs/Maps/{mapTypeFlag}").Length / 2) - 0.5f);
        mapPrefabs = new GameObject[Directory.GetFiles($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.{Random.Range(1, mapCount)}").Length / 2];
    }

    public void SetRoom()
    {
        if (isStartRoom)
        {
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Start", transform);
        }
        else
        {
            Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length)], transform);
        }
    }

}
