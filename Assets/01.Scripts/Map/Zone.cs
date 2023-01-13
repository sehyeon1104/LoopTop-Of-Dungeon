using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [field: SerializeField]
    public GameObject upZone { private set; get; } = null;
    [field: SerializeField]
    public GameObject downZone { private set; get; } = null;
    [field: SerializeField]
    public GameObject leftZone { private set; get; } = null;
    [field: SerializeField]
    public GameObject rightZone { private set; get; } = null;

    // TODO : �������� �� �ٽ� ���ư� �� �ְԲ� �ϱ�

    public void SetPreZone(string dir, GameObject zone)
    {
        GameObject preZone = dir switch
        {
            "up" => downZone = zone,
            "down" => upZone = zone,
            "left" => rightZone = zone,
            "right" => leftZone = zone,

            _ => null
        };
    }

}
