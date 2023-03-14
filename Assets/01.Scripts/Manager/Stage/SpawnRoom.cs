using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    [SerializeField]
    private Define.MapTypeFlag mapTypeFlag;

    [SerializeField]
    private GameObject[] mapPrefabs;
    private int mapCount = 0;

    private bool isSetPlayerPos = false;

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

    private void Start()
    {
        SetMapTypeFlag();
        SetRoomPrefabs();
        SetPlayerSpawnPos();
    }

    public void SetMapTypeFlag()
    {
        // 이후 현재 들어간 맵에 따라 타입 변경
        mapTypeFlag = Define.MapTypeFlag.Ghost;
    }

    public void SetRoomPrefabs()
    {
        mapCount = (int)((Directory.GetFiles($"Assets/03.Prefabs/Maps/{mapTypeFlag}").Length / 2));
        //mapPrefabs = new GameObject[mapCount - 2];
    }

    public void SetAndInstantiateRoom()
    {
        if(mapTypeFlag == Define.MapTypeFlag.Default)
        {
            SetMapTypeFlag();
        }

        if (isStartRoom)
        {
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Start", transform);
        }
        else
        {
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.{Random.Range(1, 8)}", transform);
            //Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length)], transform);
        }
    }

    public void SetPlayerSpawnPos()
    {
        if (IsStartRoom)
        {
            Player.Instance.transform.position = this.transform.position;
            isSetPlayerPos = true;
        }
    }

}
