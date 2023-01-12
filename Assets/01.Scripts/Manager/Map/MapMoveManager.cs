using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMoveManager : MonoSingleton<MapMoveManager>
{
    [SerializeField]
    private GameObject[] zones;
    [SerializeField]
    private List<GameObject> passedZones = new List<GameObject>();
    [SerializeField]
    private GameObject currentZone;

    [SerializeField]
    private int maximumMoves = 0;
    int moveCount = 0;

    [SerializeField]
    private Transform[] mapDirPos;

    private enum DIR
    {
        UP = 1,
        DOWN,
        LEFT,
        RIGHT
    };

    private void Start()
    {
        SetMap();
    }

    public void SetMap()
    {
        // for(int i = 0; i < )
    }

    public void MoveMap(string dir)
    {
        Debug.Log("MoveMap");

        if(moveCount >= maximumMoves)
        {
            Debug.Log("움직일 수 없음");
            return;
        }

        moveCount++;
        int rand = Random.Range(0, zones.Length);

        DisActiveAllZones();
        zones[rand].SetActive(true);
        passedZones.Add(zones[rand]);

        SetMapDirPos(rand);
        MovePlayerMapDir(dir);
    }

    public void SetMapDirPos(int zoneNum)
    {
        Transform[] allChildren = zones[zoneNum].GetComponentsInChildren<Transform>();

        foreach(var child in allChildren)
        {
            if(child.name == "ZoneEntranceDoors")
            {
                mapDirPos = child.GetComponentsInChildren<Transform>();
            }
        }
    }

    public void DisActiveAllZones()
    {
        for (int i = 0; i < zones.Length; ++i)
        {
            zones[i].SetActive(false);
        }
    }

    public void MovePlayerMapDir(string dir)
    {
        Vector3 pos = dir switch
        {
            "up" => mapDirPos[(int)DIR.DOWN].position + Vector3.up * 3,
            "down" => mapDirPos[(int)DIR.UP].position + Vector3.down,
            "left" => mapDirPos[(int)DIR.RIGHT].position + Vector3.left,
            "right" => mapDirPos[(int)DIR.LEFT].position + Vector3.right,

            _ => Vector3.zero
        };

        PlayerMovement.Instance.transform.position = pos;
    }
}