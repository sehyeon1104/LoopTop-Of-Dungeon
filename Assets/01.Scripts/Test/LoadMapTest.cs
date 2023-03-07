using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Stage
{
    Power,
    Ghost,
    Lava,
    Electric,
    Wolf,
    Lizard
}

public class LoadMapTest : MonoBehaviour
{
    [Space]
    [SerializeField] private Stage stage;

    void Start()
    {
        Managers.Resource.Instantiate($"03.Prefabs/Maps/{stage}/{stage}FieldNormal.{Random.Range(1,8)}",transform);
    }
}
