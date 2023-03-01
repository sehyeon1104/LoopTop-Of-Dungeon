using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapTest : MonoBehaviour
{
    void Start()
    {
        Managers.Resource.Instantiate($"03.Prefabs/Maps/Lava/Lava_MonsterZone{Random.Range(1,5)}",transform);
    }

}
