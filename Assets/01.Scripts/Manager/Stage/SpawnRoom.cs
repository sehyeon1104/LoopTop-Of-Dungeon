using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MapSpawner Script
public class SpawnRoom : MonoBehaviour
{
    private Define.MapTypeFlag mapTypeFlag;

    [SerializeField]
    private GameObject[] mapPrefabs;

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

    private bool isShopRoom = false;
    public bool IsShopRoom
    {
        get
        {
            return isShopRoom;
        }
        set
        {
            isShopRoom = value;
        }
    }

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

        if (isStartRoom)
        {
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Start", transform);
        }
        else if (isShopRoom)
        {
            Managers.Resource.Instantiate($"03.Prefabs/Maps/{mapTypeFlag}/{mapTypeFlag}FieldNormal.Shop", transform);
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
            GameManager.Instance.Player.transform.position = this.transform.position;
            // isSetPlayerPos = true;
        }
    }

    public RoomBase GetSummonedRoom()
    {
        Debug.Log("GetSummonedRoom");
        RoomBase room = GetComponentInChildren<RoomBase>();
        Debug.Log($"return {room}");
        return room;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.minimapCamera.MoveMinimapCamera(transform.position);
    }

}
