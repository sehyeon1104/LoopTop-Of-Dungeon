using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField]
    private GameObject[] zones;
    [SerializeField]
    private List<GameObject> passedZones = new List<GameObject>();
    [SerializeField]
    private GameObject currentZone;
    [SerializeField]
    private bool isClear = false;

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
        // SetZone();
    }

    public void SetZone(/*string dir*/)
    {
        // Zone curZone = FindObjectOfType<Zone>();


        int rand = Random.Range(0, zones.Length);

        GameObject newZone = Instantiate(zones[rand], Vector3.zero, Quaternion.identity);

    }

    public void MoveMap(string dir)
    {
        if (!isClear)
        {
            Debug.Log($"isClear : {isClear}");
        }
        Debug.Log("MoveMap");

        //int rand = Random.Range(0, zones.Length);

        moveCount++;
        moveCount %= 2;
        DisActiveAllZones();
        zones[moveCount].SetActive(true);
        //passedZones.Add(zones[rand]);

        SetMapDirPos(moveCount);
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
            "up" => mapDirPos[(int)DIR.DOWN].position + Vector3.up * 2,
            "down" => mapDirPos[(int)DIR.UP].position + Vector3.down * 2,
            "left" => mapDirPos[(int)DIR.RIGHT].position + Vector3.left * 2,
            "right" => mapDirPos[(int)DIR.LEFT].position + Vector3.right * 2,

            _ => Vector3.zero
        };

        PlayerMovement.Instance.transform.position = pos;
    }
}